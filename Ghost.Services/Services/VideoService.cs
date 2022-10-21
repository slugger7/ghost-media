using Ghost.Data;
using Ghost.Dtos;
using Ghost.Media;
using Ghost.Repository;
using Microsoft.Extensions.Logging;

namespace Ghost.Services
{
    public class VideoService : IVideoService
    {
        private readonly ILogger<VideoService> logger;
        private readonly IGenreService genreService;
        private readonly IActorService actorService;
        private readonly IGenreRepository genreRepository;
        private readonly IVideoRepository videoRepository;
        private readonly IActorRepository actorRepository;
        private readonly IUserRepository userRepository;
        private readonly IImageIoService imageIoService;
        private readonly IImageService imageService;
        private readonly INfoService nfoService;

        public VideoService(
          ILogger<VideoService> logger,
          IGenreService genreService,
          IActorService actorService,
          IGenreRepository genreRepository,
          IVideoRepository videoRepository,
          IActorRepository actorRepository,
          IUserRepository userRepository,
          IImageIoService imageIoService,
          IImageService imageService,
          INfoService nfoService)
        {
            this.logger = logger;
            this.genreService = genreService;
            this.actorService = actorService;
            this.genreRepository = genreRepository;
            this.videoRepository = videoRepository;
            this.actorRepository = actorRepository;
            this.imageIoService = imageIoService;
            this.imageService = imageService;
            this.nfoService = nfoService;
            this.userRepository = userRepository;
        }

        public PageResultDto<VideoDto> SearchVideos(PageRequestDto pageRequest, FilterQueryDto filterQuery, int userId)
        {
            var videosPage = videoRepository.SearchVideos(
              userId,
              filterQuery.WatchState,
              filterQuery.Genres,
              pageRequest.Page,
              pageRequest.Limit,
              pageRequest.Search,
              pageRequest.SortBy,
              pageRequest.Ascending
              );
            return new PageResultDto<VideoDto>
            {
                Total = videosPage.Total,
                Page = videosPage.Page,
                Content = videosPage.Content.Select(v => new VideoDto(v, userId)).ToList()
            };
        }

        public VideoDto GetVideoById(int id, int userId)
        {
            var video = videoRepository.FindById(id);
            if (video == null) throw new NullReferenceException("Video not found");

            return new VideoDto(video, userId);
        }

        public VideoDto GetVideoById(int id, int userId, List<string>? includes)
        {
            var video = videoRepository.FindById(id, includes);
            if (video == null) throw new NullReferenceException("Video not found");

            return new VideoDto(video, userId);
        }

        public string GenerateThumbnail(int id)
        {
            var video = videoRepository.FindById(id);

            if (video == null) throw new NullReferenceException("Video not found");
            if (video.Path == null) throw new NullReferenceException("Path was null");

            var outputPath = ImageIoService.GenerateFileName(video.Path, ".png");

            imageIoService.GenerateImage(video.Path, outputPath);

            return outputPath;
        }

        public async Task<VideoDto> UpdateMetaData(int id)
        {
            var video = videoRepository.FindById(id);

            if (video == null) throw new NullReferenceException("Video not found");
            if (video.Path == null) throw new NullReferenceException("Video has no path");

            var metaData = VideoFns.GetVideoInformation(video.Path);
            if (metaData is null) throw new NullReferenceException("Video meta data not found");
            video.Created = metaData.Created;
            video.Size = metaData.Size;
            video.Height = metaData.Height;
            video.Width = metaData.Width;
            video.Runtime = metaData.Duration.TotalMilliseconds;
            video.LastMetadataUpdate = DateTime.UtcNow;

            video = await videoRepository.UpdateVideo(video);
            if (video is null) throw new NullReferenceException("Video was not found after updating");

            return new VideoDto(video);
        }

        public VideoDto SetGenresByNameToVideo(int id, List<string> genres)
        {
            if (genres == null) throw new NullReferenceException("Genres not provided");
            var videoEntity = videoRepository.FindById(id, new List<string> { "VideoGenres" });

            if (videoEntity == null) throw new NullReferenceException("Video not found");
            var genreEntities = genres.Select(g => genreRepository.Upsert(g));

            videoEntity = videoRepository.SetGenres(videoEntity.Id, genreEntities);
            return new VideoDto(videoEntity);
        }

