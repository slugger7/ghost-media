namespace Ghost.Dtos;
public class PageResultDto<T>
{

  public int Total { get; set; }
  public int Page { get; set; }
  public List<T> Content { get; set; } = new List<T>();
}