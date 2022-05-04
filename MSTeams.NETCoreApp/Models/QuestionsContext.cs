using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSTeams.NETCoreApp.Models
{
    public class QuestionsContext : DbContext
    {
        public DbSet<Question> QuestionList { get; set; }

        public string DbPath { get; }

        public QuestionsContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "questions.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        public async Task<Guid> AddQuestion(string comment, string documentId)
        {
            var addedQuestion = new Question()
            {
                QuestionId = new Guid(),
                DocumentId = documentId,
                Comment = comment,
                Answer = null
            };
            QuestionList.Add(addedQuestion);
            await SaveChangesAsync();
            return addedQuestion.QuestionId;
        }

        public async Task<List<Question>> GetQuestions()
        {
            return await QuestionList.Include(q => q.Answer).ToListAsync();
        }

        public Question GetQuestionById(Guid questionId)
        {
            return QuestionList.Include(q => q.Answer).FirstOrDefault(q => q.QuestionId == questionId);
        }

        public async Task AnswerQuestion(string comment, string userName, Guid questionId)
        {
            var question = QuestionList.Find(questionId);
            if (question != null)
            {
                var answer = new Answer()
                {
                    AnswerId = new Guid(),
                    UserName = userName,
                    Comment = comment
                };
                question.Answer = answer;
                Add(answer);
                await SaveChangesAsync();
            }
        }
    }
}
