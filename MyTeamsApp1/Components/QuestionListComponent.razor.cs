using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using MyTeamsApp1.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MyTeamsApp1.Components
{
    public partial class QuestionListComponent : ComponentBase
    {
        [Parameter]
        public string UserDisplayName { get; set; }

        private readonly HttpClient httpClient = new();
        private Question SelectedRow = null;

        public List<Question> Questions { get; set; } = new List<Question>();
        public List<ColumnDefinition<Question>> ColumnDefinitions = new();

        protected override async Task OnInitializedAsync()
        {
            await RefreshQuestions();
            ColumnDefinitions.Add(new ColumnDefinition<Question>("Question", q => q.Comment));
            ColumnDefinitions.Add(new ColumnDefinition<Question>("Answer", q => q.AnswerStatus));
            ColumnDefinitions.Add(new ColumnDefinition<Question>("DocumentUrl", q => q.DocumentUrl));
        }

        private async Task RefreshQuestions()
        {
            Questions = await GetQuestions();
        }

        private async Task<List<Question>> GetQuestions()
        {
            //var url = "https://7bc9-2601-283-4101-2ce0-fd73-3d2-fd2c-b179.ngrok.io/api/questions";
            var url = "http://localhost:3978/api/questions";

            using (var response = await httpClient.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Question>>(content);
            }
        }

        private void SelectQuestion(Question question)
        {
            if (question.Answer == null)
            {
                question.Answer = new Answer()
                {
                    UserName = UserDisplayName
                };
            }
            SelectedRow = question;
        }

        private async Task OnSaveAnswer()
        {
            //var url = $"https://7bc9-2601-283-4101-2ce0-fd73-3d2-fd2c-b179.ngrok.io/api/answer/{SelectedRow.QuestionId}";
            var url = $"http://localhost:3978/api/answer/{SelectedRow.QuestionId}";

            using var response = await httpClient.PostAsync(url, JsonContent.Create(SelectedRow.Answer));
            response.EnsureSuccessStatusCode();
            await RefreshQuestions();
        }
    }
}
