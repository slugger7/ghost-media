using Ghost.Data;
using Ghost.Data.Enums;
using Ghost.Dtos;

namespace Ghost.Repository
{
    public interface IJobRepository
    {
        Task<int> CreateConvertJob(int id, string threadName, ConvertRequestDto convertRequest);
        Task<ConvertJob?> GetConvertJob(int jobId);
        Task<Job> UpdateJobStatus(int id, string status);
    }
}