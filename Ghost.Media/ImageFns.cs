using FFMpegCore;

namespace Ghost.Media
{
  public static class ImageFns
  {
    public static void GenerateImage(string path, string outputPath)
    {
      if (File.Exists(outputPath)) return;

      FFMpeg.Snapshot(path, outputPath, captureTime: TimeSpan.FromSeconds(1));
    }
  }
}