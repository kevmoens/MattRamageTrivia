using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace MattRamageTrivia.Models.Data
{
    public class TriviaRepository : DbContext
    {
        public TriviaRepository(DbContextOptions<TriviaRepository> options)
            : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string path = @"Trivia.sqlite";
            optionsBuilder.UseSqlite(
                $"data source={path}");
        }
    }
}
