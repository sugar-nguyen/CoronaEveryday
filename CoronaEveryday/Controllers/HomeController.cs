using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoronaEveryday.Models;
using Microsoft.Extensions.Configuration;
using CoronaEveryday.Data;

namespace CoronaEveryday.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClientRequest _client;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, HttpClientRequest client)
        {
            _logger = logger;
            _configuration = configuration;
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _client.GetSummaryAsync();
            if (model.Global != null)
            {
                var accessDb = new AccessDb(_configuration.GetConnectionString("CovidDb"));
                accessDb.InsertData(model);
                return View(model);

            }

            return Content("Api death.");
        }

        [HttpGet]
        public JsonResult GetGlobal()
        {
            var accessDb = new AccessDb(_configuration.GetConnectionString("CovidDb"));
            var model = accessDb.GetAllGlobal().OrderBy(x=>x.Date);
            return Json(model);
        }

        [HttpGet]
        public JsonResult GetJsonCountry()
        {
           
            var accessDb = new AccessDb(_configuration.GetConnectionString("CovidDb"));
            var model = accessDb.GetAllCountriesByDate(DateTime.Now);

            return Json(model);
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
