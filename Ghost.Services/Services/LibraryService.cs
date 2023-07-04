using Ghost.Data;
using Ghost.Dtos;
using Ghost.Media;
using Ghost.Repository;
using Microsoft.Extensions.Logging;

namespace Ghost.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IDirectoryService directoryService;
        private readonly IVideoService videoService;
        private readonly ILibraryRepository libraryRepository;
        private readonly ILogger<LibraryService> logger;
        private readonly IImageIoService imageIoService;
        private readonly IVideoRepository videoRepository;

        public LibraryService(
          IDirectoryService directoryService,
          IVideoService videoService,
          ILibraryRepository libraryRepository,
          IVideoRepository videoRepository,
          ILogger<LibraryService> logger,
          IImageIoService imageIoService
        )
        {
            this.directoryService = directoryService;
            this.videoService = videoService;
            this.libraryRepository = libraryRepository;
            this.logger = logger;
            this.imageIoService = imageIoService;
            this.videoRepository = videoRepository;
        }

        public LibraryDto AddDirectories(int id, AddPathsToLibraryDto pathsToLibraryDto)
        {
            if (pathsToLibraryDto.Paths == null) throw new NullReferenceException("Paths were null");
            var newPaths = pathsToLibraryDto.Paths.Select(p => new LibraryPath { Path = p });

            var library = libraryRepository.AddPaths(id, newPaths);

            return new LibraryDto(library);
        }

        public LibraryDto Create(string libraryName)
        {
            var library = libraryRepository.Create(new Library
            {
                Name = libraryName
            });

            return new LibraryDto(library);
        }

        public PageResultDto<LibraryDto> GetLibraries(int page = 0, int limit = 10)
        {
            var libraryPage = libraryRepository.GetLibraries(page, limit);

            return new PageResultDto<LibraryDto>
            {
                Total = libraryPage.Total,
                Page = libraryPage.Page,
                Content = libraryPage.Content.Select(l => new LibraryDto(l)).ToList()
            };
        }

        public Task Sync(int id)
        {
            var batchSize = 100;
            var library = libraryRepository.FindById(id);
            if (library == null) throw new NullReferenceException("Library not found");

            foreach (var path in library.Paths)
            {
                var currentVideos = path.Videos;
                if (path.Path == null) continue;

                var directories = directoryService.GetDirectories(path.Path);
                var videos = directoryService.GetFilesOfTypeInDirectory(path.Path, "mp4");
                videos = videos.Concat(directoryService.GetFilesOfTypeInDirectory(path.Path, "mkv")).ToList();

                var dirIndex = 0;

                while (directories.Count() > dirIndex)
                {
                    var currentDirectory = directories.ElementAt(dirIndex++);
                    videos = videos.Concat(directoryService.GetFilesOfTypeInDirectory(currentDirectory, "mp4")).ToList();
                    videos = videos.Concat(directoryService.GetFilesOfTypeInDirectory(currentDirectory, "mkv")).ToList();
                }

                var videoEntities = videos
                  .Where(v => !currentVideos.Any(cv => cv.Path.Equals(v)))
                  .Select(v =>
                  {
                      var videoSplit = v.Split(Path.DirectorySeparatorChar);
                      var fileName = videoSplit[videoSplit.Length - 1];
                      var initialTitle = fileName.Substring(0, fileName.LastIndexOf('.'));
                      logger.LogInformation("Adding video: {0}", initialTitle);
                      var video = new Video
                      {
                          Path = v,
                          FileName = fileName,
                          Title = initialTitle.Trim()
                      };

                      return video;
                  })
                  .ToList();

                var videoBatch = new List<Video>();
                for (var i = 0; i < videoEntities.Count(); i++)
                {
                    var video = videoEntities.ElementAt(i);
                    var metaData = VideoFns.GetVideoInformation(video.Path);
                    if (metaData != null)
                    {
                        video.Created = metaData.Created;
                        video.Size = metaData.Size;
                        video.Height = metaData.Height;
                        video.Width = metaData.Width;
                        video.Runtime = metaData.Duration.TotalMilliseconds;
                        video.LastMetadataUpdate = DateTime.UtcNow;
                    }
                    videoBatch.Add(video);

                    if (i % batchSize == 0)
                    {
                        logger.LogInformation("Writing batch {0} of {1}", i / batchSize, videoEntities.Count() / batchSize);
                        libraryRepository.AddVideosToPath(path.Id, videoBatch);
                        videoBatch = new List<Video>();
                    }
                }
                libraryRepository.AddVideosToPath(path.Id, videoBatch);
                logger.LogInformation("Synced {0} new videos", videoEntities.Count());
            }

            return Task.CompletedTask;
        }

        public LibraryDto GetLibrary(int id)
        {
            var library = libraryRepository.FindById(id);
            if (library == null) throw new NullReferenceException("Library not found");

            return new LibraryDto(library);
        }

        public async Task Delete(int id)
        {
            await libraryRepository.Delete(id);
        }

        public async Task SyncNfos(int id)
        {
            var library = libraryRepository.FindById(id);
            if (library == null) throw new NullReferenceException("Library not found");
            var videos = new List<Video>();
            foreach (var path in library.Paths)
            {
                videos.AddRange(path.Videos);
            }

            var videoCount = videos.Count();

            await videoService.BatchSyncNfos(videos);
        }

        private IEnumerable<Video> GetVideos(int id)
        {
            var library = libraryRepository.FindById(id);
            if (library == null) throw new NullReferenceException("Library not found");

            logger.LogDebug("Library has {0} paths", library.Paths.Count());

            var videos = new List<Video>();
            foreach (var path in library.Paths)
            {
                logger.LogDebug("Path has {0} videos", path.Videos.Count());
                videos = videos.Concat(path.Videos).ToList();
            }

            return videos;
        }

        public async Task GenerateThumbnails(int id, bool overwrite)
        {
            var batchSize = 10;

            var videos = GetVideos(id);

            var videoBatch = new List<Video>();
            for (int i = 0; i < videos.Count(); i++)
            {
                var video = videos.ElementAt(i);
                logger.LogInformation("Generating thumbnail for video: {0}", video.Title);
                if (video.VideoImages.Where(vi => vi.Type.ToLower().Equals("thumbnail") && !overwrite).Count() > 0) continue;
                // if overwrite it will still create multiple thumbnails per video that point to the same place
                var outputPath = ImageIoService.GenerateFileName(video.Path, ".png");
                logger.LogDebug("Creating thumbnail {0}", outputPath);
                imageIoService.GenerateImage(video.Path, outputPath);
                video.VideoImages.Add(new VideoImage
                {
                    Video = video,
                    Image = new Image
                    {
                        Name = video.Title,
                        Path = outputPath
                    },
                    Type = "thumbnail"
                });
                videoBatch.Add(video);

                if (i % batchSize == 0)
                {
                    logger.LogInformation("Writing batch {0} of {1}", i / batchSize, videos.Count() / batchSize);
                    await videoRepository.BatchUpdate(videoBatch);
                    videoBatch = new List<Video>();
                }
            }

            await videoRepository.BatchUpdate(videoBatch);
        }

        public async Task GenerateChapters(int id, bool overwrite)
        {
            var videos = GetVideos(id);

            for (int i = 0; i < videos.Count(); i++)
            {
                var video = videos.ElementAt(i);
                await this.videoService.GenerateChapters(video.Id, overwrite);
                logger.LogInformation("Generating chapters {0} of {1}", i + 1, videos.Count());
            }

            logger.LogInformation("Done generating chapter images");
        }
    }
}