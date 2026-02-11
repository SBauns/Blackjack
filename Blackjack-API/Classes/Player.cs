using BlackjackAPI.DomainPrimitives;

namespace BlackjackAPI.Classes
{
    public class PlayerDataTransferObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public HandDataTransferObject Hand { get; set; }
        public bool HasWon { get; set; }
        public bool IsStanding { get; set; }
    }
    public class Player
    {
        //PROPERTIES
        public Guid Id { get; private set; }
        public Name Name { get; private set; }
        public Quantity Credits { get; private set; }

        //RELATIONSHIPS
        public Dealer Dealer { get; private set; }
        public Hand Hand { get; private set; }

        public bool IsStanding { get; private set; } = false;
        public bool HasWon { get; set; } = false;
        public Player(Name name, Quantity credits, Dealer dealer)
        {
            Id = Guid.NewGuid();
            Name = name;
            Credits = credits;
            Dealer = dealer;
            Hand = new Hand();
        }

        public void Hit()
        {
            Card card = Dealer.Deal();
            Hand.ReceiveCard(card);
            if (Hand.IsBusted())
            {
                IsStanding = true;
            }
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
                Id = Id,
                Name = Name.GetValue(),
                Credits = Credits.GetValue(),
                Hand = Hand.ToDataTransferObject(),
                IsStanding = IsStanding,
                HasWon = HasWon
            };
        }
    }
}
