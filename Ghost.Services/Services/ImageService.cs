using Ghost.Dtos;
using Ghost.Repository;
using Microsoft.Extensions.Logging;

namespace Ghost.Services
{
  public class ImageService : IImageService
  {
    private readonly ILogger<ImageService> logger;
    private readonly IImageRepository imageRepository;

    public ImageService(
      ILogger<ImageService> logger,
      IImageRepository imageRepository
    )
    {
      this.logger = logger;
      this.imageRepository = imageRepository;
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