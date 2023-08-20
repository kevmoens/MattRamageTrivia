using Microsoft.EntityFrameworkCore;

namespace MattRamageTrivia.Models.Data
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public byte[]? Image { get; set; }
        public int Difficulty { get; set; } = 0;

        public virtual ICollection<QuestionOption> Options { get; set; } = new List<QuestionOption>();
    }
}
