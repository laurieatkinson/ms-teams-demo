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
            var incomingWebHookUrl = "https://microsoft.webhook.office.com/webhookb2/a6533262-14e2-4c3d-8533-8e94cdff932a@72f988bf-86f1-41af-91ab-2d7cd011db47/IncomingWebhook/49494659edd94d3593ef065b5a19918d/fe585082-d9ce-48c0-ac02-fd19ceb50db6";

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
