using Microsoft.AspNetCore.Mvc;
using MSTeams.NETCoreApp.Models;
using System;
using System.IO;

namespace MSTeams.NETCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly QuestionsContext questionsContext;

        public ImageController(QuestionsContext questionsContext)
        {
            this.questionsContext = questionsContext;
        }

        [HttpGet]
        [Route("{questionId}")]
        public IActionResult Get(Guid questionId)
        {
            // If question has an answer, then show answered icon
            var fileName = "open.png";
            var question =  questionsContext.GetQuestionById(questionId);
            if (question != null && question.Answer != null)
            {
                fileName = "answered.png";
            }
            var image = System.IO.File.OpenRead(Path.Combine(fileName));
            return File(image, "image/png");
        }
    }
}
