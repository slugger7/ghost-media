using Ghost.Data;
using Ghost.Dtos;
using Ghost.Repository;
using Ghost.Services.Jobs;
using Microsoft.Extensions.DependencyInjection;

namespace Ghost.Services;

public class JobService : IJobService
{
    private readonly IJobRepository jobRepository;
    private readonly IServiceScopeFactory scopeFactory;

    public JobService(IServiceScopeFactory scopeFactory, IJobRepository jobRepository)
    {
        this.scopeFactory = scopeFactory;
        this.jobRepository = jobRepository;
    }

    public async Task StartJob(int id)
    {
        var job = await jobRepository.GetConvertJobByJobId(id);
        if (job == null) throw new NullReferenceException("Job was not found to start");

        var convertJob = new ConvertVideoJob(scopeFactory, id, job.Video.Id);

        var thread = new Thread(new ThreadStart(convertJob.Run));
        thread.Name = job.Job.ThreadName;
        thread.Start();
    }

    public async Task DeleteJob(int id)
    {
        await jobRepository.DeleteJob(id);
    }

    public async Task<IEnumerable<Job>> GetJobs()
    {
        var jobs = await jobRepository.GetJobs();

        return jobs;
    }
}