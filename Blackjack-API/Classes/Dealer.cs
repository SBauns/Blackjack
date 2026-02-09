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

        public Dealer(Name name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Deck = new Deck();
            Players = new List<Player>();
            Hand = new Hand();
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

        public List<Player> EvaluateVictory()
        {
            List<Player> winningPlayers = new List<Player>();
            //Build own hand
            while (!Hand.IsBusted() && Hand.GetScore() < 17) //TODO Set this somewhere else or magic number
            {
                Hand.ReceiveCard(Deal());
            }

            //Compare to players
            foreach (Player player in Players)
            {
                bool playerBeatsDealer = player.Hand.GetScore() > Hand.GetScore();
                bool playerNotBusted = !player.Hand.IsBusted();
                bool dealerBusted = Hand.IsBusted();

                
                if ((playerBeatsDealer && playerNotBusted) || (dealerBusted && playerNotBusted))
                {
                    winningPlayers.Add(player);
                }
                // else if (player.Hand.Score.GetValue() <= Hand.Score.GetValue() && !Hand.Isbusted)
                // {
                //     //Dealer wins
                // }
            }

            return winningPlayers;
        }

        public DealerDataTransferObject ToDataTransferObject()
        {
            return new DealerDataTransferObject
            {
                Id = this.Id,
                Name = this.Name.GetValue(),
                Players = this.Players.Select(p => new PlayerDataTransferObject
                {
                    Name = p.Name.GetValue(),
                    Credits = p.Credits.GetValue(),
                    Hand = new HandDataTransferObject
                    {
                        Score = p.Hand.GetScore(),
                        IsBusted = p.Hand.IsBusted(),
                        Cards = p.Hand.Cards.Select(c => new CardDataTransferObject
                        {
                            Value = c.GetValue(),
                            Suit = c.GetSuit()
                        }).ToList()
                    },
                    IsStanding = p.IsStanding
                }).ToList(),
                Hand = new HandDataTransferObject
                {
                    Score = this.Hand.GetScore(),
                    IsBusted = this.Hand.IsBusted(),
                    Cards = this.Hand.Cards.Select(c => new CardDataTransferObject
                    {
                        Value = c.GetValue(),
                        Suit = c.GetSuit()
                    }).ToList()
                }
            };
        }
    }
}
