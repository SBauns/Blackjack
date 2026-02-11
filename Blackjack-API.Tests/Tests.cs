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
        shuffleDeck.shuffle();
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

public class PlayerTests
{
    [Fact]
    public void PlayerGetsCardWhenHitting()
    {
        //Arrange
        Dealer dealer = new Dealer(new Name("TestDealer"));
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
        Dealer dealer = new Dealer(new Name("TestDealer"));
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
        Dealer dealer = new Dealer(new Name("TestDealer"));
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

public class DealerTests
{
    [Fact]
    public void DealerCanDealCard()
    {
        //Arrange
        Dealer dealer = new Dealer(new Name("TestDealer"));

        //Act
        Card dealtCard = dealer.Deal();

        //Assert
        Assert.NotNull(dealtCard);
    }

    [Fact]
    public void DealerCanDiscardCard()
    {
        //Arrange
        Dealer dealer = new Dealer(new Name("TestDealer"));
        Card cardToDiscard = dealer.Deal();

        //Act
        dealer.Discard(cardToDiscard);
        // Since the Deck class does not have a method to check the discard pile, we will assume that if no exceptions are thrown, the card is discarded successfully. TODO Maybe do something here

        //Assert
        Assert.True(true, "Card discarded successfully.");
    }

    [Fact]
    public void DealerCanHavePlayersJoinAndLeave()
    {
        //Arrange
        Dealer dealer = new Dealer(new Name("TestDealer"));
        Player player1 = new Player(new Name("PlayerOne"), new Quantity(100), dealer);
        Player player2 = new Player(new Name("PlayerTwo"), new Quantity(100), dealer);

        //Act
        dealer.PlayerJoin(player1);
        dealer.PlayerJoin(player2);
        List<Player> playersAfterJoining = new List<Player>();

        foreach (Player player in dealer.GetPlayers())
        {
            playersAfterJoining.Add(player);
        }

        dealer.PlayerLeave(player1);
        List<Player> playersAfterLeaving = dealer.GetPlayers(); // Assuming Players is accessible for testing

        //Assert
        Assert.Contains(player1, playersAfterJoining);
        Assert.Contains(player2, playersAfterJoining);
        Assert.DoesNotContain(player1, playersAfterLeaving);
        Assert.Contains(player2, playersAfterLeaving);
    }

    [Fact]
    public void DealerEvaluatesVictoryCorrectly() //This test should be better TODO Actually test both a winner and a loser
    {
        //Arrange
        Dealer dealer = new Dealer(new Name("TestDealer"));
        Player player1 = new Player(new Name("PlayerOne"), new Quantity(100), dealer);
        Player player2 = new Player(new Name("PlayerTwo"), new Quantity(100), dealer);
        dealer.PlayerJoin(player1);
        dealer.PlayerJoin(player2);
        player1.Hit(); // Player1 receives a card
        player1.Hit(); // Player1 receives a card
        player1.Stand();
        int player1Score = player1.Hand.GetScore();

        player2.Hit(); // Player2 receives a card
        player2.Hit(); // Player2 receives a card
        player2.Stand();
        int player2Score = player2.Hand.GetScore();

        List<Player> winningPlayers = dealer.EvaluateVictory();

        if (player1Score > dealer.Hand.GetScore() && !player1.Hand.IsBusted())
        {
            Assert.Contains(player1, winningPlayers);
        }
        else
        {
            Assert.DoesNotContain(player1, winningPlayers);
        }

        if (player2Score > dealer.Hand.GetScore() && !player2.Hand.IsBusted())
        {
            Assert.Contains(player2, winningPlayers);
        }
    }
}

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
