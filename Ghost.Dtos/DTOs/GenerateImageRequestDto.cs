namespace Ghost.Dtos
{
  public class GenerateImageRequestDto
  {
    public int VideoId { get; set; }
    public string Type { get; set; } = string.Empty;
    public bool Overwrite { get; set; } = false;
    public int Timestamp { get; set; } = -1;
  }
}