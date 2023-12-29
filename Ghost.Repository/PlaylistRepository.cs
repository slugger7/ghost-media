using Ghost.Data;
using Microsoft.EntityFrameworkCore;
using Ghost.Repository.Extensions;

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
      .Include("User")
      .Include("PlaylistVideos.Video")
      .Where(p => p.User.Id == userId)
      .ToListAsync();
  }

  public async Task<Playlist?> GetPlaylistById(int userId, int id)
  {
    return await context.Playlists
      .Include("User")
      .Include("PlaylistVideos.Video")
      .FirstOrDefaultAsync(p => p.Id == id);
  }

  public PageResult<Video> GetVideos(int playlistId, int userId, string watchState, string[]? genres, int page = 0, int limit = 10, string search = "", string sortBy = "title", bool ascending = true)
  {
    var includes = new List<string> {
      "Video.VideoImages.Image",
      "Video.FavouritedBy.User",
      "Video.VideoActors.Actor",
      "Video.VideoActors.Actor.FavouritedBy.User",
      "Video.WatchedBy.User"
    };

    if (genres != null)
    {
      includes.Add("Video.VideoGenres.Genre");
    }

    var playlistVideosQueryable = context.PlaylistVideos
      .AddIncludes(includes);

    var playlistVideos = playlistVideosQueryable
      .Where(pv => pv.Playlist.User.Id == userId
      && pv.Playlist.Id == playlistId);

    var videos = playlistVideos
      .Select(pv => pv.Video)
      .Search(search)
      .FilterWatchedState(watchState, userId)
      .FilterGenres(genres)
      .SortAndOrderVideos(sortBy, ascending);

    return new PageResult<Video>
    {
      Total = videos.Count(),
      Page = page,
      Content = videos
        .Skip(page * limit)
        .Take(limit)
    };
  }

  public async Task<Playlist> CreatePlaylist(Playlist playlist)
  {
    context.Playlists.Add(playlist);
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

  public async Task<Playlist> UpdatePlaylist(int userId, int id, Playlist playlist)
  {
    context.Playlists.Update(playlist);
    await context.SaveChangesAsync();
    return playlist;
  }
}