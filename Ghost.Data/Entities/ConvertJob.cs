namespace Ghost.Data
{
    public class ConvertJob
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Video Video { get; set; }
        public Job Job { get; set; }
    }
}