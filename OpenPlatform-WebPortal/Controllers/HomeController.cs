using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenPlatform_WebPortal.Helper;
using OpenPlatform_WebPortal.Models;

namespace Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppSettings _appSettings;
        private readonly IIoTHubDpsHelper _helper;
        private HomeView _homeView;

        public HomeController(IOptions<AppSettings> optionsAccessor, ILogger<HomeController> logger, IIoTHubDpsHelper helper)
        {
            _logger = logger;
            _appSettings = optionsAccessor.Value;
            _helper = helper;
            _logger.LogInformation("HomeController");
            _homeView = new HomeView();
            _helper.SetHomeView(_homeView);
            //            ViewData["IoTHubName"] = _helper.GetIoTHubName(_appSettings.IoTHub.ConnectionString);
        }

        public async Task<IActionResult> Index()
        {
            HomeView homeView = _homeView;
            homeView.deviceList = await _helper.GetIoTHubDevices();
            homeView.enrollmentList = await _helper.GetDpsEnrollments();
            homeView.groupEnrollmentList = await _helper.GetDpsGroupEnrollments();
            ViewData["IoTHubName"] = _helper.GetIoTHubName(_appSettings.IoTHub.ConnectionString);
            ViewData["mapKey"] = _appSettings.AzureMap.MapKey.ToString();
            ViewData["TsiClientId"] = _appSettings.TimeSeriesInsights.clientId.ToString();
            ViewData["TsiTenantId"] = _appSettings.TimeSeriesInsights.tenantId.ToString();
            ViewData["TsiUri"] = _appSettings.TimeSeriesInsights.tsiUri.ToString();
            ViewData["TsiSecret"] = _appSettings.TimeSeriesInsights.tsiSecret.ToString();
            ViewData["DpsIdScope"] = _appSettings.Dps.IdScope.ToString();
            ViewData["tilesetId"] = _appSettings.AzureMap.TilesetId;
            ViewData["statesetId"] = _appSettings.AzureMap.StatesetId;
            ViewBag.EnrollmentList = await _helper.GetDpsEnrollments2();
            ViewBag.DeviceList = await _helper.GetIoTHubDevices();
            return View(homeView);
        }

        public async Task<ActionResult> RefreshIoTHubDevices2()
        {
            _homeView.deviceList = await _helper.GetIoTHubDevices();

            return PartialView("deviceListPartial", _homeView.deviceList);
        }

        [HttpGet]
        public async Task<ActionResult> RefreshIoTHubDevices()
        {
            _homeView.deviceList = await _helper.GetIoTHubDevices();

            return PartialView("deviceListPartial", _homeView.deviceList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
