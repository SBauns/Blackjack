namespace BlackjackAPI.DomainPrimitives
{
    public class Quantity
    {
        private int Value;

        public Quantity(int value)
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

        public void addToValue(int value)
        {
            Value += value;
        }

        public void substractFromValue(int value)
        {
            Value -= value;
        }

        private void IsQuantityNegative(int value)
        {
            if (value < 0)
                throw new ArgumentException("Quantity cannot be negative.");
        }

        private void IsQuantityTooLarge(int value)
        {
            if (value > 1000000)
                throw new ArgumentException("Quantity cannot be larger than 1000000.");
        }
    }
}
