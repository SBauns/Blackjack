using BlackjackAPI.Classes;
using BlackjackAPI.DomainPrimitives;

namespace Blackjack_API.Tests;

public class PlayerTests
{
    [Fact]
    public void PlayerGetsCardWhenHitting()
    {
        //Arrange
        Dealer dealer = new Dealer();
        Player player = new Player(new Name("TestPlayer"), new Quantity(100), dealer);

        //Act
        player.Hit();
        List<Card> playerCards = player.Hand.Cards;

        //Assert
        Assert.Single(playerCards);

        //Act again
        player.Hit();
        playerCards = player.Hand.Cards;

        //Assert again
        Assert.Equal(2, playerCards.Count);
    }

    [Fact]
    public void PlayerCanStand()
    {
        //Arrange
        Dealer dealer = new Dealer();
        Player player = new Player(new Name("TestPlayer"), new Quantity(100), dealer);

        //Act
        player.Stand();
        bool isStanding = player.IsStanding;

        //Assert
        Assert.True(isStanding, "Player should be standing after calling Stand().");
    }

    [Fact]
    public void PlayerCanResetHand()
    {
        //Arrange
        Dealer dealer = new Dealer();
        Player player = new Player(new Name("TestPlayer"), new Quantity(100), dealer);
        player.Hit(); // Player receives a card

        //Act
        player.Reset();
        List<Card> playerCards = player.Hand.Cards;
        bool isStanding = player.IsStanding;

        //Assert
        Assert.Empty(playerCards);
        Assert.False(isStanding, "Player should not be standing after resetting hand.");
    }
}