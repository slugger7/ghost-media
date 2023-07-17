using Ghost.Data;

namespace Ghost.Services;

public interface IJobService
{
    Task StartJob(int id);
    Task DeleteJob(int id);
    Task<IEnumerable<Job>> GetJobs();
}