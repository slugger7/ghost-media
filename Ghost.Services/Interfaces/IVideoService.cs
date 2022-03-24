using Ghost.Dtos;

namespace Ghost.Services.Interfaces
{
  public interface IVideoService
  {
    void UpsertVideos(List<string> videos);
    PageResultDto<VideoDto> GetVideos(int page, int limit);
    VideoDto GetVideoById(string id);
    List<string> RefreshVideos();
  }
}