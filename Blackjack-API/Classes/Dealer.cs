using BlackjackAPI.DomainPrimitives;

namespace BlackjackAPI.Classes
{
    public class DealerDataTransferObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<PlayerDataTransferObject> Players { get; set; }
        public HandDataTransferObject Hand { get; set; }
    }
    public class Dealer
    {
        //PROPERTIES
        public Guid Id { get; private set; }

        private Name Name;

        //RELATIONSHIPS
        private Deck Deck;

        private List<Player> Players;

        public Hand Hand;

        public Dealer()
        {
            Id = Guid.NewGuid();
            Name = new Name(RandomDealerName());
            Deck = new Deck();
            Players = new List<Player>();
            Hand = new Hand();
        }

        public void SetupGame()
        {
            foreach (Player player in Players)
            {
                player.Reset();
            }
            foreach (Card card in Hand.ReturnCards())
            {
                Discard(card);    
            }
            Hand.ResetScore();
            Deck.Shuffle();

            foreach (Player player in Players)
            {
                player.Hit();
            }

            Card firstCard = Deck.GetTopCard();
            firstCard.FlipDown();
            Hand.ReceiveCard(firstCard);

            Card secondCard = Deck.GetTopCard();
            secondCard.FlipUp();
            Hand.ReceiveCard(secondCard);
        }

        public void PlayerJoin(Player player)
        {
            Players.Add(player);
        }

        public void PlayerLeave(Player player)
        {
            Players.Remove(player);
        }

        public List<Player> GetPlayers()
        {
            return Players;
        }

        public Card Deal()
        {
            return Deck.GetTopCard();
        }

        public void Discard(Card card)
        {
            Deck.PutInDiscard(card);
        }

        public bool IsGameOver()
        {
            return Players.All(p => p.IsStanding);
        }

        public void EvaluateVictory()
        {
            Hand.FlipUpAllCards();
            //Build own hand
            if (!Players.All(p => p.Hand.IsBusted()))
            {
                while (!Hand.IsBusted() && Hand.GetScore() < 17 && Players.Any(p => p.Hand.GetScore() > Hand.GetScore())) //TODO Set this somewhere else or magic number
                {
                    Card card = Deal();
                    card.FlipUp();
                    Hand.ReceiveCard(card);
                }      
            }

            //Compare to players
            foreach (Player player in Players)
            {
                bool playerBeatsDealer = player.Hand.GetScore() > Hand.GetScore();
                bool playerNotBusted = !player.Hand.IsBusted();
                bool dealerBusted = Hand.IsBusted();

                
                if ((playerBeatsDealer && playerNotBusted) || (dealerBusted && playerNotBusted))
                {
                    player.HasWon = true;
                }
                // else if (player.Hand.Score.GetValue() <= Hand.Score.GetValue() && !Hand.Isbusted)
                // {
                //     //Dealer wins
                // }
            }
        }

        private string RandomDealerName()
        {
            string[] names = { "Alice", "Bob", "Charlie", "Diana", "Eve" };
            Random rand = new Random();
            int index = rand.Next(names.Length);
            return names[index];
        }

        public DealerDataTransferObject ToDataTransferObject()
        {
            return new DealerDataTransferObject
            {
                Id = this.Id,
                Name = this.Name.GetValue(),
                Players = this.Players.Select(player => player.ToDataTransferObject()).ToList(),
                Hand = this.Hand.ToDataTransferObject()
            };
        }
    }
}
