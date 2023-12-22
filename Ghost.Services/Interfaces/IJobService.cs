using Ghost.Data;

namespace Ghost.Services;

public interface IJobService
{
  Task StartJob(int id);
  Task DeleteJob(int id);
  Task DeleteJobsByStatus(string status);
  Task<IEnumerable<Job>> GetJobs();
}