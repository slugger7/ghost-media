namespace Ghost.Data.Ents
{
  public class Library
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<LibraryPath> Paths { get; set; }
  }
}