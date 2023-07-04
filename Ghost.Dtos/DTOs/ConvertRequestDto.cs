namespace Ghost.Dtos
{
    public class ConvertRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public bool Overwrite { get; set; } = false;
        public bool CopyMetaData { get; set; } = true;
    }
}