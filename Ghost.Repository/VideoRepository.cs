using Ghost.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ghost.Repository.Extensions;
using Ghost.Dtos;
using Ghost.Exceptions;

namespace Ghost.Repository;
public class VideoRepository : IVideoRepository
{
  private readonly GhostContext context;
  private readonly ILogger<VideoRepository> logger;
  private readonly IActorRepository actorRepository;
  private readonly IGenreRepository genreRepository;
  private readonly IImageRepository imageRepository;

  public VideoRepository(
    GhostContext context,
    ILogger<VideoRepository> logger,
    IActorRepository actorRepository,
    IGenreRepository genreRepository,
    IImageRepository imageRepository)
  {
    this.context = context;
    this.logger = logger;
    this.actorRepository = actorRepository;
    this.genreRepository = genreRepository;
    this.imageRepository = imageRepository;
  }

  public async Task<Video> CreateVideo(string path, VideoMetaDataDto videoMetaData, LibraryPath libraryPath, string? title = null)
  {
    var video = new Video
    {
      Path = path,
      FileName = Path.GetFileName(path),
      Title = title != null ? title : Path.GetFileNameWithoutExtension(path),
      Height = videoMetaData.Height,
      Width = videoMetaData.Width,
      Runtime = videoMetaData.Duration.TotalMilliseconds,
      Size = videoMetaData.Size,
      LastMetadataUpdate = DateTime.UtcNow,
      LibraryPath = libraryPath
    };

    context.Videos.Add(video);

    await context.SaveChangesAsync();

    return video;
  }

  public async Task<Video> SetActors(int id, IEnumerable<Actor> actors)
  {
    var video = await context.Videos.FindAsync(id);
    if (video == null) throw new NullReferenceException("Video was null");

    context.VideoActors.RemoveRange(video.VideoActors);
    var videoActors = actors.Select(a => new VideoActor
    {
      Actor = a,
      Video = video
    });
    video.VideoActors = videoActors.ToList();
    await context.SaveChangesAsync();

    video = FindById(id, new List<String> { "VideoActors.Actor" });
    if (video == null) throw new NullReferenceException("Video was null");

    return video;
  }

  public Video? FindById(int id)
  {
    return this.FindById(id, new List<string>
            {
                "VideoGenres.Genre.VideoGenres",
                "VideoActors.Actor.VideoActors",
                "VideoImages.Image",
                "FavouritedBy.User"
            });
  }

  public Video? FindById(int id, List<string>? includes)
  {
    var videos = context.Videos;
    if (includes != null && includes.Count() > 0)
    {
      var videoQueryable = videos.Include(includes.ElementAt(0));
      for (int i = 1; i < includes.Count(); i++)
      {
        videoQueryable = videoQueryable.Include(includes.ElementAt(i));
      }
      return videoQueryable.FirstOrDefault(v => v.Id == id);
    }

    return videos.FirstOrDefault(v => v.Id == id);
  }

  public PageResult<Video> GetForGenre(int userId, string watchState, string[]? genresFilter, string name, int page = 0, int limit = 10, string search = "", string sortBy = "title", bool ascending = true)
  {
    var genreIncludes = new List<string> {
      "VideoGenres.Video",
      "VideoGenres.Video.VideoImages.Image",
      "VideoGenres.Video.FavouritedBy.User",
      "VideoGenres.Video.VideoActors.Actor",
      "VideoGenres.Video.WatchedBy.User",
      "VideoGenres.Video.VideoActors.Actor.FavouritedBy.User"
    };

    if (genresFilter != null)
    {
      genreIncludes.Add("VideoGenres.Video.VideoGenres.Genre");
    }

    var genre = genreRepository.GetByName(name, genreIncludes);

    if (genre == null) throw new NullReferenceException("Genre not found");
    var videos = genre.VideoGenres
        .Select(vg => vg.Video)
        .Search(search)
        .FilterWatchedState(watchState, userId)
        .FilterGenres(genresFilter)
        .SortAndOrderVideos(sortBy, ascending);

    return new PageResult<Video>
    {
      Total = videos.Count(),
      Page = page,
      Content = videos
        .Skip(limit * page)
        .Take(limit)
    };
  }

