using Ghost.Data;
using Ghost.Dtos;
using Ghost.Repository;

namespace Ghost.Services;

public class PlaylistService : IPlaylistService
{
  private readonly IPlaylistRepository playlistRepository;

  public PlaylistService(IPlaylistRepository playlistRepository)
  {
    this.playlistRepository = playlistRepository;
  }

  public async Task<List<PlaylistDto>> GetPlaylists(int userId)
  {
    var playlists = await playlistRepository.GetPlaylists(userId);

    return playlists.Select(p => new PlaylistDto(p)).ToList();
  }

  public async Task<PlaylistDto?> GetPlaylistById(int userId, int id)
  {
    var playlist = await playlistRepository.GetPlaylistById(userId, id);

    if (playlist == null)
    {
      return null;
    }

    return new PlaylistDto(playlist);
  }

  public async Task<PlaylistDto> CreatePlaylist(int userId, CreatePlaylistDto playlistDto)
  {
    var playlist = new Playlist
    {
      Name = playlistDto.Name,
      User = new User { Id = userId }
    };

    var createdPlaylist = await playlistRepository.CreatePlaylist(playlist);

    return new PlaylistDto(createdPlaylist);
  }

  public async Task DeletePlaylist(int id)
  {
    await playlistRepository.DeletePlaylist(id);
  }
}