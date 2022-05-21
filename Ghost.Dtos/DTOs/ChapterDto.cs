using Ghost.Data;

namespace Ghost.Dtos
{
  public class ChapterDto
  {
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public long Timestamp { get; set; }
    public ImageDto Image { get; set; }

    public ChapterDto(Chapter chapter)
    {
      this.Id = chapter.Id;
      this.Description = chapter.Description;
      this.Timestamp = chapter.Timestamp;
      this.Image = new ImageDto(chapter.Image);
    }
  }
}