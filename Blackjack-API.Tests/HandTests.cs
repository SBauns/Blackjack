using BlackjackAPI.Classes;
using BlackjackAPI.DomainPrimitives;

namespace Blackjack_API.Tests;

public class HandTests
{
    [Fact]
    public void HandBustsWhenOverThreshold()
    {
        //Arrange
        Hand hand = new Hand();
        Card card1 = new Card(new CardQuantity(10), Suit.Hearts);
        Card card2 = new Card(new CardQuantity(10), Suit.Diamonds);
        Card card3 = new Card(new CardQuantity(5), Suit.Clubs);

        card1.FlipUp();
        card2.FlipUp();
        card3.FlipUp();
        
        //Act
        hand.ReceiveCard(card1);
        hand.ReceiveCard(card2);
        hand.ReceiveCard(card3);
        bool isBusted = hand.IsBusted();

        //Assert
        Assert.True(isBusted, "Hand should be busted when total value exceeds 21.");
    }

    [Fact]
    public void HandCanReturnScore()
    {
        //Arrange
        Hand hand = new Hand();
        Card card1 = new Card(new CardQuantity(10), Suit.Hearts);
        Card card2 = new Card(new CardQuantity(5), Suit.Diamonds);

        //Act
        card1.FlipUp();
        card2.FlipUp();
        hand.ReceiveCard(card1);
        hand.ReceiveCard(card2);
        int score = hand.GetScore();

        //Assert
        Assert.Equal(15, score);
    }

    [Fact]
    public void HandCanReturnCards()
    {
        //Arrange
        Hand hand = new Hand();
        Card card1 = new Card(new CardQuantity(10), Suit.Hearts);
        Card card2 = new Card(new CardQuantity(5), Suit.Diamonds);

        //Act
        hand.ReceiveCard(card1);
        hand.ReceiveCard(card2);
        List<Card> returnedCards = hand.ReturnCards();

        //Assert
        Assert.Contains(card1, returnedCards);
        Assert.Contains(card2, returnedCards);
        Assert.Empty(hand.Cards);
    }
}