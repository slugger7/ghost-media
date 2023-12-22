namespace Ghost.Data;
public class Progress
{
  public int Id { get; set; }
  public double Timestamp { get; set; }
  public Video Video { get; set; } = null!;
  public User User { get; set; } = null!;
}