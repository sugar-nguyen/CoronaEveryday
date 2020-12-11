using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CoronaEveryday.Models;
using System.Text.Json;

namespace CoronaEveryday.Data
{
    public class HttpClientRequest
    {
        private readonly IConfiguration _configuration;
        public HttpClient Client { get; set; }

        public HttpClientRequest(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;

            httpClient.BaseAddress = new Uri(_configuration["ApiUrl"]);
            Client = httpClient;
        }

        public async Task<CoronaRecieve> GetSummaryAsync()
        {
            try
            {

                var response = Client.GetAsync("/summary").Result;
                var json = response.Content.ReadAsStreamAsync().Result;

                return await JsonSerializer.DeserializeAsync<CoronaRecieve>(json);

            }
            catch
            {
                return new CoronaRecieve();
            }
        }
        public async Task<CoronaRecieve> GetAll()
        {
            try
            {

                var response = Client.GetAsync("/all").Result;
                var json =  response.Content.ReadAsStreamAsync().Result;

                return await JsonSerializer.DeserializeAsync<CoronaRecieve>(json);

            }
            catch
            {
                return new CoronaRecieve();
            }
        }

    }
}
