namespace BlackjackAPI.DomainPrimitives
{
    /// <summary>
    /// This class represents the quantity of a card in a standard deck of playing cards.
    /// </summary>
    public class CardQuantity
    {
        private int Value;

        public CardQuantity(int value)
        {
            // Validation logic for the quantity
            IsQuantityNegative(value);
            IsQuantityTooLarge(value);
            Value = value;
        }

        public int GetValue()
        {
            return Value;
        }

        private void IsQuantityNegative(int value)
        {
            if (value < 1)
                throw new ArgumentException("Quantity cannot be less than 1.");
        }

        private void IsQuantityTooLarge(int value)
        {
            if (value > 13)
                throw new ArgumentException("Quantity cannot be larger than 13.");
        }
    }
}
