using Ghost.Data;

namespace Ghost.Repository
{
  public interface IImageRepository
  {
    Image? GetImage(int id);
    Image CreateImageForVideo(Video video, string path);
  }
}