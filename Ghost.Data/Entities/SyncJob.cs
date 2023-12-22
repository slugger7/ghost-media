namespace Ghost.Data;

public class SyncJob
{
    public int Id { get; set; }
    public Library Library { get; set; } = null!;
    public Job Job { get; set; } = null!;
}