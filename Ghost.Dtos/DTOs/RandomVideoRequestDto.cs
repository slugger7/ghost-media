namespace Ghost.Dtos
{
    public class RandomVideoRequestDto
    {
        public string Search { get; set; } = "";
        public string WatchState { get; set; } = Ghost.Data.Enums.WatchState.All;
        public string[]? Genres { get; set; }
    }
}