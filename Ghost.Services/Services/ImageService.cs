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

    public PageResultDto<ImageDto> GetImages(PageRequestDto pageRequest)
    {
      var pageResult = imageRepository.GetImages(pageRequest.Page, pageRequest.Limit);

      return new PageResultDto<ImageDto>
      {
        Total = pageResult.Total,
        Page = pageResult.Total,
        Content = pageResult.Content
          .Select(i => new ImageDto(i))
          .ToList()
      };
    }
  }
}