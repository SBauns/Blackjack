using BlackjackAPI.DomainPrimitives;

namespace BlackjackAPI.Classes
{
    public class HandDataTransferObject
    {
        public int Score { get; set; }
        public bool IsBusted { get; set; }
        public List<CardDataTransferObject> Cards { get; set; }
    }
    public class Hand
    {
        //PROPERTIES
        private Quantity Score;

        public IsBusted Isbusted { get; private set; }

        //RELATIONSHIPS
        public List<Card> Cards { get; private set; }

        public Hand()
        {
            Score = new Quantity(0);
            Isbusted = new IsBusted(false);
            Cards = new List<Card>();
        }

        public void ReceiveCard(Card card)
        {
            Cards.Add(card);
            Score.addToValue(card.GetValue());
            IsBusted();
        }

        public List<Card> ReturnCards()
        {
            List<Card> returnedCards = new List<Card>();
            foreach (Card card in Cards)
            {
                returnedCards.Add(card);
                Score.substractFromValue(card.GetValue());
            }
            Cards.Clear();
            return returnedCards;
        }

        public int GetScore()
        {
            return Score.GetValue();
        }

        public bool IsBusted()
        {
            int totalValue = 0;
            foreach (Card card in Cards)
            {
                totalValue += card.GetValue();
            }
            Score = new Quantity(totalValue);
            if (totalValue > 21) //TODO Set this somewhere else or magic number
            {
                Isbusted.SetValue(true);
            }
            return Isbusted.GetValue();
        }
        public HandDataTransferObject ToDataTransferObject()
        {
            HandDataTransferObject dto = new HandDataTransferObject();
            dto.Score = GetScore();
            dto.IsBusted = Isbusted.GetValue();
            dto.Cards = Cards.Select(card => new CardDataTransferObject
            {
                Value = card.GetValue(),
                Suit = card.GetSuit()
            }).ToList();
            return dto;
        }
    }
}