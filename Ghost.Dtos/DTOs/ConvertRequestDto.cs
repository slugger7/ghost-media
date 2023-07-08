namespace Ghost.Dtos
{
    public class ConvertRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public bool CopyMetaData { get; set; } = true;
    }
}