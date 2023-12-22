namespace Ghost.Data;
public class RelatedVideo
{
  public int Id { get; set; }
  public Video Video { get; set; } = null!;
  public Video RelatedTo { get; set; } = null!;
}