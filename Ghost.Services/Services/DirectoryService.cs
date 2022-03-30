using Ghost.Services.Interfaces;
using Ghost.Media;
using LiteDB;
using Ghost.Data.Entities;

namespace Ghost.Services
{
  public class DirectoryService : IDirectoryService
  {
    private static string connectionString = @"..\Ghost.Data\Ghost.db";
    internal static ILiteCollection<LibraryPath> GetCollection(LiteDatabase db)
    {
      var col = db.GetCollection<LibraryPath>("paths");

      return col;
    }

    public List<string> GetDirectories(string directory)
    {
      return FileFns.ListDirectories(directory);
    }

    public List<string> GetFilesOfTypeInDirectory(string directory, string type)
    {
      return FileFns.ListFilesByExtension(directory, type);
    }

    internal static void DeleteRange(IEnumerable<ObjectId?> ids)
    {
      using (var db = new LiteDatabase(connectionString))
      {
        var col = GetCollection(db);

        foreach (var id in ids)
        {
          col.Delete(id);
        }
      }
    }
  }
}