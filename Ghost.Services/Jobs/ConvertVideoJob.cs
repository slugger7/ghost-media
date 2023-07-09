using Ghost.Data;
using Ghost.Data.Enums;
using Ghost.Dtos;
using Ghost.Exceptions;
using Ghost.Media;
using Ghost.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ghost.Services.Jobs
{
    public class ConvertVideoJob
    {
        private readonly IServiceScopeFactory scopeFactory;
        private int Id;
        private int JobId;

        public ConvertVideoJob(
            int id,
            int jobId,
            IServiceScopeFactory scopeFactory)
        {
            this.Id = id;
            this.JobId = jobId;
            this.scopeFactory = scopeFactory;
        }

        public async void Run()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                var logger = new Logger<ConvertJob>(loggerFactory);
                var videoRepository = scope.ServiceProvider.GetRequiredService<IVideoRepository>();
                var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
                var jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();
                var libraryRepository = scope.ServiceProvider.GetRequiredService<ILibraryRepository>();

                logger.LogInformation("Starting conversion job: {0}", JobId);

                var video = videoRepository.FindById(Id, new List<string> { "LibraryPath" });
                if (video == null) throw new NullReferenceException("Could not find video before conversion job");

                var convertJob = await jobRepository.GetConvertJob(JobId);
                if (convertJob == null) throw new NullReferenceException("Conversion job was not found");

                var newPath = convertJob.Path;

                convertJob.Job = await jobRepository.UpdateJobStatus(convertJob.Id, JobStatus.InProgress);

                if (String.IsNullOrEmpty(video.Path)) throw new NullReferenceException("Video to convert had no path");
                if (String.IsNullOrEmpty(newPath)) throw new NullReferenceException("Path for converted video was null or empty");
                await VideoFns.ConvertVideo(video.Path, newPath);

                var newVideoInfo = VideoFns.GetVideoInformation(newPath);
                if (newVideoInfo == null) throw new NullReferenceException("Could not find video info");

                var libraryPath = await libraryRepository.GetLibraryPathById(video.LibraryPath.Id);
                if (libraryPath == null) throw new NullReferenceException("Library path for converted video was not found");

                var newVideoEntity = await videoRepository.CreateVideo(newPath, newVideoInfo, libraryPath);

                imageService.GenerateThumbnailForVideo(new GenerateImageRequestDto
                {
                    VideoId = newVideoEntity.Id
                });

                video = videoRepository.FindById(Id, null);
                if (video != null)
                {
                    await videoRepository.RelateVideo(Id, newVideoEntity.Id);
                    await videoRepository.RelateVideo(newVideoEntity.Id, Id);
                }

                // TODO: onsider using transactions to create all of these things together
                await jobRepository.UpdateJobStatus(convertJob.Job.Id, JobStatus.Completed);

                logger.LogInformation("Completed conversion job: {0}", JobId);
            }
        }
    }
}