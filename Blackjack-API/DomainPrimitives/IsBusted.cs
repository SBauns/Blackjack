namespace BlackjackAPI.DomainPrimitives
{
    public class IsBusted
    {
        private bool Value;

        public IsBusted(bool value)
        {
            Value = value;
        }

        public bool GetValue()
        {
            return Value;
        }

        public void SetValue(bool value)
        {
            Value = value;
        }
    }
}
