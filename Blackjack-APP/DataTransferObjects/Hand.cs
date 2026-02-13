public class Hand
    {
        public int Score { get; set; }
        public bool IsBusted { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>();
    }