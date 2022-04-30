using Ghost.Dtos;

namespace Ghost.Services
{
  public interface IImageService
  {
    ImageDto GetImage(int id);
    ImageDto GenerateThumbnailForVideo(int videoId, bool overwrite, int timestamp);
  }
}