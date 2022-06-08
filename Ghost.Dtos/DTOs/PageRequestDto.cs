namespace Ghost.Dtos
{
  public class PageRequestDto
  {
    public int UserId { get; set; }
    public string Search { get; set; } = "";
    public int Page { get; set; } = 0;
    public int Limit { get; set; } = 12;
    public string SortBy { get; set; } = "";
    public bool Ascending { get; set; } = true;
  }
}