  public PageResult<Video> GetForActor(int userId, string watchState, string[]? genresFilter, int actorId, int page = 0, int limit = 10, string search = "", string sortBy = "title", bool ascending = true)
  {
    var actorIncludes = new List<string>
    {
      "VideoActors.Video",
      "VideoActors.Video.VideoImages.Image",
      "VideoActors.Video.FavouritedBy.User",
      "VideoActors.Video.VideoActors.Actor.FavouritedBy.User",
      "VideoActors.Video.WatchedBy.User",
      "FavouritedBy.User"
  };

    if (genresFilter != null)
    {
      actorIncludes.Add("VideoActors.Video.VideoGenres.Genre");
    }

    var actor = actorRepository.FindById(actorId, actorIncludes);

    if (actor == null) throw new NullReferenceException("Actor not found");
    var videos = actor.VideoActors
        .Select(va => va.Video)
        .Search(search)
        .FilterWatchedState(watchState, userId)
        .FilterGenres(genresFilter)
        .SortAndOrderVideos(sortBy, ascending);

    return new PageResult<Video>
    {
      Total = videos.Count(),
      Page = page,
      Content = videos
        .Skip(limit * page)
        .Take(limit)
    };
  }

  public async Task<Video> SetGenres(int id, IEnumerable<Genre> genres)
  {
    var video = await context.Videos.FindAsync(id);
    if (video == null) throw new NullReferenceException("Video was null");

    context.VideoGenres.RemoveRange(video.VideoGenres);

    var videoGenres = genres.Select(g => new VideoGenre
    {
      Genre = g,
      Video = video
    });

    video.VideoGenres = videoGenres.ToList();
    await context.SaveChangesAsync();

    video = FindById(id, new List<String> { "VideoGenres.Genre" });
    if (video == null) throw new NullReferenceException("Video was null");
    return video;
  }
  public PageResult<Video> SearchVideos(int userId, string watchState, string[]? genres, int page = 0, int limit = 10, string search = "", string sortBy = "title", bool ascending = true)
  {
    var videosQueryable = context.Videos
      .AddIncludes(new List<string> {
        "VideoImages.Image",
        "FavouritedBy.User",
        "VideoActors.Actor",
        "VideoActors.Actor.FavouritedBy.User",
        "WatchedBy.User"
      });

    if (genres != null)
    {
      videosQueryable = videosQueryable.Include("VideoGenres.Genre");
    }

    var videos = videosQueryable
        .Search(search)
        .FilterWatchedState(watchState, userId)
        .FilterGenres(genres)
        .SortAndOrderVideos(sortBy, ascending);

    return new PageResult<Video>
    {
      Total = videos.Count(),
      Page = page,
      Content = videos
        .Skip(limit * page)
        .Take(limit)
    };
  }

  public async Task<Video> Delete(int id)
  {
    var video = context.Videos
      .Include("VideoActors")
      .Include("VideoGenres")
      .Include("VideoImages.Image")
      .Include("Chapters.Image")
      .FirstOrDefault(v => v.Id == id);
    if (video == null) throw new NullReferenceException();

    context.VideoGenres.RemoveRange(video.VideoGenres);
    context.VideoActors.RemoveRange(video.VideoActors);
    context.Images.RemoveRange(video.VideoImages.Select(vi => vi.Image));
    context.VideoImages.RemoveRange(video.VideoImages);
    context.Images.RemoveRange(video.Chapters.Select(c => c.Image));
    context.Chapters.RemoveRange(video.Chapters);
    context.Videos.Remove(video);

    await context.SaveChangesAsync();
    logger.LogDebug("Saving changes");
    return video;
  }

  public async Task<Video> UpdateTitle(int id, string title)
  {
    var video = this.FindById(id, new List<string> { "VideoImages.Image" });
    if (video == null) throw new NullReferenceException("Video not fonud");

    video.Title = title.Trim();

    await context.SaveChangesAsync();

    return video;
  }

  public async Task<Video> UpdateVideo(Video video)
  {
    return await this.UpdateVideo(video, null);
  }

  public async Task<Video> UpdateVideo(Video video, List<string>? includes)
  {
    var videoEntity = this.FindById(video.Id, includes);
    if (videoEntity is null) throw new NullReferenceException("Video not found to update");

    videoEntity.Title = video.Title.Trim();
    videoEntity.DateAdded = video.DateAdded;
    videoEntity.Created = video.Created;
    videoEntity.Size = video.Size;
    videoEntity.Runtime = video.Runtime;
    videoEntity.Height = video.Height;
    videoEntity.Width = video.Width;
    videoEntity.LastMetadataUpdate = video.LastMetadataUpdate;
    videoEntity.LastNfoScan = video.LastNfoScan;
    videoEntity.Chapters = video.Chapters;

    await context.SaveChangesAsync();

    return video;
  }

