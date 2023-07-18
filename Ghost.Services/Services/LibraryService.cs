using Ghost.Data;
using Ghost.Dtos;
using Ghost.Media;
using Ghost.Services.Jobs;
using Ghost.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ghost.Services;

public class LibraryService : ILibraryService
{
    private readonly IDirectoryService directoryService;
    private readonly IVideoService videoService;
    private readonly ILibraryRepository libraryRepository;
    private readonly ILogger<LibraryService> logger;
    private readonly IImageIoService imageIoService;
    private readonly IVideoRepository videoRepository;
    private readonly IJobRepository jobRepository;
    private readonly IServiceScopeFactory scopeFactory;

    public LibraryService(
      IDirectoryService directoryService,
      IVideoService videoService,
      ILibraryRepository libraryRepository,
      IVideoRepository videoRepository,
      ILogger<LibraryService> logger,
      IImageIoService imageIoService,
      IJobRepository jobRepository,
      IServiceScopeFactory scopeFactory
    )
    {
        this.directoryService = directoryService;
        this.videoService = videoService;
        this.libraryRepository = libraryRepository;
        this.logger = logger;
        this.imageIoService = imageIoService;
        this.videoRepository = videoRepository;
        this.jobRepository = jobRepository;
        this.scopeFactory = scopeFactory;
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

    public async Task Sync(int libraryId)
    {
        var threadName = $"Sync library {libraryId}";
        var jobId = await jobRepository.CreateSyncJob(libraryId, threadName);

        var syncJob = new SyncLibraryJob(scopeFactory, jobId);

        Thread syncThread = new Thread(new ThreadStart(syncJob.Run));
        syncThread.Name = threadName;
        syncThread.Start();
    }

    public async Task<LibraryDto> GetLibrary(int id)
    {
        var library = await libraryRepository.FindById(id);
        if (library == null) throw new NullReferenceException("Library not found");

        return new LibraryDto(library);
    }

    public async Task Delete(int id)
    {
        await libraryRepository.Delete(id);
    }

    public async Task SyncNfos(int id)
    {
        var library = await libraryRepository.FindById(id);
        if (library == null) throw new NullReferenceException("Library not found");
        var videos = new List<Video>();
        foreach (var path in library.Paths)
        {
            videos.AddRange(path.Videos);
        }

        var videoCount = videos.Count();

        await videoService.BatchSyncNfos(videos);
    }

    public async Task GenerateThumbnails(int id, bool overwrite)
    {
        var threadName = $"GenerateThumbnails {id}";
        var jobId = await jobRepository.CreateGenerateThumbnailsJob(id, overwrite, threadName);

        var generateThumbnailsJob = new Jobs.GenerateThumbnailsJob(scopeFactory, jobId);

        var generateThumbnailsThread = new Thread(new ThreadStart(generateThumbnailsJob.Run));
        generateThumbnailsThread.Name = threadName;
        generateThumbnailsThread.Start();
    }

    public async Task GenerateChapters(int id, bool overwrite)
    {
        var threadName = $"GenerateChapters {id}";
        var jobId = await jobRepository.CreateGenerateChaptersJob(id, overwrite, threadName);

        var generateChaptersJob = new Jobs.GenerateChaptersJob(scopeFactory, jobId);

        var generateChaptersThread = new Thread(new ThreadStart(generateChaptersJob.Run));
        generateChaptersThread.Name = threadName;
        generateChaptersThread.Start();
    }
}