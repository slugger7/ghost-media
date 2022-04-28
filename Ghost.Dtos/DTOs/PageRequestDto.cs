namespace Ghost.Dtos
{
  public class PageRequestDto
  {
    public string Search { get; set; } = "";
    public int Page { get; set; } = 0;
    public int Limit { get; set; } = 12;
  }
}