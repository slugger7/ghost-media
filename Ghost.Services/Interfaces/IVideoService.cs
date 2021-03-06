using Ghost.Data;
using Ghost.Dtos;

namespace Ghost.Services
{
  public interface IVideoService
  {
    PageResultDto<VideoDto> SearchVideos(PageRequestDto pageRequest, int userId);
    VideoDto GetVideoById(int id, int userId);
    VideoDto GetVideoById(int id, int userId, List<string>? includes);
    VideoDto GetVideoInfo(int id, int userId);
    Task DeletePermanently(int id);
    string GenerateThumbnail(int id);
    Task<VideoDto> UpdateMetadData(int id);
    VideoDto SetGenresByNameToVideo(int id, List<string> genres);
    PageResultDto<VideoDto> GetVideosForGenre(string genre, int userId, PageRequestDto pageRequest);
    VideoDto SetActorsByNameToVideo(int id, List<string> actors);
    PageResultDto<VideoDto> GetVideosForActor(int actorId, int userId, PageRequestDto pageRequest);
    Task<VideoDto> UpdateTitle(int id, string title);
    Task<VideoDto> SyncWithNFO(int id);
    Task BatchSyncNfos(List<Video> videos);
    Task<VideoDto> GenerateChapters(int id, bool overwrite = false);
    Task LogProgress(int id, int userId, double progress);
    Task<VideoDto> ResetProgress(int id, int userId);
    PageResultDto<VideoDto> Favourites(int userId, PageRequestDto pageRequest);
  }
}