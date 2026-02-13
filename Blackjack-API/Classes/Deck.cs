using BlackjackAPI.DomainPrimitives;

namespace BlackjackAPI.Classes
{
    public class Deck
    {
        //PROPERTIES
        private Quantity Count;

        private List<Card> Cards;

        private List<Card> DiscardedCards;

        public Deck()
        {
            Cards = new List<Card>();
            DiscardedCards = new List<Card>();
            InitializeDeck();
            Count = new Quantity(Cards.Count);
        }

        private void InitializeDeck()
        {
            try
            {
                //Generate standard 52 card deck
                foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    for (int value = 1; value <= 13; value++)
                    {
                        Cards.Add(new Card(new CardQuantity(value), suit));
                    }
                }    

                if (Cards == null)
                {
                    throw new System.Exception("Failed to initialize deck.");
                }

                Shuffle();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void Shuffle()
        {
            //Move all cards to discard
            foreach (Card card in Cards)
            {
                PutInDiscard(card);
            }
            Cards.Clear();
            //Randomly move cards back to deck
            //By taking a random card from the discard and putting it back in the deck until there are no more cards in the discard
            Random rng = new Random();
            int amountOfCards = DiscardedCards.Count;
            while (amountOfCards > 0)
            {
                amountOfCards--;
                int randomCard = rng.Next(amountOfCards + 1);
                Cards.Add(DiscardedCards[randomCard]);
                DiscardedCards.RemoveAt(randomCard);
            }
        }

        public Card GetTopCard()
        {

            if (Cards.Count == 0)
            {
                throw new System.Exception("Deck is empty.");
            }
            Card card = Cards[0];
            Cards.RemoveAt(0);
            return card; 
        }

        public void PutInDiscard(Card card)
        {
            card.FlipDown();
            DiscardedCards.Add(card);
        }
    }
}
