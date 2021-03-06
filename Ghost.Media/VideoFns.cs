using FFMpegCore;
using Ghost.Dtos;

namespace Ghost.Media
{
  public static class VideoFns
  {
    public static VideoMetaDataDto? GetVideoInformation(string path)
    {
      var mediaInfo = FFProbe.Analyse(path);
      if (mediaInfo.PrimaryVideoStream == null) return default;
      var fileInfo = new FileInfo(path);
      return new VideoMetaDataDto
      {
        Duration = mediaInfo.Duration,
        Format = mediaInfo.Format.FormatName,
        FormatLong = mediaInfo.Format.FormatLongName,
        Width = mediaInfo.PrimaryVideoStream.Width,
        Height = mediaInfo.PrimaryVideoStream.Height,
        Created = fileInfo.CreationTimeUtc,
        Size = fileInfo.Length
      };
    }
  }
}