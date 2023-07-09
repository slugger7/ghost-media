using Ghost.Data;
using Ghost.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ghos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : Controller
    {
        private readonly ILogger<JobController> logger;
        private readonly IJobRepository jobRepository;

        public JobController(ILogger<JobController> logger, IJobRepository jobRepository)
        {
            this.logger = logger;
            this.jobRepository = jobRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs()
        {
            var jobs = await jobRepository.GetJobs();

            return jobs.ToList();
        }
    }
}