using System;
using System.Collections.Generic;

namespace MyTeamsApp1.Models
{
    public class Question
    {
        public Guid QuestionId { get; set; }
        public string DocumentId { get; set; }
        public string Comment { get; set; }
        public Answer Answer { get; set; }
        public string AnswerStatus { get => Answer != null && !string.IsNullOrEmpty(Answer.Comment) ? "Answered" : "Open"; }
        public string DocumentUrl { get => $"https://github.com/laurieatkinson/ms-teams-demo/blob/master/SampleDocument.docx?sourcedoc={DocumentId}"; }
    }
}
