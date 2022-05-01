using Ghost.Dtos;

namespace Ghost.Services
{
  public interface IVideoService
  {
    PageResultDto<VideoDto> SearchVideos(PageRequestDto pageRequest);
    VideoDto GetVideoById(int id);
    Task DeletePermanently(int id);
    string GenerateThumbnail(int id);
    Task<VideoDto> UpsertMetaData(int id);
    VideoDto SetGenresByNameToVideo(int id, List<string> genres);
    PageResultDto<VideoDto> GetVideosForGenre(string genre, PageRequestDto pageRequest);
    VideoDto SetActorsByNameToVideo(int id, List<string> actors);
    PageResultDto<VideoDto> GetVideosForActor(int actorId, PageRequestDto pageRequest);
    Task<VideoDto> UpdateTitle(int id, string title);
    Task<VideoDto> SyncWithNFO(int id);
  }
}