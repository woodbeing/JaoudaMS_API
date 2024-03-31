using JaoudaMS_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

namespace JaoudaMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuspendedController : ControllerBase
    {
        public readonly Suspend _suspend;

        public SuspendedController(Suspend suspend)
        {
            _suspend = suspend;
        }

        [HttpGet]
        public Suspend getSuspend()
        {
            return _suspend;
        }

        [HttpPost("suspend")]
        public Suspend Suspend()
        {
            _suspend.suspended = true;
            return _suspend;
        }

        [HttpPost("active")]
        public Suspend Active()
        {
            _suspend.suspended = false;
            return _suspend;
        }
    }
}
