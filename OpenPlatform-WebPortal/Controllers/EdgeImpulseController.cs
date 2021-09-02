using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using OpenPlatform_WebPortal.Models;


namespace OpenPlatform_WebPortal.Controllers
{
    public class EdgeImpulseController : Controller
    {

        private readonly ILogger<EdgeImpulseController> _logger;
        private readonly AppSettings _appSettings;

        public EdgeImpulseController(IOptions<AppSettings> optionsAccessor, ILogger<EdgeImpulseController> logger)
        {
            _logger = logger;
            _appSettings = optionsAccessor.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetEdgeImpulseProject()
        {
            try
            {
                var result = _appSettings.EdgeImpulse.ApiKey;

                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
