namespace BlackjackAPI.DomainPrimitives
{
    /// <summary>
    /// This class represents a player's name in the Blackjack game. 
    /// It includes validation logic to ensure that the name is not empty, 
    /// not too long, not too short, and does not contain invalid characters. 
    /// The name can only contain letters and spaces, and must be between 2 and 50 characters in length.
    /// </summary>
    public class Name
    {
        private string Value;

        public Name(string value)
        {
            // Validation logic for the name
            IsStringEmpty(value);
            IsStringTooLong(value);
            IsStringTooShort(value);
            DoesStringContainInvalidCharacters(value);
            Value = value;
        }

        public string GetValue()
        {
            return Value;
        }

        private void IsStringEmpty(string value)
        {
             if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be empty.");
        }

        private void IsStringTooLong(string value)
        {
            if (value.Length > 50)
                throw new ArgumentException("Name cannot be longer than 50 characters.");
        }

        private void IsStringTooShort(string value)
        {
            if (value.Length < 2)
                throw new ArgumentException("Name cannot be shorter than 2 characters.");
        }

        private void DoesStringContainInvalidCharacters(string value)
        {
            if (value.Any(c => !char.IsLetter(c) && c != ' '))
                throw new ArgumentException("Name can only contain letters and spaces.");
        }
    }
}
