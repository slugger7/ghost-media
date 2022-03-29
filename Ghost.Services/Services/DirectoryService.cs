using Ghost.Services.Interfaces;
using Ghost.Media;

namespace Ghost.Services
{
  public class DirectoryService : IDirectoryService
  {
    public List<string> GetDirectories(string directory)
    {
      return FileFns.ListDirectories(directory);
    }

    public List<string> GetFilesOfTypeInDirectory(string directory, string type)
    {
      return FileFns.ListFilesByExtension(directory, type);
    }
  }
}