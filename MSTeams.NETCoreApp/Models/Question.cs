using System;
using System.Collections.Generic;

namespace MSTeams.NETCoreApp.Models
{
    public class Question
    {
        public Guid QuestionId { get; set; }
        public string DocumentId { get; set; }
        public string Comment { get; set; }
        public Answer Answer { get; set; }
    }
}
