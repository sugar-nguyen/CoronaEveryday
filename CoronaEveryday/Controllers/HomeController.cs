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
            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetJsonCountry()
        {
            var model = await _client.GetSummaryAsync();
            return Json(model.Countries.OrderByDescending(x=>x.NewConfirmed));
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
