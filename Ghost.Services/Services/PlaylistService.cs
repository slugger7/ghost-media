using Ghost.Data;
using Ghost.Dtos;
using Ghost.Repository;

namespace Ghost.Services;

public class PlaylistService : IPlaylistService
{
  private readonly IPlaylistRepository playlistRepository;
  private readonly IUserRepository userRepository;

  public PlaylistService(IPlaylistRepository playlistRepository, IUserRepository userRepository)
  {
    this.playlistRepository = playlistRepository;
    this.userRepository = userRepository;
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
    var user = userRepository.FindById(userId);

    if (user == null)
    {
      throw new NullReferenceException($"User with id {userId} not found");
    }

    var playlist = new Playlist
    {
      Name = playlistDto.Name,
      User = user,
      CreatedAt = DateTime.UtcNow,
    };

    var createdPlaylist = await playlistRepository.CreatePlaylist(playlist);

    return new PlaylistDto(createdPlaylist);
  }

  public async Task DeletePlaylist(int id)
  {
    await playlistRepository.DeletePlaylist(id);
  }

  public async Task<PlaylistDto> UpdatePlaylist(int userId, int id, UpdatePlaylistDto playlistDto)
  {
    var playlist = await playlistRepository.GetPlaylistById(userId, id);

    if (playlist == null)
    {
      throw new NullReferenceException($"Playlist with id {id} not found");
    }

    playlist.Name = playlistDto.Name;
    playlist = await playlistRepository.UpdatePlaylist(userId, id, playlist);

    return new PlaylistDto(playlist);
  }
}