namespace Ghost.Dtos
{
    public class ConvertRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public bool Overwrite { get; set; } = false;
        public bool CopyMetaData { get; set; } = true;
    }
}