        public PageResultDto<VideoDto> GetVideosForGenre(string name, int userId, PageRequestDto pageRequest, FilterQueryDto filters)
        {
            var videosPage = videoRepository.GetForGenre(
              userId,
              filters.WatchState,
              name,
              pageRequest.Page,
              pageRequest.Limit,
              pageRequest.Search,
              pageRequest.SortBy,
              pageRequest.Ascending);
            return new PageResultDto<VideoDto>
            {
                Total = videosPage.Total,
                Page = videosPage.Page,
                Content = videosPage.Content.Select(v => new VideoDto(v, userId)).ToList()
            };
        }

        public VideoDto SetActorsByNameToVideo(int id, List<string> actors)
        {
            if (actors == null) throw new NullReferenceException("Actors not provided");
            var videoEntity = videoRepository.FindById(id, new List<string> { "VideoActors" });

            if (videoEntity == null) throw new NullReferenceException("Video not found");
            var actorEntities = actors.Select(a => actorRepository.UpsertActor(a));

            videoEntity = videoRepository.SetActors(videoEntity.Id, actorEntities);

            return new VideoDto(videoEntity);
        }

        public PageResultDto<VideoDto> GetVideosForActor(int actorId, int userId, PageRequestDto pageRequest, FilterQueryDto filters)
        {
            var videosPage = videoRepository.GetForActor(
              userId,
              filters.WatchState,
              filters.Genres,
              actorId,
              pageRequest.Page,
              pageRequest.Limit,
              pageRequest.Search,
              pageRequest.SortBy,
              pageRequest.Ascending);
            return new PageResultDto<VideoDto>
            {
                Total = videosPage.Total,
                Page = videosPage.Page,
                Content = videosPage.Content.Select(v => new VideoDto(v, userId)).ToList()
            };
        }

        public async Task<VideoDto> UpdateTitle(int id, string title)
        {
            var video = await videoRepository.UpdateTitle(id, title);

            return new VideoDto(video);
        }

        public async Task<VideoDto> SyncWithNFO(int id)
        {
            var video = videoRepository.FindById(id);
            if (video == null) throw new NullReferenceException("Video not found");

            var videoNfo = nfoService.HydrateVideo(video);
            if (videoNfo != null)
            {
                video.Title = videoNfo.title;
                video.LastNfoScan = DateTime.UtcNow;
                DateTime DateAdded;
                if (DateTime.TryParse(videoNfo.dateadded, out DateAdded))
                {
                    video.DateAdded = DateAdded;
                }
                video = await videoRepository.UpdateVideo(video);
                this.SetActorsByNameToVideo(id, videoNfo.actors.Select(a => a.name).ToList());
                this.SetGenresByNameToVideo(id, videoNfo.genres);
            }

            return new VideoDto(video);
        }

        public async Task DeletePermanently(int id)
        {
            var video = await videoRepository.Delete(id);
            File.Delete(video.Path);
        }

        public async Task BatchSyncNfos(List<Video> videos)
        {
            var videoGenreDictionary = new Dictionary<int, List<VideoGenre>>();
            var videoActorDictionary = new Dictionary<int, List<VideoActor>>();
            var newVideos = videos
              .Select(video =>
            {
                logger.LogDebug("Video: {0}", video.Title);
                var videoNfo = nfoService.HydrateVideo(video);
                if (videoNfo != null)
                {
                    logger.LogDebug("Video NFO: {0}", videoNfo.title);
                    video.Title = videoNfo.title.Trim();
                    video.LastNfoScan = DateTime.UtcNow;
                    DateTime DateAdded;
                    if (DateTime.TryParse(videoNfo.dateadded, out DateAdded))
                    {
                        video.DateAdded = DateAdded;
                    }
                    videoActorDictionary.Add(video.Id, videoNfo.actors.Select(actor => new VideoActor
                    {
                        Actor = new Actor { Name = actor.name.Trim() },
                        Video = video
                    })
              .ToList());
                    videoGenreDictionary.Add(video.Id, videoNfo.genres.Select(genre => new VideoGenre
                    {
                        Genre = new Genre { Name = genre.Trim() },
                        Video = video
                    })
              .ToList());
                }
                return video;
            });

            logger.LogDebug("Done batch sync");
            await videoRepository.BatchUpdateFromNFO(newVideos, videoGenreDictionary, videoActorDictionary);
        }

