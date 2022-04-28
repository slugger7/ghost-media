using System.Xml.Serialization;
using Ghost.Data;

namespace Ghost.Media
{
  public static class NfoFns
  {
    public static VideoNfo? Hydrate(Video video)
    {
      var nfoPath = $"{FileFns.GetFilePathWithoutExtension(video.Path)}.nfo";
      if (File.Exists(nfoPath))
      {
        using var fileStream = File.Open(nfoPath, FileMode.Open);
        var serializer = new XmlSerializer(typeof(VideoNfo));
        var nfoDto = (VideoNfo)serializer.Deserialize(fileStream);
        return nfoDto;
      }
      return null;
    }
  }
}