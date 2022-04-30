using System.Xml.Serialization;
using Ghost.Data;
using Microsoft.Extensions.Logging;

namespace Ghost.Media
{
  public class NfoService : INfoService
  {
    private readonly ILogger<NfoService> logger;

    public NfoService(ILogger<NfoService> logger)
    {
      this.logger = logger;
    }
    public VideoNfo? HydrateVideo(Video video)
    {
      logger.LogInformation("Hydrating video {0}", video.Id);
      var nfoPath = $"{FileFns.GetFilePathWithoutExtension(video.Path)}.nfo";

      using var fileStream = File.Open(nfoPath, FileMode.Open);
      var serializer = new XmlSerializer(typeof(VideoNfo));
      var deserializedNfo = serializer.Deserialize(fileStream);
      if (deserializedNfo is not null)
      {
        logger.LogDebug("Nfo deserialized");
        return (VideoNfo)deserializedNfo;
      }

      logger.LogDebug("Could not deserialize nfo");
      return null;
    }
  }
}