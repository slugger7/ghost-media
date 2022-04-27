namespace Ghost.Data.Ents
{
  public class LibraryPath
  {

    public int Id { get; set; }
    public string Path { get; set; }
    public virtual List<Video> Videos { get; set; }
  }
}