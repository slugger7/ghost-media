using Ghost.Dtos;

namespace Ghost.Services
{
  public interface IVideoService
  {
    PageResultDto<VideoDto> SearchVideos(PageRequestDto pageRequest);
    VideoDto GetVideoById(int id);
    string GenerateThumbnail(int id);
    VideoMetaDataDto? GetVideoMetaData(int id);
    VideoDto AddGenresByNameToVideo(int id, List<string> genres);
    PageResultDto<VideoDto> GetVideosForGenre(string genre, PageRequestDto pageRequest);
    VideoDto AddActorsByNameToVideo(int id, List<string> actors);
    PageResultDto<VideoDto> GetVideosForActor(int actorId, PageRequestDto pageRequest);
    Task<VideoDto> UpdateTitle(int id, string title);
  }
}