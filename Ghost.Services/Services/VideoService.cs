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

    public VideoService(
      IGenreService genreService,
      IActorService actorService,
      IGenreRepository genreRepository,
      IVideoRepository videoRepository,
      IActorRepository actorRepository)
    {
      this.genreService = genreService;
      this.actorService = actorService;
      this.genreRepository = genreRepository;
      this.videoRepository = videoRepository;
      this.actorRepository = actorRepository;
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

      var basePath = video.Path
        .Substring(0, video.Path.LastIndexOf(Path.DirectorySeparatorChar)) + Path.DirectorySeparatorChar + video.Title + ".png";

      ImageFns.GenerateImage(video.Path, basePath);

      return basePath;
    }

    public VideoMetaDataDto? GetVideoMetaData(int id)
    {
      var video = GetVideoById(id);

      if (video == null) throw new NullReferenceException("Video not found");
      if (video.Path == null) return default;

      return VideoFns.GetVideoInformation(video.Path);
    }

    public VideoDto AddGenresByNameToVideo(int id, List<string> genres)
    {
      if (genres == null) throw new NullReferenceException("Genres not provided");
      var videoEntity = videoRepository.FindById(id);

      if (videoEntity == null) throw new NullReferenceException("Video not found");
      var genreEntities = genres.Select(g => genreRepository.Upsert(g));

      var newGenres = genreEntities.Where(g => !videoEntity.VideoGenres.Any(vg => vg.Genre.Id == g.Id));

      videoEntity = videoRepository.AddGenres(videoEntity.Id, newGenres);
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

    public VideoDto AddActorsByNameToVideo(int id, List<string> actors)
    {
      if (actors == null) throw new NullReferenceException("Actors not provided");
      var videoEntity = videoRepository.FindById(id);

      if (videoEntity == null) throw new NullReferenceException("Video not found");
      var actorEntities = actors.Select(a => actorRepository.UpsertActor(a));

      var newActors = actorEntities.Where(a => !videoEntity.VideoActors.Any(va => va.Actor.Id == a.Id));

      videoEntity = videoRepository.AddActors(videoEntity.Id, newActors);

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
  }
}