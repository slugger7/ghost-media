using Ghost.Dtos;
using Ghost.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ghost.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlaylistController : BaseController
{
  private readonly ILogger<PlaylistController> logger;
  private readonly IPlaylistService playlistService;

  public PlaylistController(ILogger<PlaylistController> logger, IPlaylistService playlistService)
  {
    this.logger = logger;
    this.playlistService = playlistService;
  }

  [HttpGet]
  [Authorize]
  public async Task<ActionResult<List<PlaylistDto>>> GetPlaylists()
  {
    return await playlistService.GetPlaylists(UserId);
  }

  [HttpGet("{id}")]
  [Authorize]
  public async Task<ActionResult<PlaylistDto>> GetPlaylistById(int id)
  {
    var playlist = await playlistService.GetPlaylistById(UserId, id);

    if (playlist == null)
    {
      return NotFound();
    }

    return playlist;
  }

  [HttpGet("{id}/videos")]
  [Authorize]
  public ActionResult<PageResultDto<VideoDto>> GetVideos(
    int id,
    [FromQuery] PageRequestDto pageRequest,
    [FromQuery] FilterQueryDto filters)
  {
    var videos = playlistService.GetVideos(id, UserId, pageRequest, filters);

    return videos;
  }

  [HttpPost]
  [Authorize]
  public async Task<ActionResult<PlaylistDto>> CreatePlaylist(
    CreatePlaylistDto playlistDto)
  {
    var playlist = await playlistService.CreatePlaylist(UserId, playlistDto);

    return CreatedAtAction(nameof(GetPlaylistById), new { id = playlist.Id }, playlist);
  }

  [HttpDelete("{id}")]
  [Authorize]
  public async Task<ActionResult> DeletePlaylist(int id)
  {
    await playlistService.DeletePlaylist(id);

    return NoContent();
  }

  [HttpPut("{id}")]
  [Authorize]
  public async Task<ActionResult<PlaylistDto>> UpdatePlaylist(
    int id,
    UpdatePlaylistDto playlistDto)
  {
    try
    {
      var playlist = await playlistService.UpdatePlaylist(UserId, id, playlistDto);

      return Ok(playlist);
    }
    catch (NullReferenceException e)
    {
      return NotFound(e.Message);
    }
  }

  [HttpPost("{id}/videos")]
  [Authorize]
  public async Task<ActionResult<PlaylistDto>> AddVideoToPlaylist(
    int id,
    AddVideosToPlaylistDto addVideosToPlaylistDto)
  {
    try
    {
      var playlist = await playlistService.AddVideosToPlaylist(UserId, id, addVideosToPlaylistDto);

      return Ok(playlist);
    }
    catch (NullReferenceException e)
    {
      return NotFound(e.Message);
    }
  }

  [HttpDelete("{id}/videos/{videoId}")]
  [Authorize]
  public async Task<ActionResult<PlaylistDto>> RemoveVideoFromPlaylist(
    int id,
    int videoId)
  {
    try
    {
      var playlist = await playlistService.RemoveVideoFromPlaylist(UserId, id, videoId);

      return Ok(playlist);
    }
    catch (NullReferenceException e)
    {
      return NotFound(e.Message);
    }
  }
}