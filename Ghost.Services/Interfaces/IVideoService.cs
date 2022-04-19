using Ghost.Dtos;

namespace Ghost.Services.Interfaces
{
  public interface IVideoService
  {
    PageResultDto<VideoDto> GetVideos(int page, int limit);
    VideoDto GetVideoById(string id);
    string GenerateThumbnail(string id);
    VideoMetaDataDto? GetVideoMetaData(string id);
    VideoDto AddGenresByNameToVideo(string id, List<string> genres);
    PageResultDto<VideoDto> GetVideosForGenre(string genre, int page, int limit);
    VideoDto AddActorsByNameToVideo(string id, List<string> actors);
  }
}