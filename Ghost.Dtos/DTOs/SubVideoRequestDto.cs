namespace Ghost.Dtos;
public class SubVideoRequestDto
{
  public string Name { get; set; } = string.Empty;
  public int StartMillis { get; set; } = 0;
  public int EndMillis { get; set; }
}