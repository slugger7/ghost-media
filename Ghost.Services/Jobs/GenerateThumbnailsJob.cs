using Ghost.Data;
using Ghost.Data.Enums;
using Ghost.Media;
using Ghost.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ghost.Services.Jobs;

public class GenerateThumbnailsJob : BaseJob
{
    public GenerateThumbnailsJob(IServiceScopeFactory scopeFactory, int jobId)
    : base(scopeFactory, jobId)
    { }

    public override async Task<string> RunJob()
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<GenerateThumbnailsJob>();
            var jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();
            var libraryRepository = scope.ServiceProvider.GetRequiredService<ILibraryRepository>();
            var imageIoService = scope.ServiceProvider.GetRequiredService<IImageIoService>();
            var videoRepository = scope.ServiceProvider.GetRequiredService<IVideoRepository>();

            var generateThumbnailsJob = await jobRepository.GetGenerateThumbnailsJobByJobId(jobId);
            if (generateThumbnailsJob == null) throw new NullReferenceException("Could not find generate thumbnails job in job");

            var batchSize = 10;

            var videos = await libraryRepository.GetVideos(generateThumbnailsJob.Library.Id);

            var videoBatch = new List<Video>();
            for (int i = 0; i < videos.Count(); i++)
            {
                var video = videos.ElementAt(i);
                logger.LogInformation("Generating thumbnail for video: {0}", video.Title);
                if (video.VideoImages.Where(vi => vi.Type.ToLower().Equals("thumbnail") && !generateThumbnailsJob.Overwrite).Count() > 0) continue;
                // if overwrite it will still create multiple thumbnails per video that point to the same place
                var outputPath = ImageIoService.GenerateFileName(video.Path, ".png");
                logger.LogDebug("Creating thumbnail {0}", outputPath);
                imageIoService.GenerateImage(video.Path, outputPath);
                video.VideoImages.Add(new VideoImage
                {
                    Video = video,
                    Image = new Image
                    {
                        Name = video.Title,
                        Path = outputPath
                    },
                    Type = "thumbnail"
                });
                videoBatch.Add(video);

                if (i % batchSize == 0)
                {
                    logger.LogInformation("Writing batch {0} of {1}", i / batchSize + 1, videos.Count() / batchSize);
                    await videoRepository.BatchUpdate(videoBatch);
                    videoBatch = new List<Video>();
                }
            }

            await videoRepository.BatchUpdate(videoBatch);

            return JobStatus.Completed;
        }
    }
}