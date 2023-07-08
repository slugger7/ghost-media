using Ghost.Data;
using Ghost.Data.Enums;
using Ghost.Dtos;
using Ghost.Exceptions;
using Ghost.Media;
using Ghost.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ghost.Services.Jobs
{
    public class ConvertVideoJob
    {
        private readonly IServiceScopeFactory scopeFactory;
        private int Id;
        private ConvertRequestDto convertRequest;
        public string ThreadName { get; }
        private int JobId;

        public ConvertVideoJob(
            int id,
            string threadName,
            int jobId,
            ConvertRequestDto convertRequestDto,
            IServiceScopeFactory scopeFactory)
        {
            this.Id = id;
            this.convertRequest = convertRequestDto;
            this.ThreadName = threadName;
            this.JobId = jobId;
            this.scopeFactory = scopeFactory;
        }

        public async void Run()
        {
            Console.WriteLine("Conversion job starting");
            using (var scope = scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<GhostContext>();
                string videoPath = string.Empty;
                string newPath = string.Empty;
                int libPathId;

                var videoRepository = scope.ServiceProvider.GetRequiredService<IVideoRepository>();
                var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();

                var video = videoRepository.FindById(Id, new List<string> { "LibraryPath" });
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

                var newVideoEntity = await videoRepository.CreateVideo(newPath, newVideoInfo, libraryPath);

                imageService.GenerateThumbnailForVideo(new GenerateImageRequestDto
                {
                    VideoId = newVideoEntity.Id
                });

                // copy actors
                // copy genres

                video = videoRepository.FindById(Id, null);
                if (video != null)
                {
                    await videoRepository.RelateVideo(Id, newVideoEntity.Id);
                    await videoRepository.RelateVideo(newVideoEntity.Id, Id);
                }

                //consider using transactions to create all of these things together
                var job = await context.Jobs.FirstOrDefaultAsync(j => j.Id == JobId);
                if (job == null) throw new NullReferenceException("Colud not find job after conversion completed");

                job.Status = JobStatus.Completed;
            }

            Console.WriteLine("Finished converting video in thread");
        }
    }
}