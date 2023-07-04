using Ghost.Data;
using Ghost.Dtos;

namespace Ghost.Services
{
    public interface IVideoService
    {
        PageResultDto<VideoDto> SearchVideos(PageRequestDto pageRequest, FilterQueryDto filterQuery, int userId);
        VideoDto GetVideoById(int id, int userId);
        VideoDto GetVideoById(int id, int userId, List<string>? includes);
        VideoDto GetVideoInfo(int id, int userId);
        Task DeletePermanently(int id);
        string GenerateThumbnail(int id);
        Task<VideoDto> UpdateMetaData(int id);
        VideoDto SetGenresByNameToVideo(int id, List<string> genres);
        PageResultDto<VideoDto> GetVideosForGenre(string genre, int userId, PageRequestDto pageRequest, FilterQueryDto filters);
        VideoDto SetActorsByNameToVideo(int id, List<string> actors);
        PageResultDto<VideoDto> GetVideosForActor(int actorId, int userId, PageRequestDto pageRequest, FilterQueryDto filters);
        Task<VideoDto> UpdateTitle(int id, string title);
        Task<VideoDto> SyncWithNFO(int id);
        Task BatchSyncNfos(List<Video> videos);
        Task<VideoDto> GenerateChapters(int id, bool overwrite = false);
        Task LogProgress(int id, int userId, ProgressUpdateDto progress);
        PageResultDto<VideoDto> Favourites(int userId, PageRequestDto pageRequest, FilterQueryDto filters);
        VideoDto Random(int userId, RandomVideoRequestDto randomVideoRequest);
        VideoDto GetRandomVideoForGenre(string genre, int userId, RandomVideoRequestDto randomVideoRequest);
        VideoDto GetRandomVideoForActor(int id, int userId, RandomVideoRequestDto randomVideoRequest);
        VideoDto GetRandomVideoFromFavourites(int userId, RandomVideoRequestDto randomVideoRequest);
        Task<List<VideoDto>> RelateVideo(int id, int relateTo);
        Task<List<VideoDto>> DeleteRelation(int id, int relatedTo);
        Task CreateSubVideo(int id, int userId, SubVideoRequestDto subVideoRequest);
        void Convert(int id, ConvertRequestDto convertRequest);
    }
}