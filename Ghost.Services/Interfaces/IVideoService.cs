using Ghost.Data;
using Ghost.Dtos;

namespace Ghost.Services
{
  public interface IVideoService
  {
    PageResultDto<VideoDto> SearchVideos(PageRequestDto pageRequest);
    VideoDto GetVideoById(int id);
    VideoDto GetVideoById(int id, List<string>? includes);
    Task DeletePermanently(int id);
    string GenerateThumbnail(int id);
    Task<VideoDto> UpdateMetadData(int id);
    VideoDto SetGenresByNameToVideo(int id, List<string> genres);
    PageResultDto<VideoDto> GetVideosForGenre(string genre, PageRequestDto pageRequest);
    VideoDto SetActorsByNameToVideo(int id, List<string> actors);
    PageResultDto<VideoDto> GetVideosForActor(int actorId, PageRequestDto pageRequest);
    Task<VideoDto> UpdateTitle(int id, string title);
    Task<VideoDto> SyncWithNFO(int id);
    Task BatchSyncNfos(List<Video> videos);
    Task<VideoDto> GenerateChapters(int id, bool overwrite = false);
  }
}