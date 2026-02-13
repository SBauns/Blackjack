public class Player
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Credits { get; set; }
        public Hand Hand { get; set; } = new Hand();
        public bool HasWon { get; set; }
        public bool IsStanding { get; set; }
    }