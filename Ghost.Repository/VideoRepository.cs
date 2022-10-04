using Ghost.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ghost.Data.Enums;
using Ghost.Repository.Extensions;

namespace Ghost.Repository
{
    public class VideoRepository : IVideoRepository
    {
        private readonly GhostContext context;
        private readonly ILogger<VideoRepository> logger;
        private readonly IActorRepository actorRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IImageRepository imageRepository;
        public static Func<String, Func<Video, bool>> videoSearch = search => v => v.Title.ToUpper().Contains(search.Trim().ToUpper());

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

        public Video SetActors(int id, IEnumerable<Actor> actors)
        {
            var video = context.Videos.Find(id);
            if (video == null) throw new NullReferenceException("Video was null");

            context.VideoActors.RemoveRange(video.VideoActors);
            var videoActors = actors.Select(a => new VideoActor
            {
                Actor = a,
                Video = video
            });
            video.VideoActors = videoActors.ToList();
            context.SaveChanges();

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

        public PageResult<Video> GetForGenre(int userId, string watchState, string name, int page = 0, int limit = 10, string search = "", string sortBy = "title", bool ascending = true)
        {
            var genre = genreRepository.GetByName(name);

            if (genre == null) throw new NullReferenceException("Genre not found");
            var videos = genre.VideoGenres
                .Select(vg => vg.Video)
                .Where(videoSearch(search))
                .FilterWatchedState(watchState, userId)
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

        public PageResult<Video> GetForActor(int userId, string watchState, int actorId, int page = 0, int limit = 10, string search = "", string sortBy = "title", bool ascending = true)
        {
            var actor = actorRepository.FindById(actorId);

            if (actor == null) throw new NullReferenceException("Actor not found");
            var videos = actor.VideoActors
                .Select(va => va.Video)
                .Where(videoSearch(search))
                .FilterWatchedState(watchState, userId)
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

        public Video SetGenres(int id, IEnumerable<Genre> genres)
        {
            var video = context.Videos.Find(id);
            if (video == null) throw new NullReferenceException("Video was null");

            context.VideoGenres.RemoveRange(video.VideoGenres);

            var videoGenres = genres.Select(g => new VideoGenre
            {
                Genre = g,
                Video = video
            });

            video.VideoGenres = videoGenres.ToList();
            context.SaveChanges();

            video = FindById(id, new List<String> { "VideoGenres.Genre" });
            if (video == null) throw new NullReferenceException("Video was null");
            return video;
        }
        public PageResult<Video> SearchVideos(int userId, string watchState, int page = 0, int limit = 10, string search = "", string sortBy = "title", bool ascending = true)
        {
            var videos = context.Videos
              .Include("VideoImages.Image")
              .Include("FavouritedBy.User")
              .Include("VideoActors.Actor")
              .Include("VideoActors.Actor.FavouritedBy.User")
              .Include("WatchedBy.User")
              .Where(videoSearch(search))
              .FilterWatchedState(watchState, userId)
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
              .FirstOrDefault(v => v.Id == id);
            if (video == null) throw new NullReferenceException();

            context.VideoGenres.RemoveRange(video.VideoGenres);
            context.VideoActors.RemoveRange(video.VideoActors);
            context.Images.RemoveRange(video.VideoImages.Select(vi => vi.Image));
            context.VideoImages.RemoveRange(video.VideoImages);
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
    }
}