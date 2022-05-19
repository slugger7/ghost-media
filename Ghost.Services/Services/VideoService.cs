using Ghost.Data;
using Ghost.Dtos;
using Ghost.Media;
using Ghost.Repository;
using Microsoft.Extensions.Logging;

namespace Ghost.Services
{
  public class VideoService : IVideoService
  {
    private readonly ILogger<VideoService> logger;
    private readonly IGenreService genreService;
    private readonly IActorService actorService;
    private readonly IGenreRepository genreRepository;
    private readonly IVideoRepository videoRepository;
    private readonly IActorRepository actorRepository;
    private readonly IImageIoService imageIoService;
    private readonly IImageService imageService;
    private readonly INfoService nfoService;

    public VideoService(
      ILogger<VideoService> logger,
      IGenreService genreService,
      IActorService actorService,
      IGenreRepository genreRepository,
      IVideoRepository videoRepository,
      IActorRepository actorRepository,
      IImageIoService imageIoService,
      IImageService imageService,
      INfoService nfoService)
    {
      this.logger = logger;
      this.genreService = genreService;
      this.actorService = actorService;
      this.genreRepository = genreRepository;
      this.videoRepository = videoRepository;
      this.actorRepository = actorRepository;
      this.imageIoService = imageIoService;
      this.imageService = imageService;
      this.nfoService = nfoService;
    }

    public PageResultDto<VideoDto> SearchVideos(PageRequestDto pageRequest)
    {
      var videosPage = videoRepository.SearchVideos(
        pageRequest.Page,
        pageRequest.Limit,
        pageRequest.Search,
        pageRequest.SortBy,
        pageRequest.Ascending);
      return new PageResultDto<VideoDto>
      {
        Total = videosPage.Total,
        Page = videosPage.Page,
        Content = videosPage.Content.Select(v => new VideoDto(v)).ToList()
      };
    }

    public VideoDto GetVideoById(int id)
    {
      var video = videoRepository.FindById(id);
      if (video == null) throw new NullReferenceException("Video not found");

      return new VideoDto(video);
    }

    public string GenerateThumbnail(int id)
    {
      var video = videoRepository.FindById(id);

      if (video == null) throw new NullReferenceException("Video not found");
      if (video.Path == null) throw new NullReferenceException("Path was null");

      var outputPath = ImageIoService.GenerateFileName(video.Path, ".png");

      imageIoService.GenerateImage(video.Path, outputPath);

      return outputPath;
    }

    public async Task<VideoDto> UpdateMetadData(int id)
    {
      var video = videoRepository.FindById(id);

      if (video == null) throw new NullReferenceException("Video not found");
      if (video.Path == null) throw new NullReferenceException("Video has no path");

      var metaData = VideoFns.GetVideoInformation(video.Path);
      if (metaData is null) throw new NullReferenceException("Video meta data not found");
      video.Created = metaData.Created;
      video.Size = metaData.Size;
      video.Height = metaData.Height;
      video.Width = metaData.Width;
      video.Runtime = metaData.Duration.TotalMilliseconds;
      video.LastMetadataUpdate = DateTime.UtcNow;

      video = await videoRepository.UpdateVideo(video);
      if (video is null) throw new NullReferenceException("Video was not found after updating");

      return new VideoDto(video);
    }

    public VideoDto SetGenresByNameToVideo(int id, List<string> genres)
    {
      if (genres == null) throw new NullReferenceException("Genres not provided");
      var videoEntity = videoRepository.FindById(id);

      if (videoEntity == null) throw new NullReferenceException("Video not found");
      var genreEntities = genres.Select(g => genreRepository.Upsert(g));

      videoEntity = videoRepository.SetGenres(videoEntity.Id, genreEntities);
      return new VideoDto(videoEntity);
    }

