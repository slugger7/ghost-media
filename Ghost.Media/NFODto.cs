using System.Xml.Serialization;

namespace Ghost.Media;
public class ActorNfo
{
  public string name { get; set; } = string.Empty;
}

[XmlRoot("movie")]
public class VideoNfo
{
  public string title { get; set; } = string.Empty;
  public string dateadded { get; set; } = string.Empty;
  [XmlElement("genre")]
  public List<string> genres { get; set; } = new List<string>();
  [XmlElement("actor")]
  public List<ActorNfo> actors { get; set; } = new List<ActorNfo>();
}