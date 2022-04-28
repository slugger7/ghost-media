namespace Ghost.Data
{
  public class Library
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual List<LibraryPath> Paths { get; set; }
  }
}