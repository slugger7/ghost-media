using Ghost.Data;
using Ghost.Data.Enums;
using Ghost.Dtos;
using Ghost.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ghost.Repository;

public class JobRepository : IJobRepository
{
  private readonly GhostContext context;
  private readonly IVideoRepository videoRepository;
  private readonly ILibraryRepository libraryRepository;
  private readonly ILogger<IJobRepository> logger;
  public JobRepository(
      GhostContext context,
      IVideoRepository videoRepository,
      ILibraryRepository libraryRepository,
      ILogger<IJobRepository> logger)
  {
    this.context = context;
    this.videoRepository = videoRepository;
    this.libraryRepository = libraryRepository;
    this.logger = logger;
  }

  public async Task<int> CreateConvertJob(int id, string threadName, ConvertRequestDto convertRequest)
  {
    var video = videoRepository.FindById(id, null);
    if (video == null) throw new NullReferenceException("Video was not found to convert");

    var root = Path.GetDirectoryName(video.Path) ?? "";
    var newPath = Path.Combine(root, convertRequest.Title + ".mp4");

    if (File.Exists(newPath)) throw new FileExistsException("Path to save converted video already exists");

    if (video.Height < convertRequest.Height || video.Width < convertRequest.Width)
    {
      logger.LogWarning($"Video {video.Id} had a height or width greater than the original height or width and has been reset");
      convertRequest.Height = video.Height;
      convertRequest.Width = video.Width;
    }

    var convertJob = new ConvertJob
    {
      Video = video,
      Title = convertRequest.Title,
      Path = newPath,
      ConstantRateFactor = convertRequest.ConstantRateFactor,
      VariableBitrate = convertRequest.VariableBitrate,
      ForcePixelFormat = convertRequest.ForcePixelFormat,
      Width = convertRequest.Width,
      Height = convertRequest.Height,
      Job = new Job
      {
        ThreadName = threadName,
        Type = JobType.Conversion
      }
    };

    context.ConvertJobs.Add(convertJob);

    await context.SaveChangesAsync();

    return convertJob.Job.Id;
  }

  public async Task<int> CreateSyncJob(int libraryId, string threadName)
  {
    var library = await libraryRepository.FindById(libraryId);
    if (library == null) throw new NullReferenceException("Could not get the library to create a sync job");

    var syncJob = new SyncJob
    {
      Library = library,
      Job = new Job
      {
        ThreadName = threadName,
        Type = JobType.Synchronise
      }
    };

    context.SyncJobs.Add(syncJob);

    await context.SaveChangesAsync();

    return syncJob.Job.Id;
  }

  public async Task<int> CreateGenerateThumbnailsJob(int libraryId, bool overwrite, string threadName)
  {
    var libray = await libraryRepository.FindById(libraryId);
    if (libray == null) throw new NullReferenceException("Could not get the library to create generate thumbnails job");

    var generateThumbnailsJob = new GenerateThumbnailsJob
    {
      Overwrite = overwrite,
      Library = libray,
      Job = new Job
      {
        ThreadName = threadName,
        Type = JobType.GenerateThumbnails
      }
    };

    context.GenerateThumbnailsJobs.Add(generateThumbnailsJob);

    await context.SaveChangesAsync();

    return generateThumbnailsJob.Job.Id;
  }

  public async Task<int> CreateGenerateChaptersJob(int libraryId, bool overwrite, string threadName)
  {
    var library = await libraryRepository.FindById(libraryId);
    if (library == null) throw new NullReferenceException("Could not get the library to create generate chapters job");

    var generateChaptersJob = new GenerateChaptersJob
    {
      Overwrite = overwrite,
      Library = library,
      Job = new Job
      {
        ThreadName = threadName,
        Type = JobType.GenerateChapters
      }
    };

    context.GenerateChaptersJobs.Add(generateChaptersJob);

    await context.SaveChangesAsync();

    return generateChaptersJob.Job.Id;
  }

  public async Task<ConvertJob?> GetConvertJobByJobId(int jobId)
  {
    var convertJob = await context.ConvertJobs
        .Include("Job")
        .Include("Video")
        .FirstOrDefaultAsync(j => j.Job.Id == jobId);

    return convertJob;
  }

  public async Task<SyncJob?> GetSyncJobByJobId(int jobId)
  {
    var syncJob = await context.SyncJobs
        .Include("Job")
        .Include("Library.Paths.Videos.VideoImages")
        .FirstOrDefaultAsync(j => j.Job.Id == jobId);

    return syncJob;
  }
  public async Task<GenerateThumbnailsJob?> GetGenerateThumbnailsJobByJobId(int jobId)
  {
    var generateThumbnailsJob = await context.GenerateThumbnailsJobs
        .Include("Job")
        .Include("Library")
        .FirstOrDefaultAsync(g => g.Job.Id == jobId);

    return generateThumbnailsJob;
  }

  public async Task<GenerateChaptersJob?> GetGenerateChaptersJobByJobId(int jobId)
  {
    var generateChaptersJob = await context.GenerateChaptersJobs
        .Include("Job")
        .Include("Library")
        .FirstOrDefaultAsync(g => g.Job.Id == jobId);

    return generateChaptersJob;
  }

  public async Task<Job?> GetJobById(int id)
  {
    var job = await context.Jobs.FirstOrDefaultAsync(j => j.Id == id);

    return job;
  }

  public async Task<IEnumerable<Job>> GetJobs()
  {
    return await context.Jobs.OrderByDescending(j => j.Created).ToListAsync();
  }

  public async Task<Job> UpdateJobStatus(int id, string status)
  {
    var job = await context.Jobs.FirstOrDefaultAsync(j => j.Id == id);
    if (job == null) throw new NullReferenceException("Job was null when updating status");

    job.Status = status;
    job.Modified = DateTime.UtcNow;

    await context.SaveChangesAsync();

    return job;
  }

  public async Task<IEnumerable<Job>> GetJobsByStatus(string status)
  {
    return await context.Jobs
        .Where(j => j.Status.Equals(status))
        .OrderBy(j => j.Created)
        .ToListAsync();
  }

  public async Task DeleteJob(int id)
  {
    var job = await context.Jobs.FindAsync(id);
    if (job == null) throw new NullReferenceException("Job was not found to delet");

    context.Jobs.Remove(job);

    await context.SaveChangesAsync();
  }

  public async Task DeleteJobsByStatus(string status)
  {
    var completedJobs = context.Jobs.Where(j => j.Status.ToLower().Equals(status.ToLower()));
    context.Jobs.RemoveRange(completedJobs);

    await context.SaveChangesAsync();
  }
}