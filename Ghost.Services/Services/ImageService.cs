using Ghost.Dtos;
using Ghost.Media;
using Ghost.Repository;
using Microsoft.Extensions.Logging;

namespace Ghost.Services
{
  public class ImageService : IImageService
  {
    private readonly ILogger<ImageService> logger;
    private readonly IImageRepository imageRepository;
    private readonly IVideoRepository videoRepository;
    private readonly IImageIoService imageIoService;

    public ImageService(
      ILogger<ImageService> logger,
      IImageRepository imageRepository,
      IVideoRepository videoRepository,
      IImageIoService imageIoService
    )
    {
      this.logger = logger;
      this.imageRepository = imageRepository;
      this.videoRepository = videoRepository;
      this.imageIoService = imageIoService;
    }

    public ImageDto GenerateThumbnailForVideo(int videoId, string type, bool overwrite, int timestamp)
    {
      var video = videoRepository.FindById(videoId);
      if (video is null)
      {
        logger.LogWarning("Video not found {0}", videoId);
        throw new NullReferenceException("Video not found");
      }

      if (!overwrite)
      {
        var videoImage = video.VideoImages.FirstOrDefault(vi => vi.Type.Equals(type));
        if (videoImage is not null)
        {
          logger.LogInformation("Image found not regenerating {0}", videoId);
          return new ImageDto(videoImage.Image);
        }
      }

      var outputPath = ImageIoService.GenerateFileName(video.Path, video.Title, ".png");
      imageIoService.GenerateImage(video.Path, outputPath);

      var image = imageRepository.CreateImageForVideo(video, outputPath);
      return new ImageDto(image);
    }

    public ImageDto GetImage(int id)
    {
      logger.LogDebug("Getting image {0}", id);
      var image = imageRepository.GetImage(id);
      if (image is null)
      {
        logger.LogWarning("Image not found {0}", id);
        throw new NullReferenceException("Image not found");
      }

      return new ImageDto(image);
    }
  }
}