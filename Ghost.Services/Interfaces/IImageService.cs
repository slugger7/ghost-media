using Ghost.Dtos;

namespace Ghost.Services
{
  public interface IImageService
  {
    PageResultDto<ImageDto> GetImages(PageRequestDto pageRequest);
    ImageDto GetImage(int id);
  }
}