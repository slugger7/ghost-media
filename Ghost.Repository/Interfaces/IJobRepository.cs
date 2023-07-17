using Ghost.Data;
using Ghost.Data.Enums;
using Ghost.Dtos;

namespace Ghost.Repository
{
    public interface IJobRepository
    {
        Task<int> CreateConvertJob(int id, string threadName, ConvertRequestDto convertRequest);
        Task<int> CreateSyncJob(int libraryId, string threadName);
        Task<ConvertJob?> GetConvertJobByJobId(int jobId);
        Task<SyncJob?> GetSyncJobByJobId(int jobId);
        Task<Job?> GetJobById(int id);
        Task<Job> UpdateJobStatus(int id, string status);
        Task<IEnumerable<Job>> GetJobs();
        Task<IEnumerable<Job>> GetJobsByStatus(string status);
        Task DeleteJob(int id);
    }
}