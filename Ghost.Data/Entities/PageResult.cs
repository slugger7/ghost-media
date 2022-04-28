namespace Ghost.Data
{
  public class PageResult<T>
  {
    public int Total { get; set; }
    public int Page { get; set; }
    public IEnumerable<T> Content { get; set; } = new List<T>();
  }
}