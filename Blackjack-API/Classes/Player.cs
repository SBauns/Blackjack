using BlackjackAPI.DomainPrimitives;

namespace BlackjackAPI.Classes
{
    public class PlayerDataTransferObject
    {
        public string Name { get; set; }
        public int Credits { get; set; }
        public HandDataTransferObject Hand { get; set; }
        public bool IsStanding { get; set; }
    }
    public class Player
    {
        //PROPERTIES
        public Name Name { get; private set; }
        public Quantity Credits { get; private set; }

        //RELATIONSHIPS
        public Dealer Dealer { get; private set; }
        public Hand Hand { get; private set; }

        public bool IsStanding { get; private set; } = false;
        public Player(Name name, Quantity credits, Dealer dealer)
        {
            Name = name;
            Credits = credits;
            Dealer = dealer;
            Hand = new Hand();
        }

        public void Hit()
        {
            Card card = Dealer.Deal();
            Hand.ReceiveCard(card);
        }

        public void Reset()
        {
            foreach (Card card in Hand.ReturnCards())
            {
                Dealer.Discard(card);
            }
            IsStanding = false;
        }

        public void Stand()
        {
            IsStanding = true;
        }

        public PlayerDataTransferObject ToDataTransferObject()
        {
            return new PlayerDataTransferObject
            {
                Name = Name.ToString(),
                Credits = Credits.GetValue(),
                Hand = new HandDataTransferObject
                {
                    Score = Hand.GetScore(),
                    IsBusted = Hand.IsBusted(),
                    Cards = Hand.Cards.Select(card => new CardDataTransferObject
                    {
                        Value = card.GetValue(),
                        Suit = card.GetSuit()
                    }).ToList()
                },
                IsStanding = IsStanding
            };
        }
    }
}
