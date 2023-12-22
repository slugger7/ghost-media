using Ghost.Data;

namespace Ghost.Media;
public interface INfoService
{
  public VideoNfo? HydrateVideo(Video video);
}