        private IEnumerable<Tuple<int, string>> CreateChaptersFromExistingImages(string directory)
        {
            logger.LogDebug("Creating chapters from existing images {0}", directory);
            var files = Directory.GetFiles(directory, "*.png");
            var chapterImages = files.Select(file =>
            {
                logger.LogDebug("Chapter image: {0}", file);
                var subStr = file.Substring(file.LastIndexOf('-') + 1, file.LastIndexOf('.') - file.LastIndexOf('-') - 1);
                logger.LogDebug("SubStr: {0}", subStr);
                var timestamp = Int32.Parse(subStr);
                return new Tuple<int, string>(timestamp, file);
            });

            return chapterImages;
        }

        public async Task<VideoDto> GenerateChapters(int id, bool overwrite = true)
        {
            logger.LogDebug("Generating chapters for {0}", id);
            var video = videoRepository.FindById(id, new List<string> { "Chapters.Image" });
            if (video == null) throw new NullReferenceException("Video not found");

            var assetsPath = Environment.GetEnvironmentVariable("ASSETS_PATH") ?? $"/home/slugger/dev/ghost-media/{Path.DirectorySeparatorChar}assets";
            var chapterLength = Int32.Parse(Environment.GetEnvironmentVariable("CHAPTER_LENGTH") ?? "300000");
            var directoryName = video.Id.ToString();
            var videoAssets = $"{assetsPath}{Path.DirectorySeparatorChar}{directoryName}";
            var directoryExists = Directory.Exists(videoAssets);

            IEnumerable<Tuple<int, string>> chapterImageTuples;
            if (overwrite || !directoryExists)
            {
                if (overwrite && directoryExists)
                {
                    logger.LogInformation("Deleting directory: {0}", videoAssets);
                    Directory.Delete(videoAssets);
                }
                Directory.CreateDirectory(videoAssets);
                var chapterMarks = new List<int>();
                var currentChapter = chapterLength;
                while (currentChapter <= video.Runtime)
                {
                    chapterMarks.Add(currentChapter);
                    currentChapter = currentChapter + chapterLength;
                }
                logger.LogDebug("Creating {0} chapter images", chapterMarks.Count());
                chapterImageTuples = imageIoService.CreateChapterImages(video.Path, video.Id.ToString(), videoAssets, chapterMarks);
            }
            else
            {
                chapterImageTuples = CreateChaptersFromExistingImages(videoAssets);
            }

            var chapters = chapterImageTuples.Select(chapterTuple => new Chapter
            {
                Description = "",
                Image = new Image
                {
                    Name = $"{video.Title}-{chapterTuple.Item1}",
                    Path = $"{chapterTuple.Item2}"
                },
                Timestamp = chapterTuple.Item1
            });

            video.Chapters = chapters.ToList();
            video = await videoRepository.UpdateVideo(video, new List<string> { "VideoImages.Image" });
            logger.LogInformation("Chapter images created for {0}", video.FileName);

            return new VideoDto(video);
        }

        public async Task LogProgress(int id, int userId, double progress)
        {
            await userRepository.LogProgress(id, userId, progress);
        }

        public VideoDto GetVideoInfo(int id, int userId)
        {
            var includes = new List<string> { "VideoImages.Image", "Chapters.Image", "FavouritedBy.User", "WatchedBy.User" };
            var video = videoRepository.FindById(id, includes);
            if (video == null) throw new NullReferenceException("Video not found");

            return new VideoDto(video, userId);
        }

        public PageResultDto<VideoDto> Favourites(int userId, PageRequestDto pageRequest, FilterQueryDto filters)
        {
            var videoPage = userRepository.Favourites(
              userId,
              filters.WatchState,
              filters.Genres,
              pageRequest.Page,
              pageRequest.Limit,
              pageRequest.Search,
              pageRequest.SortBy,
              pageRequest.Ascending);

            return new PageResultDto<VideoDto>
            {
                Total = videoPage.Total,
                Page = videoPage.Page,
                Content = videoPage.Content.Select(v => new VideoDto(v, userId)).ToList()
            };
        }
    }
}