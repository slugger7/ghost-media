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
            catch (Exception ex)
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

            var runningJobs = await jobRepository.GetJobsByStatus(JobStatus.InProgress);

            if (runningJobs.Count() == 0)
            {
                await jobRepository.UpdateJobStatus(jobId, JobStatus.InProgress);

                return true;
            }

            return false;
        }
    }

    protected async Task PostRun(string status)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();

            await jobRepository.UpdateJobStatus(jobId, status);

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