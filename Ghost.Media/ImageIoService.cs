using FFMpegCore;
using Microsoft.Extensions.Logging;
using System.Drawing;

namespace Ghost.Media
{
  public class ImageIoService : IImageIoService
  {
    private readonly ILogger<ImageIoService> logger;

    public ImageIoService(
      ILogger<ImageIoService> logger
    )
    {
      this.logger = logger;
    }

    private static Size CalculateSize(Size maxSize, Size currentSize)
    {
      var newSize = new Size(currentSize.Width, currentSize.Height);
      if (newSize.Width > maxSize.Width)
      {
        var newHeight = ((double)maxSize.Width / (double)currentSize.Width) * (double)currentSize.Height;
        newSize.Height = (int)newHeight;
        newSize.Width = maxSize.Width;
      }
      if (newSize.Height > maxSize.Height)
      {
        newSize.Width = (int)((maxSize.Height / currentSize.Height) * currentSize.Width);
        newSize.Height = maxSize.Height;
      }

      return newSize;
    }

    public static string GenerateFileName(string basePath, string extension)
    => basePath
        .Substring(
          0,
          basePath.LastIndexOf('.')
        ) + extension;

    public void GenerateImage(string videoPath, string outputPath, int captureTimeMillis = -1, int maxWidth = 400, int maxHeight = 400)
    {
      if (File.Exists(outputPath)) return;
      var videoInfo = VideoFns.GetVideoInformation(videoPath);
      if (videoInfo == null) return;
      if (captureTimeMillis < 0)
      {
        var percentage = 0.25;
        captureTimeMillis = (int)Math.Floor(videoInfo.Duration.TotalMilliseconds * percentage);
      }
      var captureTime = TimeSpan.FromMilliseconds(captureTimeMillis);
      var scaledSize = CalculateSize(new Size(maxWidth, maxHeight), new Size(videoInfo.Width, videoInfo.Height));

      FFMpeg.Snapshot(videoPath, outputPath, scaledSize, captureTime: captureTime);
    }

    public IEnumerable<Tuple<int, string>> CreateChapterImages(string videoPath, string filename, string outputDirectory, List<int> chapterMarks)
    {
      var images = chapterMarks.Select(chapter =>
      {
        var fileName = $"{filename}-{chapter.ToString()}.png";
        this.GenerateImage(videoPath, $"{outputDirectory}{Path.DirectorySeparatorChar}{fileName}", chapter);
        return new Tuple<int, string>(chapter, fileName);
      });
      return images;
    }
  }
}