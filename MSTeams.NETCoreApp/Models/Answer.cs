using System;

namespace MSTeams.NETCoreApp.Models
{
    public class Answer
    {
        public Guid AnswerId { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
    }
}