    public PageResultDto<VideoDto> GetVideosForGenre(string name, PageRequestDto pageRequest)
    {
      var videosPage = videoRepository.GetForGenre(
        name,
        pageRequest.Page,
        pageRequest.Limit,
        pageRequest.Search,
        pageRequest.SortBy,
        pageRequest.Ascending);
      return new PageResultDto<VideoDto>
      {
        Total = videosPage.Total,
        Page = videosPage.Page,
        Content = videosPage.Content.Select(v => new VideoDto(v)).ToList()
      };
    }

    public VideoDto SetActorsByNameToVideo(int id, List<string> actors)
    {
      if (actors == null) throw new NullReferenceException("Actors not provided");
      var videoEntity = videoRepository.FindById(id);

      if (videoEntity == null) throw new NullReferenceException("Video not found");
      var actorEntities = actors.Select(a => actorRepository.UpsertActor(a));

      videoEntity = videoRepository.SetActors(videoEntity.Id, actorEntities);

      return new VideoDto(videoEntity);
    }

    public PageResultDto<VideoDto> GetVideosForActor(int actorId, PageRequestDto pageRequest)
    {
      var videosPage = videoRepository.GetForActor(
        actorId,
        pageRequest.Page,
        pageRequest.Limit,
        pageRequest.Search,
        pageRequest.SortBy,
        pageRequest.Ascending);
      return new PageResultDto<VideoDto>
      {
        Total = videosPage.Total,
        Page = videosPage.Page,
        Content = videosPage.Content.Select(v => new VideoDto(v)).ToList()
      };
    }

    public async Task<VideoDto> UpdateTitle(int id, string title)
    {
      var video = await videoRepository.UpdateTitle(id, title);

      return new VideoDto(video);
    }

    public async Task<VideoDto> SyncWithNFO(int id)
    {
      var video = videoRepository.FindById(id);
      if (video == null) throw new NullReferenceException("Video not found");

      var videoNfo = nfoService.HydrateVideo(video);
      if (videoNfo != null)
      {
        video.Title = videoNfo.title;
        video.LastNfoScan = DateTime.UtcNow;
        DateTime DateAdded;
        if (DateTime.TryParse(videoNfo.dateadded, out DateAdded))
        {
          video.DateAdded = DateAdded;
        }
        video = await videoRepository.UpdateVideo(video);
        this.SetActorsByNameToVideo(id, videoNfo.actors.Select(a => a.name).ToList());
        this.SetGenresByNameToVideo(id, videoNfo.genres);
      }

      return new VideoDto(video);
    }

    public async Task DeletePermanently(int id)
    {
      var video = await videoRepository.Delete(id);
      File.Delete(video.Path);
    }

    public async Task BatchSyncNfos(List<Video> videos)
    {
      var videoGenreDictionary = new Dictionary<int, List<VideoGenre>>();
      var videoActorDictionary = new Dictionary<int, List<VideoActor>>();
      var newVideos = videos
        .Select(video =>
      {
        logger.LogDebug("Video: {0}", video.Title);
        var videoNfo = nfoService.HydrateVideo(video);
        if (videoNfo != null)
        {
          logger.LogDebug("Video NFO: {0}", videoNfo.title);
          video.Title = videoNfo.title.Trim();
          video.LastNfoScan = DateTime.UtcNow;
          DateTime DateAdded;
          if (DateTime.TryParse(videoNfo.dateadded, out DateAdded))
          {
            video.DateAdded = DateAdded;
          }
          videoActorDictionary.Add(video.Id, videoNfo.actors.Select(actor => new VideoActor
          {
            Actor = new Actor { Name = actor.name.Trim() },
            Video = video
          })
          .ToList());
          videoGenreDictionary.Add(video.Id, videoNfo.genres.Select(genre => new VideoGenre
          {
            Genre = new Genre { Name = genre.Trim() },
            Video = video
          })
          .ToList());
        }
        return video;
      });

      logger.LogDebug("Done batch sync");
      await videoRepository.BatchUpdate(newVideos, videoGenreDictionary, videoActorDictionary);
    }
  }
}