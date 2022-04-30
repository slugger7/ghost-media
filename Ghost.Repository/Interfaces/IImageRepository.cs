using Ghost.Data;

namespace Ghost.Repository
{
  public interface IImageRepository
  {
    Image? GetImage(int id);
  }
}