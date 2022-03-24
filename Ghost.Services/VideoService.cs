using LiteDB;
using Ghost.Services.Interfaces;
using Ghost.Data.Entities;
using Ghost.Dtos;
using Ghost.Media;

namespace Ghost.Services
{
  public class VideoService : IVideoService
  {
    private string connectionString = @"..\Ghost.Data\Ghost.db";
    private string collectionName = "videos";

    public void UpsertVideos(List<string> videos)
    {
      using (var db = new LiteDatabase(connectionString))
      {
        var col = db.GetCollection<Video>(collectionName);
        col.EnsureIndex(x => x.Path);

        foreach (var video in videos)
        {
          var videoSplit = video.Split(Path.DirectorySeparatorChar);
          var fileName = videoSplit[videoSplit.Length - 1];
          var videoEntity = new Video
          {
            Path = video,
            FileName = fileName,
            Title = fileName
          };
          
          col.Insert(videoEntity);
        }
      }
    }

    public PageResultDto<VideoDto> GetVideos(int page, int limit)
    {
      using (var db = new LiteDatabase(connectionString))
      {
        var col = db.GetCollection<Video>(collectionName);

        var size = col.Count();

        var videos = col.Query()
          .Limit(limit)
          .Skip(limit * page)
          .ToEnumerable()
          .Select(v => new VideoDto(v))
          .ToList();
          
        db.Dispose();

        return new PageResultDto<VideoDto>
        {
          Total = size,
          Page = page,
          Content = videos
        };
      }
    }

    public VideoDto GetVideoById(string id)
    {
      var _id = new ObjectId(id);

      using (var db = new LiteDatabase(connectionString))
      {
        var col = db.GetCollection<Video>(collectionName);

        return new VideoDto(col.FindOne(v => v._id == _id));
      }
    }

    public List<string> RefreshVideos()
    {
      var dir = @"C:\dev\GhostMedia\assets";
      var ext = "mp4";
      IEnumerable<string> directories = FileFns.ListDirectories(dir);
      IEnumerable<string> files = FileFns.ListFilesByExtension(dir, ext);
      var dirIndex = 0;
      while (directories.Count() > dirIndex)
      {
        var currentDir = directories.ElementAt(dirIndex++);
        Console.WriteLine(currentDir);
        directories = directories.Concat(FileFns.ListDirectories(currentDir));
        files = files.Concat(FileFns.ListFilesByExtension(currentDir, ext));
      }

      this.UpsertVideos(files.ToList());

      return files.ToList();
    }
  }
}