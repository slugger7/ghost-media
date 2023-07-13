namespace Ghost.Services;

public interface IJobService
{
    Task StartJob(int id);
    Task DeleteJob(int id);
}