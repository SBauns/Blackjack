public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public Hand Hand { get; set; }
        public bool HasWon { get; set; }
        public bool IsStanding { get; set; }
    }