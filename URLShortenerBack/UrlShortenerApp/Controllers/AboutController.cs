using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using URL_Shortener.Services;

namespace URL_Shortener.Controllers
{

    [Route("api/about")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private static string description = "This URL Shortener application allows users to shorten long URLs into more manageable short links.";

        [HttpGet("description")]
        public ActionResult<string> GetDescription()
        {
            return Ok(description);
        }

        [HttpPut("description")]
        public ActionResult UpdateDescription([FromBody] string newDescription)
        {
            description = newDescription;
            return NoContent();
        }
    }

}