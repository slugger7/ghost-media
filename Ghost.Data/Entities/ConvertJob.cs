namespace Ghost.Data
{
    public class ConvertJob
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public int? ConstantRateFactor { get; set; }
        public int? VariableBitrate { get; set; }
        public Video Video { get; set; }
        public Job Job { get; set; }
    }
}