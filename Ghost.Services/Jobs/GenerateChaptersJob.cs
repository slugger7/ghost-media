using Ghost.Data.Enums;
using Ghost.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ghost.Services.Jobs;

public class GenerateChaptersJob : BaseJob
{
  public GenerateChaptersJob(IServiceScopeFactory scopeFactory, int jobId)
  : base(scopeFactory, jobId) { }
  public override async Task<string> RunJob()
  {
    using (var scope = scopeFactory.CreateScope())
    {
      var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
      var logger = loggerFactory.CreateLogger<GenerateChaptersJob>();
      var libraryRepository = scope.ServiceProvider.GetRequiredService<ILibraryRepository>();
      var videoService = scope.ServiceProvider.GetRequiredService<IVideoService>();
      var jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();

      var chaptersJob = await jobRepository.GetGenerateChaptersJobByJobId(jobId);
      if (chaptersJob == null) throw new NullReferenceException("Chapters job was not found when running job");

      var videos = await libraryRepository.GetVideos(chaptersJob.Library.Id);

      for (int i = 0; i < videos.Count(); i++)
      {
        var video = videos.ElementAt(i);
        await videoService.GenerateChapters(video.Id, chaptersJob.Overwrite);
        logger.LogInformation("Generating chapters {0} of {1}", i + 1, videos.Count());
      }

      return JobStatus.Completed;
    }
  }
}