using Ghost.Data;
using Ghost.Data.Enums;
using Ghost.Dtos;
using Ghost.Media;
using Ghost.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ghost.Services.Jobs;

public class ConvertVideoJob : BaseJob
{
    private int VideoId;

    public ConvertVideoJob(
        IServiceScopeFactory scopeFactory,
        int jobId,
        int videoId
    ) : base(scopeFactory, jobId)
    {
        this.VideoId = videoId;
    }

    public override async Task<string> RunJob()
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var logger = new Logger<ConvertJob>(loggerFactory);
            var videoRepository = scope.ServiceProvider.GetRequiredService<IVideoRepository>();
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
            var jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();
            var libraryRepository = scope.ServiceProvider.GetRequiredService<ILibraryRepository>();

            logger.LogInformation("Starting conversion job: {0}", jobId);

            var video = videoRepository.FindById(VideoId, new List<string> { "LibraryPath" });
            if (video == null) throw new NullReferenceException("Could not find video before conversion job");

            var convertJob = await jobRepository.GetConvertJobByJobId(jobId);
            if (convertJob == null) throw new NullReferenceException("Conversion job was not found");

            var newPath = convertJob.Path;

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

            video = videoRepository.FindById(VideoId, new List<string> {
                "VideoGenres.Genre",
                "VideoActors.Actor"
            });
            if (video != null)
            {
                await videoRepository.RelateVideo(VideoId, newVideoEntity.Id);
                await videoRepository.RelateVideo(newVideoEntity.Id, VideoId);

                var actors = video.VideoActors.Select(va => va.Actor);
                await videoRepository.SetActors(newVideoEntity.Id, actors);

                var genres = video.VideoGenres.Select(vg => vg.Genre);
                await videoRepository.SetGenres(newVideoEntity.Id, genres);
                // relations
                //var relations = video.RelatedVideos.Select(rv => rv.Video);

            }

            logger.LogInformation("Completed conversion job: {0}", jobId);

            return JobStatus.Completed;
        }
    }
}