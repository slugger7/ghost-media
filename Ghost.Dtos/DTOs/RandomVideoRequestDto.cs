namespace Ghost.Dtos
{
    public class RandomVideoRequestDto
    {
        public string Search = string.Empty;
        public string WatchState = Ghost.Data.Enums.WatchState.All;
        public string[]? Genres;
    }
}