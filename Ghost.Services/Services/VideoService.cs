using Ghost.Dtos;
using Ghost.Media;
using Ghost.Repository;

namespace Ghost.Services
{
  public class VideoService : IVideoService
  {
    private readonly IGenreService genreService;
    private readonly IActorService actorService;
    private readonly IGenreRepository genreRepository;
    private readonly IVideoRepository videoRepository;
    private readonly IActorRepository actorRepository;
    private readonly IImageIoService imageIoService;
    private readonly INfoService nfoService;

    public VideoService(
      IGenreService genreService,
      IActorService actorService,
      IGenreRepository genreRepository,
      IVideoRepository videoRepository,
      IActorRepository actorRepository,
      IImageIoService imageIoService,
      INfoService nfoService)
    {
      this.genreService = genreService;
      this.actorService = actorService;
      this.genreRepository = genreRepository;
      this.videoRepository = videoRepository;
      this.actorRepository = actorRepository;
      this.imageIoService = imageIoService;
      this.nfoService = nfoService;
    }

    public PageResultDto<VideoDto> SearchVideos(PageRequestDto pageRequest)
    {
      var videosPage = videoRepository.SearchVideos(pageRequest.Page, pageRequest.Limit, pageRequest.Search);
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

      var outputPath = ImageIoService.GenerateFileName(video.Path, video.Title, ".png");

      imageIoService.GenerateImage(video.Path, outputPath);

      return outputPath;
    }

    public VideoMetaDataDto? GetVideoMetaData(int id)
    {
      var video = GetVideoById(id);

      if (video == null) throw new NullReferenceException("Video not found");
      if (video.Path == null) return default;

      return VideoFns.GetVideoInformation(video.Path);
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
      var videosPage = videoRepository.GetForGenre(name, pageRequest.Page, pageRequest.Limit, pageRequest.Search);
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
      var videosPage = videoRepository.GetForActor(actorId, pageRequest.Page, pageRequest.Limit, pageRequest.Search);
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

      var vdieoNfo = nfoService.HydrateVideo(video);
      if (vdieoNfo != null)
      {
        this.SetActorsByNameToVideo(id, vdieoNfo.actors.Select(a => a.name).ToList());
        this.SetGenresByNameToVideo(id, vdieoNfo.genres);
        await this.UpdateTitle(id, vdieoNfo.title);
        video = videoRepository.FindById(id);
        if (video == null) throw new NullReferenceException("Video not found");
      }


      return new VideoDto(video);
    }
  }
}