using Ghost.Data;
using Microsoft.Extensions.Logging;

namespace Ghost.Repository;
public class ImageRepository : IImageRepository
{
  private readonly GhostContext context;
  private readonly ILogger<ImageRepository> logger;

  public ImageRepository(
    ILogger<ImageRepository> logger,
    GhostContext context)
  {
    this.context = context;
    this.logger = logger;
  }

  public Image CreateImageForVideo(Video video, string path, string type, int timestamp = -1)
  {
    if (video is null)
    {
      logger.LogWarning("Video not provided");
      throw new NullReferenceException("Video was null");
    }

    var videoImage = video.VideoImages.Find(vi => vi.Type.Equals(type));
    if (videoImage == null)
    {

      var image = new Image
      {
        Name = video.Title,
        Path = path
      };

      context.Images.Add(image);

      videoImage = new VideoImage
      {
        Type = "thumbnail",
        Video = video,
        Image = image
      };

      context.VideoImages.Add(videoImage);
    }
    else
    {
      if (timestamp > 0)
      {
        videoImage.Image.Name = video.Title + timestamp.ToString();
      }
    }

    context.SaveChanges();

    return videoImage.Image;
  }

  public Image? GetImage(int id)
  {
    return context.Images.Find(id);
  }

  public PageResult<Image> GetImages(int page = 0, int limit = 10)
  {
    var images = context.Images
      .OrderBy(i => i.Name);

    return new PageResult<Image>
    {
      Total = images.Count(),
      Page = page,
      Content = images
        .Skip(limit * page)
        .Take(limit)
    };
  }
}