using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace OpenPlatform_WebPortal.Helper
{
    public class EdgeImpulseApiHelper
    {
        private static string _apiKey = string.Empty;
        private static HttpClient _httpClient = new HttpClient();
        private static ILogger _logger = null;

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
                _logger.LogError($"Error GetModelContentAsync(): {e.Message}");
            }
            return jsonModel;
        }
    }
}
