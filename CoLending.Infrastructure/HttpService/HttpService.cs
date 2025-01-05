using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CoLending.Infrastructure.HttpService
{
    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<JsonDocument> GetAsync(string uri, Dictionary<string, string> Headers)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            foreach (var header in Headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<JsonDocument>();
        }

        public async Task<JsonDocument> PostAsync<TIn>(string uri, TIn model, Dictionary<string, string> Headers)
        {

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            foreach (var header in Headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            HttpResponseMessage response = await httpClient.PostAsync(uri, new StringContent(JsonSerializer.Serialize(model), UnicodeEncoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<JsonDocument>() ?? throw new ArgumentNullException();
        }

        public async Task<JsonDocument> PostAsyncToView(string uri, Dictionary<string, string> Headers)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            foreach (var header in Headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            HttpResponseMessage response = await httpClient.PostAsync(uri, null);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<JsonDocument>() ?? throw new ArgumentNullException();
        }
    }
}
