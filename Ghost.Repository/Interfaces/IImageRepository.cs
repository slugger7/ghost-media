using Ghost.Data;

namespace Ghost.Repository;
public interface IImageRepository
{
  PageResult<Image> GetImages(int page = 0, int limit = 10);
  Image? GetImage(int id);
  Image CreateImageForVideo(Video video, string path, string type, int timestamp = -1);
}