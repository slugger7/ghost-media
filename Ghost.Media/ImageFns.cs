using FFMpegCore;

namespace Ghost.Media
{
  public static class ImageFns
  {
    public static void GenerateImage(string path, string outputPath, int captureTimeMillis = -1)
    {
      if (File.Exists(outputPath)) return;

      if (captureTimeMillis < 0)
      {
        var percentage = 0.25;
        var videoInfo = VideoFns.GetVideoInformation(path);
        if (videoInfo == null) return;
        captureTimeMillis = (int)Math.Floor(videoInfo.Duration.TotalMilliseconds * percentage);
      }
      var captureTime = TimeSpan.FromMilliseconds(captureTimeMillis);

      FFMpeg.Snapshot(path, outputPath, captureTime: captureTime);
    }
  }
}