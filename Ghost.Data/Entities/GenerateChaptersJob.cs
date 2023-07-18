namespace Ghost.Data;

public class GenerateChaptersJob
{
    public int Id { get; set; }
    public bool Overwrite { get; set; }
    public Library Library { get; set; }
    public Job Job { get; set; }
}