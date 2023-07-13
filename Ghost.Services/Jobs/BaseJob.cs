using Ghost.Data.Enums;
using Ghost.Repository;
using Microsoft.Extensions.DependencyInjection;

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

    public abstract Task RunJob();

    public async void Run()
    {
        if (await this.PreRun())
        {
            await this.RunJob();

            await this.PostRun();
        }

    }

    protected async Task<bool> PreRun()
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();

            var runningJobs = await jobRepository.GetJobsByStatus(JobStatus.InProgress);

            return runningJobs.Count() == 0;
        }
    }

    protected async Task PostRun()
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();

            var notStartedJobs = await jobRepository.GetJobsByStatus(JobStatus.NotStarted);
            var nextJob = notStartedJobs.FirstOrDefault();

            if (nextJob != null)
            {
                // strategy pattern to construct the new job
                var convertJobEntity = await jobRepository.GetConvertJobByJobId(nextJob.Id);
                if (convertJobEntity == null) throw new NullReferenceException("Convert job is null");
                ConvertVideoJob convertJob = new ConvertVideoJob(scopeFactory, nextJob.Id, convertJobEntity.Video.Id);

                Thread convertThread = new Thread(new ThreadStart(convertJob.Run));
                convertThread.Name = nextJob.ThreadName;
                convertThread.Start();
            }
        }
    }
}