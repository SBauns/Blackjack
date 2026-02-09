namespace BlackjackAPI.DomainPrimitives
{
    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    public class CardSuit
    {
        private Suit Value;

        public CardSuit(Suit value)
        {
            // Validation logic for the suit
            IsSuitInvalid(value);
            Value = value;
        }

        private void IsSuitInvalid(Suit value)
        {
            if (!Enum.IsDefined(typeof(Suit), value))
                throw new ArgumentException("Invalid suit value.");
        }

        public Suit GetValue()
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
