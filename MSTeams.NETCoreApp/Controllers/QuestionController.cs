using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System;
using MSTeams.NETCoreApp.Models;

namespace MSTeams.NETCoreApp.Controllers
{
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly QuestionsContext questionsContext;

        public QuestionController(QuestionsContext questionsContext)
        {
            this.questionsContext = questionsContext;
        }

        [HttpGet]
        [Route("api/questions")]
        public async Task<IActionResult> GetQuestions()
        {
            var questionList = await questionsContext.GetQuestions();
            return Ok(questionList);
        }

        [HttpPost]
        [Route("api/help/{id}")]
        public async Task<HttpResponseMessage> SendQuestion(string id, [FromBody] Question question)
        {
            var newQuestionId = await questionsContext.AddQuestion(question.Comment, id);
            var jsonMessage = System.IO.File.ReadAllText(Path.Combine("Models\\question_message.json"));
            jsonMessage = jsonMessage.Replace("{{questionText}}", question.Comment);
            jsonMessage = jsonMessage.Replace("{{questionId}}", newQuestionId.ToString());
            jsonMessage = jsonMessage.Replace("{{answerId}}", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString()); // cache buster to force API call
            var incomingWebHookUrl = "https://microsoft.webhook.office.com/webhookb2/GUID@GUID/IncomingWebhook/GUID/GUID";

            return await httpClient.PostAsync(incomingWebHookUrl, new StringContent(jsonMessage));
        }

        [HttpPost]
        [Route("api/answer/{questionId}")]
        public async Task<IActionResult> PostAnswerAsync(Guid questionId, [FromBody] Answer answer)
        {
            try
            {
                await questionsContext.AnswerQuestion(answer.Comment, answer.UserName, questionId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}
