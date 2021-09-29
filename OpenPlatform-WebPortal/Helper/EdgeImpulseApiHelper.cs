using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenPlatform_WebPortal.Models;

namespace OpenPlatform_WebPortal.Helper
{
    public class EdgeImpulseApiHelper
    {
        private static string _apiKey = string.Empty;
        private static HttpClient _httpClient = new HttpClient();
        private static ILogger _logger = null;

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
            var jsonModel = string.Empty;
            try
            {
                var fullPath = new Uri($"{path}");
                if (!string.IsNullOrEmpty(_apiKey))
                {
                    //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("x-api-key", _apiKey);
                    _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
                }
                jsonModel = await _httpClient.GetStringAsync(fullPath);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error GetProjects(): {e.Message}");
            }
            return jsonModel;
        }

        public async Task<string> GetBuiltModels(string path)
        {
            var jsonModel = string.Empty;
            try
            {
                var fullPath = new Uri($"{path}");
                if (!string.IsNullOrEmpty(_apiKey))
                {
                    _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                }
                jsonModel = await _httpClient.GetStringAsync(fullPath);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error GetBuiltModels(): {e.Message}");
            }
            return jsonModel;
        }

        public IActionResult GetModelBinary(string path)
        {
            string ZIP_PATH = "bin\\Debug\\netcoreapp3.1\\p15-elephant-audio-nrf52840-dk-v1.zip"; //  $"{filename}.zip";
            string filename = "download-model";

            const string contentType = "application/zip";
            // HttpContext.Response.ContentType = contentType;
            var result = new FileContentResult(System.IO.File.ReadAllBytes(ZIP_PATH), contentType)
            {
                FileDownloadName = $"{filename}.zip"
            };

            return result;

            //using (HttpResponseMessage response = await _httpClient.GetAsync(path))
            //{
            //    using (var stream = await response.Content.ReadAsStreamAsync())
            //    {
            //        //return stream;

            //        // string filename = "myzip";
            //        string ZIP_PATH = "Elephant-voices-project-dataset-20210912T193510Z-001.zip"; //  $"{filename}.zip";

            //        using (Stream zip = File.OpenWrite(ZIP_PATH))
            //        {
            //            stream.CopyTo(zip);

            //            const string contentType = "application/zip";
            //            // HttpContext.Response.ContentType = contentType;
            //            var result = new FileContentResult(System.IO.Stream., contentType)
            //            {
            //                FileDownloadName = ZIP_PATH //  $"{filename}.zip"
            //            };

            //            return result;
            //        }
            //    }
            //}


            //using (System.Net.WebClient wc = new System.Net.WebClient())
            //{
            //    wc.DownloadFile(path, @"C:\Downloads\modelzip.zip");
            //}

            ////var jsonModel = string.Empty;
            ////Stream model = 
            //try
            //{
            //    var fullPath = new Uri($"{path}");
            //    if (!string.IsNullOrEmpty(_apiKey))
            //    {
            //        _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
            //        _httpClient.DefaultRequestHeaders.Add("Accept", "application/zip");
            //    }
            //    jsonModel = await _httpClient.GetStreamAsync(); // .GetStringAsync(fullPath);
            //}
            //catch (Exception e)
            //{
            //    _logger.LogError($"Error GetBuiltModels(): {e.Message}");
            //}
            //return jsonModel;
        }

        /**********************************************************************************
         * Get list of devices from IoT Hub
         *********************************************************************************/
        public EiFirmwareListViewModel GetEiFirmwareList()
        {
            var iothubDeviceListViewModel = new EiFirmwareListViewModel();

            iothubDeviceListViewModel.Options.Add(new FirmwareOptionViewModel
            {
                OptionName = "Nordic NRF52840 DK",
                OptionKey = "nordic-nrf52840-dk"
            });

            iothubDeviceListViewModel.Options.Add(new FirmwareOptionViewModel
            {
                OptionName = "Nordic NRF5340 DK",
                OptionKey = "nordic-nrf5340-dk"
            });

            iothubDeviceListViewModel.Options.Add(new FirmwareOptionViewModel
            {
                OptionName = "C++ library",
                OptionKey = "zip"
            });

            iothubDeviceListViewModel.SelectedOption = "Nordic NRF52840 DK";

            return iothubDeviceListViewModel;
        }
    }
}
