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
        var job = await jobRepository.GetJobById(id);
        if (job == null) throw new NullReferenceException("Job was not found to start");

        var jobFactory = new JobFactory(scopeFactory);
        var runableJob = await jobFactory.CreateJob(job);
        if (runableJob == null) throw new NullReferenceException("Job to start was not found");

        var thread = new Thread(new ThreadStart(runableJob.Run));
        thread.Name = job.ThreadName;
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