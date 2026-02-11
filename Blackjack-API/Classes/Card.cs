using BlackjackAPI.DomainPrimitives;

namespace BlackjackAPI.Classes
{
    public class CardDataTransferObject
    {
        public int Value { get; set; }
        public string Suit { get; set; }
        public bool IsFaceDown { get; set; }
    }
    public class Card
    {
        //PROPERTIES
        private CardQuantity Value;
        private Suit Suit;

        private bool isFaceDown { get; set; } = false;

        public Card(CardQuantity value, Suit suit)
        {
            Value = value;
            Suit = suit;
        }

        public void FlipCard()
        {
            isFaceDown = !isFaceDown;
        }

        public bool IsFaceDown()
        {
            return isFaceDown;
        }

        public int GetValue()
        {
            return Value.GetValue();
        }

        public string GetSuit()
        {
            return Suit.ToString();
        }

        public CardDataTransferObject ToDataTransferObject()
        {
            return new CardDataTransferObject
            {
                Value = GetValue(),
                Suit = GetSuit(),
                IsFaceDown = isFaceDown
            };
        }
    }
}
