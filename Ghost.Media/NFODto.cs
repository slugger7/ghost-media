using System.Xml.Serialization;

namespace Ghost.Media
{
  public class ActorNfo
  {
    public string name;
  }

  [XmlRoot("movie")]
  public class VideoNfo
  {
    public string title { get; set; }
    public string dateadded { get; set; }
    [XmlElement("genre")]
    public List<string> genres { get; set; }
    [XmlElement("actor")]
    public List<ActorNfo> actors { get; set; }
  }
}