using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FinalProject_SolarSystemEducationApp.Models
{
    public class SolarDAL
    {
        public HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.le-systeme-solaire.net/rest/");

            return client;
        }

        public async Task<List<Body>> GetBody()
        {
            var client = GetHttpClient();
            var response = await client.GetAsync("bodies/");
            var solar = await response.Content.ReadAsAsync<Bodies>();
            return solar.bodies;
        }
    }
}
