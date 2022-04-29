using FFMpegCore;
using System.Drawing;

namespace Ghost.Media
{
  public static class ImageFns
  {
    private static Size CalculateSize(Size maxSize, Size currentSize)
    {
      Console.Write("Current width: ");
      Console.WriteLine(currentSize.Width);
      Console.Write("Current height: ");
      Console.WriteLine(currentSize.Height);
      var newSize = new Size(currentSize.Width, currentSize.Height);
      if (newSize.Width > maxSize.Width)
      {
        Console.WriteLine("Scaling based on width");
        var newHeight = ((double)maxSize.Width / (double)currentSize.Width) * (double)currentSize.Height;
        Console.WriteLine(newHeight);
        newSize.Height = (int)newHeight;
        newSize.Width = maxSize.Width;
        Console.Write("Width scale width: ");
        Console.WriteLine(newSize.Width);
        Console.Write("Width scale height: ");
        Console.WriteLine(newSize.Height);
      }
      if (newSize.Height > maxSize.Height)
      {
        Console.WriteLine("Scaling based on height");
        newSize.Width = (int)((maxSize.Height / currentSize.Height) * currentSize.Width);
        newSize.Height = maxSize.Height;
      }

      Console.Write("Width: ");
      Console.WriteLine(newSize.Width);
      Console.Write("Height: ");
      Console.WriteLine(newSize.Height);
      return newSize;
    }
    public static void GenerateImage(string path, string outputPath, int captureTimeMillis = -1, int maxWidth = 720, int maxHeight = 480)
    {
      Console.Write("Before file exists check: ");
      Console.WriteLine(DateTime.UtcNow);
      if (File.Exists(outputPath)) return;
      Console.Write("Before video info: ");
      Console.WriteLine(DateTime.UtcNow);
      var videoInfo = VideoFns.GetVideoInformation(path);
      if (videoInfo == null) return;
      if (captureTimeMillis < 0)
      {
        var percentage = 0.25;
        captureTimeMillis = (int)Math.Floor(videoInfo.Duration.TotalMilliseconds * percentage);
      }
      var captureTime = TimeSpan.FromMilliseconds(captureTimeMillis);
      var scaledSize = CalculateSize(new Size(maxWidth, maxHeight), new Size(videoInfo.Width, videoInfo.Height));
      Console.Write("Before snapshotting: ");
      Console.WriteLine(DateTime.UtcNow);
      FFMpeg.Snapshot(path, outputPath, scaledSize, captureTime: captureTime);
      Console.Write("Done with snapshot: ");
      Console.WriteLine(DateTime.UtcNow);
    }
  }
}