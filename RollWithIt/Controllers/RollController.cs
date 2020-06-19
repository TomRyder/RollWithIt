using Microsoft.AspNetCore.Mvc;
using RollWithIt.Application;
using RollWithIt.Domain;

namespace RollWithIt.Controllers
{
    [Route("api/[controller]")]
    public class RollController : Controller
    {
        [HttpGet("")]
        public Roll Get(string rollString)
        {
            RollApplication rollApplication = new RollApplication();
            return rollApplication.GetRoll(rollString);
        }
    }
}
