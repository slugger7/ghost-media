using Ghost.Data;
using Microsoft.EntityFrameworkCore;

namespace Ghost.Repository;

public class PlaylistRepository : IPlaylistRepository
{
  private readonly GhostContext context;

  public PlaylistRepository(GhostContext dbContext)
  {
    context = dbContext;
  }

  public async Task<List<Playlist>> GetPlaylists(int userId)
  {
    return await context.Playlists
      .ToListAsync();
  }

  public async Task<Playlist?> GetPlaylistById(int userId, int id)
  {
    return await context.Playlists
      .Include("PlaylistVideos.Video")
      .Include("User")
      .Where(p => p.User.Id == userId && p.Id == id)
      .FirstOrDefaultAsync();
  }

  public async Task<Playlist> CreatePlaylist(Playlist playlist)
  {
    await context.Playlists.AddAsync(playlist);
    await context.SaveChangesAsync();
    return playlist;
  }

  public async Task DeletePlaylist(int id)
  {
    var playlist = context.Playlists
      .Include("PlaylistVideos")
      .FirstOrDefault(p => p.Id == id);
    ;

    if (playlist != null)
    {
      context.Playlists.Remove(playlist);
      context.PlaylistVideos.RemoveRange(playlist.PlaylistVideos);
      await context.SaveChangesAsync();
    }
  }
}