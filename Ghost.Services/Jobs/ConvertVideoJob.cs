using Ghost.Data;
using Ghost.Data.Enums;
using Ghost.Dtos;
using Ghost.Exceptions;
using Ghost.Media;
using Ghost.Repository;
using Microsoft.EntityFrameworkCore;

namespace Ghost.Services.Jobs
{
    public class ConvertVideoJob
    {
        private readonly DbContextOptions<GhostContext> contextOptions;
        private int Id;
        private ConvertRequestDto convertRequest;
        public string ThreadName { get; }
        private int JobId;

        public ConvertVideoJob(int id, string threadName, int jobId, ConvertRequestDto convertRequestDto, DbContextOptions<GhostContext> contextOptions)
        {
            this.Id = id;
            this.convertRequest = convertRequestDto;
            this.contextOptions = contextOptions;
            this.ThreadName = threadName;
            this.JobId = jobId;
        }

        public async void Run()
        {
            Console.WriteLine("Conversion job starting");
            string videoPath = string.Empty;
            string newPath = string.Empty;
            int libPathId;

            using (var context = new GhostContext(contextOptions))
            {
                var video = VideoRepository.FindById(context, Id, new List<string> { "LibraryPath" });
                if (video == null) throw new NullReferenceException("Could not find video before conversion job");
                videoPath = video.Path;
                libPathId = video.LibraryPath.Id;

                var convertJob = await context.ConvertJobs.Include("Job").FirstOrDefaultAsync(j => j.Job.Id == JobId);
                if (convertJob == null) throw new NullReferenceException("Conversion job was not found");
                convertJob.Job.Status = JobStatus.InProgress;
                newPath = convertJob.Path;

                await context.SaveChangesAsync();

                if (String.IsNullOrEmpty(videoPath)) throw new NullReferenceException("Video to convert had no path");
                if (String.IsNullOrEmpty(newPath)) throw new NullReferenceException("Path for converted video was null or empty");
                await VideoFns.ConvertVideo(videoPath, newPath);

                var newVideoInfo = VideoFns.GetVideoInformation(newPath);
                if (newVideoInfo == null) throw new NullReferenceException("Could not find video info");

                var libraryPath = await context.LibraryPaths.FirstOrDefaultAsync(l => l.Id == libPathId);
                if (libraryPath == null) throw new NullReferenceException("Library path for converted video was not found");

                var newVideoEntity = await VideoRepository.CreateVideo(context, newPath, newVideoInfo, libraryPath);

                // ImageService.GenerateThumbnailForVideo(new GenerateImageRequestDto
                // {
                //     VideoId = newVideoEntity.Id
                // });

                // copy actors
                // copy genres

                video = VideoRepository.FindById(context, Id, null);
                if (video != null)
                {
                    await VideoRepository.RelateVideo(context, Id, newVideoEntity.Id);
                    await VideoRepository.RelateVideo(context, newVideoEntity.Id, Id);
                }

                //consider using transactions to create all of these things together
                var job = await context.Jobs.FirstOrDefaultAsync(j => j.Id == JobId);
                if (job == null) throw new NullReferenceException("Colud not find job after conversion completed");

                job.Status = JobStatus.Completed;

                await context.SaveChangesAsync();
            }

            // Optional: create thread
            // Optional: create jobs entity
            Console.WriteLine("Finished converting video in thread");
        }
    }
}