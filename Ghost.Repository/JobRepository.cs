using Ghost.Data;
using Ghost.Data.Enums;
using Ghost.Dtos;
using Ghost.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Ghost.Repository
{
    public class JobRepository : IJobRepository
    {
        private readonly GhostContext context;
        private readonly IVideoRepository videoRepository;
        public JobRepository(GhostContext context, IVideoRepository videoRepository)
        {
            this.context = context;
            this.videoRepository = videoRepository;
        }

        public async Task<int> CreateConvertJob(int id, string threadName, ConvertRequestDto convertRequest)
        {
            var video = videoRepository.FindById(id, null);
            if (video == null) throw new NullReferenceException("Video was not found to convert");

            var root = Path.GetDirectoryName(video.Path) ?? "";
            var newPath = Path.Combine(root, convertRequest.Title + ".mp4");

            if (File.Exists(newPath)) throw new FileExistsException("Path to save converted video already exists");

            var convertJob = new ConvertJob
            {
                Video = video,
                Title = convertRequest.Title,
                Path = newPath,
                Job = new Job
                {
                    ThreadName = threadName,
                    Type = JobType.Conversion
                }
            };

            context.ConvertJobs.Add(convertJob);

            await context.SaveChangesAsync();

            return convertJob.Job.Id;
        }

        public async Task<ConvertJob?> GetConvertJobByJobId(int jobId)
        {
            var convertJob = await context.ConvertJobs
                .Include("Job")
                .Include("Video")
                .FirstOrDefaultAsync(j => j.Job.Id == jobId);

            return convertJob;
        }

        public async Task<Job?> GetJobById(int id)
        {
            var job = await context.Jobs.FirstOrDefaultAsync(j => j.Id == id);

            return job;
        }

        public async Task<IEnumerable<Job>> GetJobs()
        {
            return await context.Jobs.OrderByDescending(j => j.Created).ToListAsync();
        }

        public async Task<Job> UpdateJobStatus(int id, string status)
        {
            var job = await context.Jobs.FirstOrDefaultAsync(j => j.Id == id);
            if (job == null) throw new NullReferenceException("Job was null when updating status");

            job.Status = status;
            job.Modified = DateTime.UtcNow;

            await context.SaveChangesAsync();

            return job;
        }

        public async Task<IEnumerable<Job>> GetJobsByStatus(string status)
        {
            return await context.Jobs
                .Where(j => j.Status.Equals(status))
                .OrderBy(j => j.Created)
                .ToListAsync();
        }

        public async Task DeleteJob(int id)
        {
            var job = await context.Jobs.FindAsync(id);
            if (job == null) throw new NullReferenceException("Job was not found to delet");

            context.Jobs.Remove(job);

            await context.SaveChangesAsync();
        }
    }
}