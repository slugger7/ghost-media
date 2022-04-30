using Ghost.Data;
using Microsoft.Extensions.Logging;

namespace Ghost.Repository
{
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

    public Image CreateImageForVideo(Video video, string path)
    {
      if (video is null)
      {
        logger.LogWarning("Video not provided");
        throw new NullReferenceException("Video was null");
      }

      var image = new Image
      {
        Name = video.Title,
        Path = path
      };

      context.Images.Add(image);

      var videoImage = new VideoImage
      {
        Type = "thumbnail",
        Video = video,
        Image = image
      };

      context.VideoImages.Add(videoImage);

      context.SaveChanges();

      return image;
    }

    public Image? GetImage(int id)
    {
      return context.Images.Find(id);
    }
  }
}