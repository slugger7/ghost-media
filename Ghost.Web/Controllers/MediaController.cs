using Microsoft.AspNetCore.Mvc;

using Ghost.Dtos;
using Ghost.Services;
using Microsoft.AspNetCore.Authorization;

namespace Ghost.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : Controller
    {
        private readonly IVideoService videoService;
        private readonly ILogger<MediaController> logger;

        public MediaController(ILogger<MediaController> logger, IVideoService videoService)
        {
            this.logger = logger;
            this.videoService = videoService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<PageResultDto<VideoDto>> SearchVideos(
          [FromQuery] PageRequestDto pageRequest,
          [FromQuery] FilterQueryDto filters,
          [FromHeader(Name = "User-Id")] int userId)
        {
            return videoService.SearchVideos(pageRequest, filters, userId);
        }

        [HttpPut("{id}/title")]
        [Authorize]
        public async Task<ActionResult<VideoDto>> UpdateTitle(int id, [FromBody] TitleUpdateDto titleUpdate)
        {
            try
            {
                return await videoService.UpdateTitle(id, titleUpdate.Title);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}/info")]
        [Authorize]
        public ActionResult<VideoDto> GetVideoInfo(int id, [FromHeader(Name = "User-Id")] int userId)
        {
            var video = videoService.GetVideoInfo(id, userId);

            return video;
        }

        [HttpPut("{id}/metadata")]
        [Authorize]
        public async Task<ActionResult<VideoDto>> GetVideoMetaData(int id)
        {
            try
            {
                var videoInfo = await videoService.UpdateMetaData(id);
                if (videoInfo == null) return NoContent();
                return videoInfo;
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetVideo(int id, [FromHeader(Name = "User-Id")] int userId)
        {
            try
            {
                var video = videoService.GetVideoById(id, userId, null);
                if (video != null)
                {
                    return PhysicalFile(video.Path ?? "", "video/mp4", true);
                }
                return NotFound();
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteVideo(int id)
        {
            try
            {
                await videoService.DeletePermanently(id);
                return NoContent();
            }
            catch (NullReferenceException)
            {
                logger.LogWarning("Video was not found: {id}", id);
                return NotFound();
            }
        }

        [HttpPut("{id}/genres")]
        [Authorize]
        public ActionResult<VideoDto> AddGenresByNameToVideo(int id, [FromBody] GenreAddDto genreAddDto)
        {
            try
            {
                return this.videoService.SetGenresByNameToVideo(id, genreAddDto.Genres);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpGet("genre/{genre}")]
        [Authorize]
        public ActionResult<PageResultDto<VideoDto>> GetVideosForGenre(
          string genre,
          [FromQuery] PageRequestDto pageRequest,
          [FromQuery] FilterQueryDto filters,
          [FromHeader(Name = "User-Id")] int userId)
        {
            return videoService.GetVideosForGenre(genre, userId, pageRequest, filters);
        }

        [HttpGet("genre/{genre}/random")]
        [Authorize]
        public ActionResult<VideoDto> GetRandomVideoForGenre(
            string genre,
            [FromHeader(Name = "User-Id")] int userId,
            [FromQuery] RandomVideoRequestDto randomVideoRequest
        )
        {
            return videoService.GetRandomVideoForGenre(genre, userId, randomVideoRequest);
        }

        [HttpPut("{id}/actors")]
        [Authorize]
        public ActionResult<VideoDto> AddActorsByNameToVideo(int id, [FromBody] ActorAddDto actorAddDto)
        {
            try
            {
                return this.videoService.SetActorsByNameToVideo(id, actorAddDto.Actors);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpGet("actor/{id}")]
        [Authorize]
        public ActionResult<PageResultDto<VideoDto>> GetVideosForActor(
          int id,
          [FromQuery] PageRequestDto pageRequest,
          [FromQuery] FilterQueryDto filters,
          [FromHeader(Name = "User-Id")] int userId)
        {
            try
            {
                return videoService.GetVideosForActor(id, userId, pageRequest, filters);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}/nfo")]
        [Authorize]
        public async Task<ActionResult<VideoDto>> UpdateFromNFO(int id)
        {
            try
            {
                return await videoService.SyncWithNFO(id);
            }
            catch (NullReferenceException)
            {
                logger.LogWarning("Video was not found when updating from NFO {0}", id);
                return NotFound();
            }
        }

        [HttpPut("{id}/chapters")]
        [Authorize]
        public async Task<ActionResult<VideoDto>> GenerateChapters(int id, bool overwrite = false)
        {
            try
            {
                return await videoService.GenerateChapters(id, overwrite);
            }
            catch (NullReferenceException)
            {
                logger.LogWarning("Video was not found when generating chapters {0}", id);
                return NotFound();
            }
        }

        [HttpPut("{id}/progress")]
        [Authorize]
        public async Task<ActionResult> LogProgress(
          int id,
          [FromHeader(Name = "User-Id")] int userId,
          [FromBody] ProgressUpdateDto progress)
        {
            try
            {
                await videoService.LogProgress(id, userId, progress);
                return Ok();
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpGet("favourites")]
        [Authorize]
        public ActionResult<PageResultDto<VideoDto>> GetFavourites(
          [FromHeader(Name = "User-Id")] int userId,
          [FromQuery] PageRequestDto pageRequest,
          [FromQuery] FilterQueryDto filterQueryDto
        )
        {
            try
            {
                return videoService.Favourites(userId, pageRequest, filterQueryDto);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpGet("random")]
        [Authorize]
        public ActionResult<VideoDto> RandomVideo(
            [FromHeader(Name = "User-Id")] int userId,
            [FromQuery] RandomVideoRequestDto randomVideoRequest
        )
        {
            try
            {
                //Console.WriteLine(randomVideoRequest.Genres.ToString());
                return videoService.Random(userId, randomVideoRequest);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Random video not found");
                return NotFound();
            }
        }
    }
}