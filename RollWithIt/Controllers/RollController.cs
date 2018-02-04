using Microsoft.AspNetCore.Mvc;
using RollWithIt.Domain;

namespace RollWithIt.Controllers
{
    [Route("api/[controller]")]
    public class RollController : Controller
    {
        [HttpGet("{rollString}")]
        public string Get(string rollString)
        {
            Roll roll = Roll.Create(rollString);
            return roll.ToString();
        }
    }
}
