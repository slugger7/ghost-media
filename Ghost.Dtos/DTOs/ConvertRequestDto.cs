namespace Ghost.Dtos
{
    public class ConvertRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public int? ConstantRateFactor { get; set; }
        public int? VariableBitrate { get; set; }
        public string ForcePixelFormat { get; set; } = string.Empty;
    }
}