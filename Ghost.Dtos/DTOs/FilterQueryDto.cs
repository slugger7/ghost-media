namespace Ghost.Dtos
{
    public class FilterQueryDto
    {
        public string WatchState { get; set; } = String.Empty;
        public string[]? Genres { get; set; }
    }
}