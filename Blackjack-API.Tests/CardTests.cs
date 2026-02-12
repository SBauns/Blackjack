using BlackjackAPI.Classes;
using BlackjackAPI.DomainPrimitives;

namespace Blackjack_API.Tests;

public class CardTests
{
    [Fact]
    public void CardCanReturnValue()
    {
        //Arrange
        Card card = new Card(new CardQuantity(10), Suit.Hearts);

        //Act
        int value = card.GetValue();

        //Assert
        Assert.Equal(10, value);
    }

    [Fact]
    public void CardCanReturnSuit()
    {
        //Arrange
        Card card = new Card(new CardQuantity(10), Suit.Hearts);

        //Act
        string suit = card.GetSuit();

        //Assert
        Assert.Equal("Hearts", suit);
    }
}
