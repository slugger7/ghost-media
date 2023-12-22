using Ghost.Data.Enums;
using Ghost.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ghost.Services.Jobs;
public abstract class BaseJob
{
  protected int jobId;
  protected readonly IServiceScopeFactory scopeFactory;

  public BaseJob(IServiceScopeFactory scopeFactory, int jobId)
  {
    this.scopeFactory = scopeFactory;
    this.jobId = jobId;
  }

  public abstract Task<string> RunJob();

  public async void Run()
  {
    if (await this.PreRun())
    {
      string status;
      try
      {
        status = await this.RunJob();
      }
      catch (Exception)
      {
        status = JobStatus.Error;
      }

      await this.PostRun(status);
    }

  }

  protected async Task<bool> PreRun()
  {
    using (var scope = scopeFactory.CreateScope())
    {
      var jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();
      var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
      var logger = loggerFactory.CreateLogger<BaseJob>();

      var runningJobs = await jobRepository.GetJobsByStatus(JobStatus.InProgress);

      if (runningJobs.Count() == 0)
      {
        logger.LogInformation($"Starting job {jobId}");
        await jobRepository.UpdateJobStatus(jobId, JobStatus.InProgress);

        return true;
      }

      logger.LogInformation($"Job {jobId} will run after current job finishes");
      return false;
    }
  }

  protected async Task PostRun(string status)
  {
    using (var scope = scopeFactory.CreateScope())
    {
      var jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();
      var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
      var logger = loggerFactory.CreateLogger<BaseJob>();

      logger.LogInformation($"Completed job {jobId}");

      await jobRepository.UpdateJobStatus(jobId, status);

      var notStartedJobs = await jobRepository.GetJobsByStatus(JobStatus.NotStarted);
      var nextJob = notStartedJobs.FirstOrDefault();

      if (nextJob != null)
      {
        var jobFactory = new JobFactory(scopeFactory);
        var runnableJob = await jobFactory.CreateJob(nextJob);
        if (runnableJob == null) throw new NullReferenceException("Could not create a job to run next");

        Thread convertThread = new Thread(new ThreadStart(runnableJob.Run));
        convertThread.Name = nextJob.ThreadName;
        convertThread.Start();
      }
    }
  }
}