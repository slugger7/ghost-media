using Ghost.Dtos;
using Ghost.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ghost.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlaylistController : Controller
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
  public async Task<ActionResult<List<PlaylistDto>>> GetPlaylists([FromHeader(Name = "User-Id")] int userId)
  {
    return await playlistService.GetPlaylists(userId);
  }

  [HttpGet("{id}")]
  [Authorize]
  public async Task<ActionResult<PlaylistDto>> GetPlaylistById([FromHeader(Name = "User-Id")] int userId, int id)
  {
    var playlist = await playlistService.GetPlaylistById(userId, id);

    if (playlist == null)
    {
      return NotFound();
    }

    return playlist;
  }

  [HttpPost]
  [Authorize]
  public async Task<ActionResult<PlaylistDto>> CreatePlaylist(
    [FromHeader(Name = "User-Id")] int userId,
    CreatePlaylistDto playlistDto)
  {
    var playlist = await playlistService.CreatePlaylist(userId, playlistDto);

    return CreatedAtAction(nameof(GetPlaylistById), new { id = playlist.Id }, playlist);
  }

  [HttpDelete("{id}")]
  [Authorize]
  public async Task<ActionResult> DeletePlaylist(int id)
  {
    await playlistService.DeletePlaylist(id);

    return NoContent();
  }
}