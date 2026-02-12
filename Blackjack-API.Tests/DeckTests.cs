using BlackjackAPI.Classes;
using BlackjackAPI.DomainPrimitives;

namespace Blackjack_API.Tests;

public class DeckTests
{
    [Fact]
    public void DeckCanShuffleRandomly()
    {
        //Arrange
        Deck assertDeck = new Deck();
        Deck shuffleDeck = new Deck();
        //ACT
        shuffleDeck.Shuffle();
        //Assert
        Assert.NotEqual(assertDeck, shuffleDeck);
    }

    [Fact]
    public void DeckHas52Cards()
    {
        //Arrange
        Deck deck = new Deck();
        int cardCount = 0;

        //Act
        for (int i = 0; i < 52; i++)
        {
            deck.GetTopCard();
            cardCount++;
        }

        //Assert
        Assert.Equal(52, cardCount);
    }

    [Fact]
    public void DeckHas52CardsAfterShuffle()
    {
        //Arrange
        Deck deck = new Deck();
        int cardCount = 0;
        //Act
        deck.Shuffle();
        for (int i = 0; i < 52; i++)
        {
            deck.GetTopCard();
            cardCount++;
        }

        //Assert
        Assert.Equal(52, cardCount);
    }

    [Fact]
    public void DeckDoesNotHaveDuplicateCards()
    {
        //Arrange
        Deck deck = new Deck();
        HashSet<string> cardSet = new HashSet<string>();
        bool hasDuplicates = false;

        //Act
        for (int i = 0; i < 52; i++)
        {
            Card card = deck.GetTopCard();
            string cardIdentifier = card.GetValue().ToString() + card.GetSuit();
            if (!cardSet.Add(cardIdentifier))
            {
                hasDuplicates = true;
                break;
            }
        }

        //Assert
        Assert.False(hasDuplicates, "Deck contains duplicate cards.");
    }
}