  private async Task BatchHydrateGenres(Video video, List<VideoGenre> videoGenres)
  {
    logger.LogDebug("Batch updating genres");
    var newVideoGenres = new List<VideoGenre>();
    foreach (var videoGenre in videoGenres)
    {
      logger.LogDebug("Video genre loop");
      var genre = context.Genres.FirstOrDefault(
        g => g.Name.Trim().ToLower().Equals(videoGenre.Genre.Name.ToLower().Trim())
      );
      logger.LogDebug("Serach for genre done");
      if (genre == null)
      {
        logger.LogDebug("Creating genre: {0}", videoGenre.Genre.Name);
        newVideoGenres.Add(new VideoGenre
        {
          Video = videoGenre.Video,
          Genre = videoGenre.Genre
        });
        logger.LogDebug("New genre added");
        continue;
      }
      else
      {
        logger.LogDebug("Genre existed: {0}", genre.Name);
        var videoGenreEntity = context.VideoGenres.FirstOrDefault(vg => vg.Genre.Id == genre.Id && vg.Video.Id == video.Id);
        if (videoGenreEntity != null)
        {
          logger.LogDebug("Video genre existed");
          videoGenre.Id = videoGenreEntity.Id;
        }
        videoGenre.Genre = genre;
        logger.LogDebug("Adding video genre to new video genres");
        newVideoGenres.Add(new VideoGenre
        {
          Genre = videoGenre.Genre,
          Video = video,
          Id = videoGenre.Id
        });
        logger.LogDebug("Added it");
        continue;
      }
    }
    logger.LogDebug("Setting video genres");
    video.VideoGenres = newVideoGenres;
    logger.LogDebug("Saving");
    await context.SaveChangesAsync();
    logger.LogDebug("Done with genres");
  }

  private async Task BatchHydrateActors(Video video, List<VideoActor> videoActors)
  {
    logger.LogDebug("Batch updating actors");
    var newVideoActors = new List<VideoActor>();
    foreach (var videoActor in videoActors)
    {
      logger.LogDebug("Video actor loop");
      var actor = context.Actors.FirstOrDefault(
        a => a.Name.Trim().ToLower().Equals(videoActor.Actor.Name.ToLower().Trim())
      );
      logger.LogDebug("Serach for actor done");
      if (actor == null)
      {
        logger.LogDebug("Creating actor: {0}", videoActor.Actor.Name);
        newVideoActors.Add(new VideoActor
        {
          Video = videoActor.Video,
          Actor = videoActor.Actor
        });
        logger.LogDebug("New actor added");
        continue;
      }
      else
      {
        logger.LogDebug("Actor existed: {0}", actor.Name);
        var videoActorEntity = context.VideoActors.FirstOrDefault(va => va.Actor.Id == actor.Id && va.Video.Id == video.Id);
        if (videoActorEntity != null)
        {
          logger.LogDebug("Video actor existed");
          videoActor.Id = videoActorEntity.Id;
        }
        videoActor.Actor = actor;
        logger.LogDebug("Adding video actor to new video actors");
        newVideoActors.Add(new VideoActor
        {
          Actor = videoActor.Actor,
          Video = video,
          Id = videoActor.Id
        });
        logger.LogDebug("Added it");
        continue;
      }
    }
    logger.LogDebug("Setting video actors");
    video.VideoActors = newVideoActors;
    logger.LogDebug("Saving");
    await context.SaveChangesAsync();
    logger.LogDebug("Done with actors");
  }

  public async Task BatchUpdateFromNFO(IEnumerable<Video> videos, Dictionary<int, List<VideoGenre>> videoGenreDictionary, Dictionary<int, List<VideoActor>> videoActorDictionary)
  {
    logger.LogDebug("Batch sync updating");
    foreach (var video in videos)
    {
      logger.LogDebug("Finding Video: {0}", video.Id);
      logger.LogDebug("Updating information for video");
      var videoActors = videoActorDictionary.GetValueOrDefault(video.Id);
      if (videoActors != null)
      {
        await BatchHydrateActors(video, videoActors);
      }
      var videoGenres = videoGenreDictionary.GetValueOrDefault(video.Id);
      if (videoGenres != null)
      {
        await BatchHydrateGenres(video, videoGenres);
      }
    }
    logger.LogDebug("Updating the range");

    await context.SaveChangesAsync();
    logger.LogDebug("Batch sync updated done");
  }

