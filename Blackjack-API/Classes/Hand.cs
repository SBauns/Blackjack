using BlackjackAPI.DomainPrimitives;

namespace BlackjackAPI.Classes
{
    public class HandDataTransferObject
    {
        public int Score { get; set; }
        public bool IsBusted { get; set; }
        public List<CardDataTransferObject> Cards { get; set; } = new List<CardDataTransferObject>();
    }
    public class Hand
    {
        //PROPERTIES
        private Dictionary<string, Quantity> Scores;

        public bool Isbusted { get; private set; }

        //RELATIONSHIPS
        public List<Card> Cards { get; private set; }

        public Hand()
        {
            Scores = new Dictionary<string, Quantity>()
            {
                    {"Low", new Quantity(0)},
                    {"High", new Quantity(0)}
            };
            Isbusted = false;
            Cards = new List<Card>();
        }

        public void ResetScore()
        {
            Scores = new Dictionary<string, Quantity>()
            {
                    {"Low", new Quantity(0)},
                    {"High", new Quantity(0)}
            };
            Isbusted = false;
        }

        public void ReceiveCard(Card card)
        {
            Cards.Add(card);

            if (card.IsFaceDown())
            {
                return;
            }

            addToScore(card);

            IsBusted();
        }

        /// <summary>
        /// This method adds the value of a card to the hand's score.
        /// It handles the special cases for royalty cards (Jack, Queen, King) which are worth 10 points,
        /// and Aces which can be worth either 1 or 11 points.
        /// The method updates both the "Low" and "High" scores to account for the potential values of Aces.
        /// </summary>
        /// <param name="card"></param>
        private void addToScore(Card card)
        {
            if (card.GetValue() > 10) //Royalty cards are worth 10 points
            {
                Scores["Low"].addToValue(10);
                Scores["High"].addToValue(10); //Alternative value for Ace
            }
            else if (card.GetValue() == 1) //Ace can be worth 1 or 11 points
            {
                Scores["Low"].addToValue(1);
                Scores["High"].addToValue(11); //Alternative value for Ace
            }
            else
            {
                Scores["Low"].addToValue(card.GetValue());
                Scores["High"].addToValue(card.GetValue());
            }
        }

        public List<Card> ReturnCards()
        {
            List<Card> returnedCards = new List<Card>();
            foreach (Card card in Cards)
            {
                returnedCards.Add(card);
                Scores["Low"].substractFromValue(card.GetValue());
                Scores["High"].substractFromValue(card.GetValue());
            }
            Cards.Clear();
            return returnedCards;
        }

        public void FlipUpAllCards()
        {
            foreach (Card card in Cards)
            {
                if (card.IsFaceDown())
                {
                    card.FlipUp();
                    addToScore(card);
                }
            }
        }

        public int GetScore()
        {
            if (Scores["High"].GetValue() > 21)
            {
                return Scores["Low"].GetValue();
            }
            else
            {
                return Scores["High"].GetValue();
            }
        }

        public bool IsBusted()
        {
            if (Scores["Low"].GetValue() > 21 && Scores["High"].GetValue() > 21) //TODO Set this somewhere else or magic number
            {
                Isbusted = true;
            }
            return Isbusted;
        }
        public HandDataTransferObject ToDataTransferObject()
        {
            HandDataTransferObject dto = new HandDataTransferObject();
            dto.Score = GetScore();
            dto.IsBusted = Isbusted;
            dto.Cards = Cards.Select(card => card.ToDataTransferObject()).ToList();
            return dto;
        }
    }
}