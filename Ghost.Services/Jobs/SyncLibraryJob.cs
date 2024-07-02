using Ghost.Data;
using Ghost.Data.Enums;
using Ghost.Media;
using Ghost.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ghost.Services.Jobs;

public class SyncLibraryJob : BaseJob
{
  public SyncLibraryJob(IServiceScopeFactory scopeFactory, int jobId)
      : base(scopeFactory, jobId)
  { }

  public override async Task<string> RunJob()
  {
    using (var scope = scopeFactory.CreateScope())
    {
      var jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();
      var libraryRepository = scope.ServiceProvider.GetRequiredService<ILibraryRepository>();
      var directoryService = scope.ServiceProvider.GetRequiredService<IDirectoryService>();
      var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
      var logger = loggerFactory.CreateLogger<SyncLibraryJob>();

      var syncJob = await jobRepository.GetSyncJobByJobId(jobId);
      if (syncJob == null) throw new NullReferenceException("Could not find sync job in sync library job");

      var batchSize = 100;
      var library = syncJob.Library;
      if (library == null) throw new NullReferenceException("Library not found");

      var videoTypes = new List<string> { "mp4", "m4v", "mkv", "avi", "wmv", "flv", "webm", "f4v", "mpg", "m2ts", "mov" };

      foreach (var path in library.Paths)
      {
        var currentVideos = path.Videos;
        if (path.Path == null) continue;

        var directories = directoryService.GetDirectories(path.Path);
        List<string> videos = videoTypes.Aggregate(
            new List<string>(),
            (acc, fileType) => acc.Concat(directoryService.GetFilesOfTypeInDirectory(path.Path, fileType)).ToList()
        );

        var dirIndex = 0;

        while (directories.Count() > dirIndex)
        {
          var currentDirectory = directories.ElementAt(dirIndex++);

          directories = directories.Concat(directoryService.GetDirectories(currentDirectory)).ToList();

          videos = videoTypes.Aggregate(
              videos,
              (acc, fileType) => acc.Concat(directoryService.GetFilesOfTypeInDirectory(currentDirectory, fileType)).ToList()
          );
        }

        var videoEntities = videos
          .Where(v => !currentVideos.Any(cv => cv.Path.Equals(v)))
          .Select(v =>
          {
            var videoSplit = v.Split(Path.DirectorySeparatorChar);
            var fileName = videoSplit[videoSplit.Length - 1];
            var initialTitle = fileName.Substring(0, fileName.LastIndexOf('.'));
            logger.LogInformation("Adding video: {0}", initialTitle);
            var video = new Video
            {
              Path = v,
              FileName = fileName,
              Title = initialTitle.Trim()
            };

            return video;
          })
          .ToList();

        var videoBatch = new List<Video>();
        for (var i = 0; i < videoEntities.Count(); i++)
        {
          var video = videoEntities.ElementAt(i);
          var metaData = VideoFns.GetVideoInformation(video.Path);
          if (metaData != null)
          {
            video.Created = metaData.Created;
            video.Size = metaData.Size;
            video.Height = metaData.Height;
            video.Width = metaData.Width;
            video.Runtime = metaData.Duration.TotalMilliseconds;
            video.LastMetadataUpdate = DateTime.UtcNow;
          }
          videoBatch.Add(video);

          if (i % batchSize == 0)
          {
            logger.LogInformation("Writing batch {0} of {1}", i / batchSize, videoEntities.Count() / batchSize);
            libraryRepository.AddVideosToPath(path.Id, videoBatch);
            videoBatch = new List<Video>();
          }
        }
        libraryRepository.AddVideosToPath(path.Id, videoBatch);
        logger.LogInformation("Synced {0} new videos", videoEntities.Count());


      }

      return JobStatus.Completed;
    }
  }
}