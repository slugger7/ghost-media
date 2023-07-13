using Ghost.Dtos;
using Ghost.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ghost.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobController : Controller
{
    private readonly ILogger<JobController> logger;
    private readonly IJobService jobService;

    public JobController(
      ILogger<JobController> logger,
      IJobService jobService
    )
    {
        this.logger = logger;
        this.jobService = jobService;
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> StartJob(int id)
    {
        await jobService.StartJob(id);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> DeleteJob(int id)
    {
        await jobService.DeleteJob(id);
        return Ok();
    }
}