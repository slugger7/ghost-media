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

    public LibraryService(
      IDirectoryService directoryService,
      IVideoService videoService,
      ILibraryRepository libraryRepository,
      ILogger<LibraryService> logger
    )
    {
      this.directoryService = directoryService;
      this.videoService = videoService;
      this.libraryRepository = libraryRepository;
      this.logger = logger;
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
      var library = libraryRepository.FindById(id);
      if (library == null) throw new NullReferenceException("Library not found");

      foreach (var path in library.Paths)
      {
        var currentVideos = path.Videos;
        if (path.Path == null) continue;

        var directories = directoryService.GetDirectories(path.Path);
        var videos = directoryService.GetFilesOfTypeInDirectory(path.Path, "mp4");

        var dirIndex = 0;

        while (directories.Count() > dirIndex)
        {
          var currentDirectory = directories.ElementAt(dirIndex++);
          videos = videos.Concat(directoryService.GetFilesOfTypeInDirectory(currentDirectory, "mp4")).ToList();
        }

        var videoEntities = videos
          .Where(v => !currentVideos.Any(cv => cv.Path.Equals(v)))
          .Select(v =>
          {
            var videoSplit = v.Split(Path.DirectorySeparatorChar);
            var fileName = videoSplit[videoSplit.Length - 1];
            var initialTitle = fileName.Substring(0, fileName.LastIndexOf('.'));
            var video = new Video
            {
              Path = v,
              FileName = fileName,
              Title = initialTitle.Trim()
            };
            return video;
          })
          .ToList();

        logger.LogInformation("Synced {0} new videos", videoEntities.Count());
        libraryRepository.AddVideosToPath(path.Id, videoEntities);
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

      for (int i = 0; i < videoCount; i++)
      {
        var video = videos.ElementAt(i);
        await videoService.SyncWithNFO(video.Id);
        logger.LogInformation("Video: {0} of {1} - {2}", i, videoCount, video.Title);
      }
    }
  }
}