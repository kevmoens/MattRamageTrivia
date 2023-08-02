namespace MattRamageTrivia.Models
{
    public class Contestant
    {
        public required string Name { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Score { get; set; } = 0;
    }
}
