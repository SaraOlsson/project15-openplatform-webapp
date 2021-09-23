using System;
using System.Collections.Generic;
using System.Threading;
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
        private IIoTHubDpsHelper _hubHelper;

        public EdgeImpulseController(IIoTHubDpsHelper helper, IOptions<AppSettings> optionsAccessor, ILogger<EdgeImpulseController> logger)
        {
            _hubHelper = helper;
            _logger = logger;
            _appSettings = optionsAccessor.Value;
        }

        // NOTE: needed?
        public IActionResult Index()
        {
            return View();
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

        // ConnectEIModelToDevice
        [HttpPost]
        public async Task<IActionResult> ConnectEIModelToDevice(string deviceId, string projectName)
        {
            try
            {
                //string tagsJson = "{\"eiModel\":\"ElephantEdge\"}";

                string tagsJson = "{\"eiModel\":\""+ projectName + "\"}";
                await _hubHelper.UpdateDeviceTwin(deviceId, tagsJson);
                return StatusCode(200);
                
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in ConnectEIModelToDevice() : {e.Message}");
                return StatusCode(400, new { message = e.Message });
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DownloadModel(string apiKey, string type, string projectId)
        {
            try
            {
                if (apiKey != null)
                {
                    // string projectId = "40654";
                    // string type = "nordic-nrf52840-dk";
                    string path = $"https://studio.edgeimpulse.com/v1/api/{projectId}/deployment/download?type={type}";

                   EdgeImpulseApiHelper resolver = new EdgeImpulseApiHelper(apiKey, _logger);
                    var modelData = await resolver.GetModelBinary(path);

                    _logger.LogInformation($"Mock: Got model file info from project: {apiKey}");
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

        // Refresh Device List from IoT Hub (in progress)
        [HttpGet]
        public async Task<ActionResult> RefreshFirmwareOptions(int delay, string selectedDevice)
        {

            var eiFirmwareList = new EiFirmwareListViewModel();

            List<FirmwareOptionViewModel> Options = new List<FirmwareOptionViewModel>();
            Options.Add(new FirmwareOptionViewModel()
            {
                OptionName = "Nordic NRF52840 DK",
                OptionKey = "nordic-nrf52840-dk"
            });
            Options.Add(new FirmwareOptionViewModel()
            {
                OptionName = "C++ library",
                OptionKey = "zip"
            });

            eiFirmwareList.Options = Options;

            try
            {
                Thread.Sleep(delay);

                ViewBag.EiFirmwareListViewModel = eiFirmwareList;

                //ViewBag.IoTHubDeviceList = await _helper.GetIoTHubDevices();
                //ViewBag.IoTHubDeviceList.SelectedIoTHubDevice = selectedDevice;
                return PartialView("EiFirmwareListPartialView");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in RefreshIoTHubDevices() : {e.Message}");
                return StatusCode(400, new { message = e.Message });
            }
        }
    }
}
