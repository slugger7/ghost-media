namespace Ghost.Media
{
  public interface IImageIoService
  {
    void GenerateImage(string videoPath, string outputPath, int captureTimeMillis = -1, int maxWidth = 720, int maxHeight = 480);
  }
}