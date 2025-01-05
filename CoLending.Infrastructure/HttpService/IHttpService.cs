using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoLending.Infrastructure.HttpService
{
    public interface IHttpService
    {
        Task<JsonDocument> GetAsync(string uri, Dictionary<string, string> Headers);

        Task<JsonDocument> PostAsync<TIn>(string uri, TIn model, Dictionary<string, string> Headers);

        Task<JsonDocument> PostAsyncToView(string uri, Dictionary<string, string> Headers);

    }
}
