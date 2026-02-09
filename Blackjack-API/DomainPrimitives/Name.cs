namespace BlackjackAPI.DomainPrimitives
{
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
