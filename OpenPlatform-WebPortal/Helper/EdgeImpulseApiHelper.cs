using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using OpenPlatform_WebPortal.Models;
using Newtonsoft.Json;

namespace OpenPlatform_WebPortal.Helper
{
    public class DeploymentInfo
    {
        public bool hasDeployment { get; set; }
    }

    public class EdgeImpulseApiHelper
    {
        private static string _apiKey = string.Empty;
        private static ILogger _logger = null;

        List<FirmwareOptionViewModel> ALL_FIRMWARES = new List<FirmwareOptionViewModel>()
        {
            new FirmwareOptionViewModel{ OptionName = "Nordic NRF52840 DK", OptionKey = "nordic-nrf52840-dk"},
            new FirmwareOptionViewModel{ OptionName = "Nordic NRF5340 DK", OptionKey = "nordic-nrf5340-dk"},
            new FirmwareOptionViewModel{ OptionName = "C++ library", OptionKey = "zip"}
        };

        public EdgeImpulseApiHelper()
        {

        }

        public EdgeImpulseApiHelper(string apiKey, ILogger logger)
        {
            _apiKey = apiKey;
            _logger = logger;
        }

        public async Task<string> GetProjects(string path)
        {
            HttpClient httpClient = new HttpClient();
            var jsonModel = string.Empty;
            try
            {
                var fullPath = new Uri($"{path}");
                if (!string.IsNullOrEmpty(_apiKey))
                {
                    httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                }
                jsonModel = await httpClient.GetStringAsync(fullPath);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error GetProjects(): {e.Message}");
                return null;
            }
            return jsonModel;
        }

        public async Task<string> GetBuiltModels(string path)
        {
            HttpClient httpClient = new HttpClient();
            var jsonModel = string.Empty;
            try
            {
                var fullPath = new Uri($"{path}");
                if (!string.IsNullOrEmpty(_apiKey))
                {
                    httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                }
                jsonModel = await httpClient.GetStringAsync(fullPath);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error GetBuiltModels(): {e.Message}");
            }
            return jsonModel;
        }

        public async Task<HttpResponseMessage> GetModelBinary(string path)
        {
            HttpClient httpClient = new HttpClient();

            try
            {
                var fullPath = new Uri($"{path}");
                if (!string.IsNullOrEmpty(_apiKey))
                {
                    httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/zip");
                }

                HttpResponseMessage response = await httpClient.GetAsync(fullPath);
                
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error GetBuiltModels(): {e.Message}");
            }
            return null;
        }

        /**********************************************************************************
         * Get list of firmware builds from Edge Impulse
         *********************************************************************************/

        public async Task<EiFirmwareListViewModel> GetEiFirmwareList(string projectId)
        {
            // prepare  
            EdgeImpulseApiHelper resolver = new EdgeImpulseApiHelper(_apiKey, _logger);
            var eiFirmwareListModel = new EiFirmwareListViewModel();
            List<FirmwareOptionViewModel> Options = new List<FirmwareOptionViewModel>();
            string selected = "";

            foreach (var firmware in ALL_FIRMWARES)
            {
                try
                {
                    if (_apiKey != null)
                    {
                        string path = $"https://studio.edgeimpulse.com/v1/api/{projectId}/deployment?type={firmware.OptionKey}";
                        var modelData = await GetBuiltModels(path);

                        var deploymentInfo = JsonConvert.DeserializeObject<DeploymentInfo>(modelData);

                        if (deploymentInfo.hasDeployment)
                        {
                            Options.Add(firmware);
                            selected = firmware.OptionName;
                        }

                        _logger.LogInformation($"RefreshFirmwareOptions: Got model info from project: {_apiKey}");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError($"Exception in GetEIBuilds() : {e.Message}");
                }
            }

            eiFirmwareListModel.Options = Options;
            eiFirmwareListModel.SelectedOption = selected;
            return eiFirmwareListModel;
        }
    }
}
