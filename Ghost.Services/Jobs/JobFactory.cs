using Ghost.Data;
using Ghost.Data.Enums;
using Ghost.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Ghost.Services.Jobs;

public class JobFactory
{
    private readonly IServiceScopeFactory scopeFactory;
    public JobFactory(IServiceScopeFactory scopeFactory)
    {
        this.scopeFactory = scopeFactory;
    }

    public async Task<BaseJob?> CreateJob(Job job)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();

            if (job.Type.Equals(JobType.Conversion))
            {
                var conversionJob = await jobRepository.GetConvertJobByJobId(job.Id);
                if (conversionJob == null) throw new NullReferenceException("Could not find conversion job in factory");

                return new ConvertVideoJob(scopeFactory, job.Id, conversionJob.Video.Id);
            }
        }

        return null;
    }
}