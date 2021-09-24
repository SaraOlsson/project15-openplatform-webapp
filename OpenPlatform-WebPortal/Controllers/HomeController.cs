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
            EdgeImpulseApiHelper _eihelper = new EdgeImpulseApiHelper();

            HomeView homeView = _homeView;
            ViewData["IoTHubName"] = _helper.GetIoTHubName(_appSettings.IoTHub.ConnectionString);
            ViewData["mapKey"] = _appSettings.AzureMap.MapKey.ToString();
            ViewData["TsiClientId"] = _appSettings.TimeSeriesInsights.clientId.ToString();
            ViewData["TsiTenantId"] = _appSettings.TimeSeriesInsights.tenantId.ToString();
            ViewData["TsiUri"] = _appSettings.TimeSeriesInsights.tsiUri.ToString();
            ViewData["TsiSecret"] = _appSettings.TimeSeriesInsights.tsiSecret.ToString();
            ViewData["DpsIdScope"] = _appSettings.Dps.IdScope.ToString();
            ViewData["tilesetId"] = _appSettings.AzureMap.TilesetId;
            ViewData["statesetId"] = _appSettings.AzureMap.StatesetId;
            ViewData["EiProjectId"] = 0; //40654; // TODO SARA (maybe not as app setting)
            ViewBag.DpsEnrollmentList = await _helper.RefreshDpsEnrollments();
            ViewBag.DpsGroupEnrollmentList = await _helper.RefreshDpsGroupEnrollments();
            ViewBag.IoTHubDeviceList = await _helper.GetIoTHubDevices();
            ViewBag.EiFirmwareList = _eihelper.GetEiFirmwareList();
            return View(homeView);
        }

        //[HttpGet]
        //public async Task<ActionResult> RefreshIoTHubDevices()
        //{
        //    var deviceList = await _helper.GetIoTHubDevices();
        //    ViewBag.IoTHubDeviceList = deviceList;
        //    return PartialView("IoTHubDeviceListViewModel");
        //}

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
