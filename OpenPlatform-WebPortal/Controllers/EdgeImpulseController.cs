using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenPlatform_WebPortal.Helper;
using OpenPlatform_WebPortal.Models;


namespace OpenPlatform_WebPortal.Controllers
{

    public class EdgeImpulseController : Controller
    {

        private readonly ILogger<EdgeImpulseController> _logger;
        private readonly AppSettings _appSettings;
        private IIoTHubDpsHelper _hubHelper;

        public EdgeImpulseController(IIoTHubDpsHelper helper, IOptions<AppSettings> optionsAccessor, ILogger<EdgeImpulseController> logger)
        {
            _hubHelper = helper;
            _logger = logger;
            _appSettings = optionsAccessor.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Get Edge Impulse projects
        [HttpPost]
        public async Task<ActionResult> GetEIProjects(string apiKey)
        {
            try
            {
                if (apiKey != null)
                {
                    string PATH = "https://studio.edgeimpulse.com/v1/api/projects";

                    EdgeImpulseApiHelper resolver = new EdgeImpulseApiHelper(apiKey, _logger);
                    var modelData = await resolver.GetProjects(PATH);

                    if(modelData == null)
                    {
                        return StatusCode(400, new { message = "Could not get Edge Impulse projects" });
                    }

                    _logger.LogInformation($"GetEIProjects: Connected Edge Impulse project with API key: {apiKey}");
                    return StatusCode(200, modelData);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in GetEIProjects() : {e.Message}");
                return StatusCode(400, new { message = e.Message });
            }

            return Ok();
        }

        // Update device Twin with Edge Impulse project information
        [HttpPost]
        public async Task<IActionResult> ConnectEIModelToDevice(string deviceId, string projectName, string projectId)
        {
            try
            {
                string tagsJson = "{\"eiModelName\":\""+ projectName + "\", \"eiProjectId\":\"" + projectId + "\"}";
                await _hubHelper.UpdateDeviceTwin(deviceId, tagsJson);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in ConnectEIModelToDevice() : {e.Message}");
                return StatusCode(400, new { message = e.Message });
            }

            return StatusCode(200);
        }

        [HttpPost]
        public async Task<IActionResult> DownloadModel(string apiKey, string type, string projectId)
        {
            try
            {
                if (apiKey != null)
                {
                    string path = $"https://studio.edgeimpulse.com/v1/api/{projectId}/deployment/download?type={type}";

                   EdgeImpulseApiHelper resolver = new EdgeImpulseApiHelper(apiKey, _logger);
                    var modelData = await resolver.GetModelBinary(path);

                    _logger.LogInformation($"DownloadModel: Got model file info from project: {apiKey}");
                    return StatusCode(200, modelData);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in DownloadModel() : {e.Message}");
                return StatusCode(400, new { message = e.Message });
            }

            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> RefreshFirmwareOptions(string apiKey, string projectId, string type)
        {

            EdgeImpulseApiHelper resolver = new EdgeImpulseApiHelper(apiKey, _logger);
            EiFirmwareListViewModel eiFirmwareList =  await resolver.GetEiFirmwareList(projectId);

            try
            {
                ViewBag.EiFirmwareList = eiFirmwareList;

                return PartialView("EiFirmwareListPartialView");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in RefreshFirmwareOptions() : {e.Message}");
                return StatusCode(400, new { message = e.Message });
            }
        }
    }
}
