using Ghost.Dtos;

namespace Ghost.Services.Interfaces
{
  public interface IVideoService
  {
    PageResultDto<VideoDto> GetVideos(int page, int limit);
    VideoDto GetVideoById(string id);
  }
}