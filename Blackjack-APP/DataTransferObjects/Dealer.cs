public class Dealer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Player> Players { get; set; }
        public Hand Hand { get; set; }
    }