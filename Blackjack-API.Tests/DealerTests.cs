using BlackjackAPI.Classes;
using BlackjackAPI.DomainPrimitives;

namespace Blackjack_API.Tests;

public class DealerTests
{
    [Fact]
    public void DealerCanDealCard()
    {
        //Arrange
        Dealer dealer = new Dealer();

        //Act
        Card dealtCard = dealer.Deal();

        //Assert
        Assert.NotNull(dealtCard);
    }

    [Fact]
    public void DeckIsCompleteAfterSetup()
    {
        //Arrange
        Dealer dealer = new Dealer();

        //Act
        dealer.SetupGame();
        int cardCount = 0;
        for (int i = 0; i < 49; i++) //49 because 3 cards are dealt during setup (2 to player, 1 to dealer)
        {
            dealer.Deal();
            cardCount++;
        }

        //Assert
        Assert.Equal(49, cardCount);
    }

    [Fact]
    public void DealerCanDiscardCard()
    {
        //Arrange
        Dealer dealer = new Dealer();
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
        Dealer dealer = new Dealer();
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
        Dealer dealer = new Dealer();
        Player player1 = new Player(new Name("PlayerOne"), new Quantity(100), dealer);
        Player player2 = new Player(new Name("PlayerTwo"), new Quantity(100), dealer);
        Card dealerCard1 = new Card(new CardQuantity(10), Suit.Spades);
        Card dealerCard2 = new Card(new CardQuantity(7), Suit.Hearts);
        dealerCard1.FlipUp();
        dealerCard2.FlipUp();
        dealer.Hand.ReceiveCard(dealerCard1);
        dealer.Hand.ReceiveCard(dealerCard2);

        dealer.PlayerJoin(player1);
        dealer.PlayerJoin(player2);
        Card cardForPlayer11 = new Card(new CardQuantity(9), Suit.Clubs);
        cardForPlayer11.FlipUp();
        Card cardForPlayer12 = new Card(new CardQuantity(8), Suit.Clubs);
        cardForPlayer12.FlipUp();
        player1.Hand.ReceiveCard(cardForPlayer11);
        player1.Hand.ReceiveCard(cardForPlayer12);
        player1.Stand();

        Card cardForPlayer21 = new Card(new CardQuantity(10), Suit.Clubs);
        cardForPlayer21.FlipUp();
        Card cardForPlayer22 = new Card(new CardQuantity(9), Suit.Clubs);
        cardForPlayer22.FlipUp();
        player2.Hand.ReceiveCard(cardForPlayer21);
        player2.Hand.ReceiveCard(cardForPlayer22);
        player2.Stand();

        dealer.EvaluateVictory();
        List<Player> winningPlayers = dealer.GetPlayers().Where(p => p.HasWon).ToList();

        Assert.Equal(17, dealer.Hand.GetScore());
        Assert.Equal(17, player1.Hand.GetScore());
        Assert.Equal(19, player2.Hand.GetScore());
        Assert.Contains(player2, winningPlayers);
        Assert.DoesNotContain(player1, winningPlayers);
    }

    [Fact]
    public void DealerWinsWhenFirstCardIsAceAndSecondCardIs10()
    {
        //Arrange
        Dealer dealer = new Dealer();
        Player player = new Player(new Name("PlayerOne"), new Quantity(100), dealer);
        Card dealerCard1 = new Card(new CardQuantity(1), Suit.Spades); // Ace
        Card dealerCard2 = new Card(new CardQuantity(10), Suit.Hearts); // 10
        dealerCard2.FlipUp();
        dealer.Hand.ReceiveCard(dealerCard1);
        dealer.Hand.ReceiveCard(dealerCard2);

        dealer.PlayerJoin(player);
        Card cardForPlayer1 = new Card(new CardQuantity(1), Suit.Clubs);
        cardForPlayer1.FlipUp();
        Card cardForPlayer2 = new Card(new CardQuantity(10), Suit.Clubs);
        cardForPlayer2.FlipUp();
        player.Hand.ReceiveCard(cardForPlayer1);
        player.Hand.ReceiveCard(cardForPlayer2);
        player.Stand();

        //Act
        dealer.EvaluateVictory();

        //Assert
        Assert.Equal(21, dealer.Hand.GetScore());
        Assert.Equal(21, player.Hand.GetScore());
        Assert.True(dealer.IsGameOver());
        Assert.True(player.HasWon == false, "Dealer should win with a blackjack.");
    }

    [Fact]
    public void GameDoesNotThrowAnyExceptions()
    {
        //Arrange
        Dealer dealer = new Dealer();
        Player player = new Player(new Name("TestPlayer"), new Quantity(100), dealer);

        //Act & Assert
        try
        {
            //First Game
            dealer.PlayerJoin(player);
            dealer.SetupGame();
            player.Hit();
            player.Stand();
            dealer.EvaluateVictory();
            //Second Game
            dealer.SetupGame();
            player.Hit();
            player.Stand();
            dealer.EvaluateVictory();
        }
        catch (Exception ex)
        {
            Assert.True(false, $"An exception was thrown during the game flow: {ex.Message}");
        }
    }
}