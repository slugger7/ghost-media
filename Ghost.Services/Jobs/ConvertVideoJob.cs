using Ghost.Data.Enums;
using Ghost.Dtos;
using Ghost.Media;
using Ghost.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Ghost.Services.Jobs;

public class ConvertVideoJob : BaseJob
{

  public ConvertVideoJob(
      IServiceScopeFactory scopeFactory,
      int jobId
  ) : base(scopeFactory, jobId)
  { }

  public override async Task<string> RunJob()
  {
    using (var scope = scopeFactory.CreateScope())
    {
      var videoRepository = scope.ServiceProvider.GetRequiredService<IVideoRepository>();
      var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
      var jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();
      var libraryRepository = scope.ServiceProvider.GetRequiredService<ILibraryRepository>();

      var convertJob = await jobRepository.GetConvertJobByJobId(jobId);
      if (convertJob == null) throw new NullReferenceException("Conversion job was not found");

      var video = videoRepository.FindById(convertJob.Video.Id, new List<string> { "LibraryPath" });
      if (video == null) throw new NullReferenceException("Could not find video before conversion job");

      var newPath = convertJob.Path;

      if (String.IsNullOrEmpty(video.Path)) throw new NullReferenceException("Video to convert had no path");
      if (String.IsNullOrEmpty(newPath)) throw new NullReferenceException("Path for converted video was null or empty");
      await VideoFns.ConvertVideo(
          inputPath: video.Path,
          outputPath: newPath,
          height: convertJob.Height,
          width: convertJob.Width,
          constantRateFactor: convertJob.ConstantRateFactor,
          variableBitrate: convertJob.VariableBitrate,
          forcePixelFormat: convertJob.ForcePixelFormat);

      var newVideoInfo = VideoFns.GetVideoInformation(newPath);
      if (newVideoInfo == null) throw new NullReferenceException("Could not find video info");

      var libraryPath = await libraryRepository.GetLibraryPathById(video.LibraryPath.Id);
      if (libraryPath == null) throw new NullReferenceException("Library path for converted video was not found");

      var newVideoEntity = await videoRepository.CreateVideo(newPath, newVideoInfo, libraryPath, video.Title);

      imageService.GenerateThumbnailForVideo(new GenerateImageRequestDto
      {
        VideoId = newVideoEntity.Id
      });

      video = videoRepository.FindById(convertJob.Video.Id, new List<string> {
                "VideoGenres.Genre",
                "VideoActors.Actor",
                "RelatedVideos.RelatedTo"
            });
      if (video != null)
      {
        await videoRepository.RelateVideo(convertJob.Video.Id, newVideoEntity.Id);
        await videoRepository.RelateVideo(newVideoEntity.Id, convertJob.Video.Id);

        var actors = video.VideoActors.Select(va => va.Actor);
        await videoRepository.SetActors(newVideoEntity.Id, actors);

        var genres = video.VideoGenres.Select(vg => vg.Genre);
        await videoRepository.SetGenres(newVideoEntity.Id, genres);

        var relations = video.RelatedVideos.Select(rv => rv.RelatedTo);
        foreach (var relation in relations)
        {
          if (newVideoEntity.Id == relation.Id) continue;

          await videoRepository.RelateVideo(newVideoEntity.Id, relation.Id);
          await videoRepository.RelateVideo(relation.Id, newVideoEntity.Id);
        }
      }

      return JobStatus.Completed;
    }
  }
}