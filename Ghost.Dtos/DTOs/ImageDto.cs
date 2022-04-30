using Ghost.Data;

namespace Ghost.Dtos
{
  public class ImageDto
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ImageDto(Image image)
    {
      this.Id = image.Id;
      this.Name = image.Name;
    }
  }
}