using Ghost.Data.Enums;

namespace Ghost.Data;
public class Job
{
  public int Id { get; set; }
  public string Status { get; set; } = JobStatus.NotStarted;
  public string Type { get; set; } = string.Empty;
  public DateTime Created { get; set; } = DateTime.UtcNow;
  public DateTime Modified { get; set; }
  public string ThreadName { get; set; } = string.Empty;
}