namespace Ghost.Services.Interfaces
{
  public interface IDirectoryService
  {
    List<string> GetDirectories(string directory);
    List<string> GetFilesOfTypeInDirectory(string directory, string type);
  }
}