  public async Task BatchUpdate(IEnumerable<Video> videos)
  {
    logger.LogDebug("Batch updating {0} videos", videos.Count());
    context.UpdateRange(videos);
    await context.SaveChangesAsync();
  }

  public Video Random(int userId, RandomVideoRequestDto randomVideoRequest)
  {
    var videos = context.Videos
         .Include("WatchedBy.User")
         .Include("VideoGenres.Genre");

    return videos.RandomVideo(userId, randomVideoRequest);
  }

  public Video GetRandomVideoForGenre(string name, int userId, RandomVideoRequestDto randomVideoRequest)
  {
    var genreIncludes = new List<string> {
                "VideoGenres.Video",
                "VideoGenres.Video.WatchedBy.User",
                "VideoGenres.Video.VideoGenres.Genre"
            };

    var genre = genreRepository.GetByName(name, genreIncludes);

    if (genre == null) throw new NullReferenceException("Genre not found");

    var video = genre.VideoGenres
        .Select(vg => vg.Video)
        .RandomVideo(userId, randomVideoRequest);

    return video;
  }

  public Video GetRandomVideoForActor(int id, int userId, RandomVideoRequestDto randomVideoRequest)
  {
    var actorIncludes = new List<string> {
                "VideoActors.Video",
                "VideoActors.Video.WatchedBy.User",
                "VideoActors.Video.VideoGenres.Genre"
            };

    var actor = actorRepository.FindById(id, actorIncludes);

    if (actor == null) throw new NullReferenceException("Genre not found");

    var video = actor.VideoActors
        .Select(vg => vg.Video)
        .RandomVideo(userId, randomVideoRequest);

    return video;
  }


  public async Task<List<Video>> RelateVideo(int id, int relateTo)
  {
    if (id == relateTo)
    {
      throw new VideoRelationException(id, relateTo);
    }
    var video = this.FindById(id, new List<string> {
                "RelatedVideos.RelatedTo"
            });

    if (video == null) throw new NullReferenceException("Video to relate to was not found");

    var relatedVideo = this.FindById(relateTo, new List<string> {
                "VideoImages.Image"
            });

    if (relatedVideo == null) throw new NullReferenceException("Related video was not found");

    if (video.RelatedVideos.Find(v => v.RelatedTo.Id == relatedVideo.Id) == null)
    {
      video.RelatedVideos.Add(new RelatedVideo { RelatedTo = relatedVideo });

      await context.SaveChangesAsync();
    }

    return video.RelatedVideos.Select(v => v.RelatedTo).ToList();
  }

  public async Task<List<Video>> DeleteRelation(int id, int relatedTo)
  {
    var video = this.FindById(id, new List<string> { "RelatedVideos.RelatedTo" });

    if (video == null) throw new NullReferenceException("Video with relation was not found");

    var relation = video.RelatedVideos.Find(v => v.RelatedTo.Id == relatedTo);

    if (relation == null) throw new NullReferenceException("Relation for video was not found");

    video.RelatedVideos.Remove(relation);

    await context.SaveChangesAsync();

    return video.RelatedVideos.Select(v => v.RelatedTo).ToList();
  }

  public async Task<Video> CreateSubVideo(int id, Video newVideo)
  {
    var video = this.FindById(id, new List<string> {
                "LibraryPath",
                "RelatedVideos.RelatedTo.RelatedVideos"
            });
    if (video == null) throw new NullReferenceException("Video was not found to create sub video on");

    newVideo.LibraryPath = video.LibraryPath;
    newVideo.RelatedVideos.Add(new RelatedVideo
    {
      RelatedTo = video
    });
    foreach (var relatedVideo in video.RelatedVideos)
    {
      newVideo.RelatedVideos.Add(new RelatedVideo
      {
        RelatedTo = relatedVideo.RelatedTo
      });
      relatedVideo.RelatedTo.RelatedVideos.Add(new RelatedVideo
      {
        RelatedTo = newVideo
      });
    }

    context.Videos.Add(newVideo);

    video.RelatedVideos.Add(new RelatedVideo
    {
      RelatedTo = newVideo
    });

    await context.SaveChangesAsync();

    return video;
  }

  public async Task<List<Video>> GetVideosByIds(IEnumerable<int> ids)
  {
    return await context.Videos
      .Where(v => ids.Contains(v.Id))
      .ToListAsync();
  }
}