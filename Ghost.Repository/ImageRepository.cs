using Ghost.Data;


namespace Ghost.Repository
{
  public class ImageRepository : IImageRepository
  {
    private readonly GhostContext context;
    public ImageRepository(GhostContext context)
    {
      this.context = context;
    }
    public Image? GetImage(int id)
    {
      return context.Images.Find(id);
    }
  }
}