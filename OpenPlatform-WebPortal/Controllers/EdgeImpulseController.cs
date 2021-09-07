using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using OpenPlatform_WebPortal.Helper;
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

        // ConnectEIProject
        [HttpPost]
        public async Task<ActionResult> ConnectEIProject(string apiKey)
        {
            try
            {
                if (apiKey != null)
                {
                    string path = "https://studio.edgeimpulse.com/v1/api/projects";

                    EdgeImpulseApiHelper resolver = new EdgeImpulseApiHelper(apiKey, _logger);
                    var modelData = await resolver.GetProjects(path);

                    _logger.LogInformation($"Mock: Connected Edge Impulse project with API key: {apiKey}");
                    return StatusCode(200, modelData);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in ConnectEIProject() : {e.Message}");
                return StatusCode(400, new { message = e.Message });
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> GetEIBuilds(string apiKey)
        {
            try
            {
                if (apiKey != null)
                {
                    string projectId = "40654";
                    string type = "nordic-nrf52840-dk";
                    string path = $"https://studio.edgeimpulse.com/v1/api/{projectId}/deployment?type={type}";

                    EdgeImpulseApiHelper resolver = new EdgeImpulseApiHelper(apiKey, _logger);
                    var modelData = await resolver.GetBuiltModels(path);

                    _logger.LogInformation($"Mock: Got model info from project: {apiKey}");
                    return StatusCode(200, modelData);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in GetEIBuilds() : {e.Message}");
                return StatusCode(400, new { message = e.Message });
            }

            return Ok();
        }
    }
}
