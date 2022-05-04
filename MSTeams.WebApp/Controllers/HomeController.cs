using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MSTeams.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MSTeams.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private HttpClient httpClient = new HttpClient();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Question question)
        {
            var questionId = "127c6a3c-5f17-435f-a1b9-c8e334e86cd2";
            var apiUrl = $"http://localhost:3978/api/help/{questionId}";

            await httpClient.PostAsync(apiUrl, JsonContent.Create(new { Comment = question.Comment }));
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
