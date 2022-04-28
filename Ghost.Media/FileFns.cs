namespace Ghost.Media;
public static class FileFns
{
  public static List<string> ListDirectories(string directory) =>
    new List<string>(Directory.EnumerateDirectories(directory));

  public static List<string> ListFiles(string directory) =>
    new List<string>(Directory.EnumerateFiles(directory));

  public static string ExtractExtension(this string file)
  {
    var fileSplit = file.Split('.');
    if (fileSplit.Length > 0)
    {
      return fileSplit[fileSplit.Length - 1];
    }
    return String.Empty;
  }

  public static List<string> ListFilesByExtension(string directory, string extension) =>
    ListFiles(directory)
    .Where(f => f.ExtractExtension().Equals(extension))
    .ToList();

  public static string GetFilePathWithoutExtension(string path) => path.Substring(0, path.LastIndexOf('.'));
}
