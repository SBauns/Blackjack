using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlackjackAPI.Services;
using BlackjackAPI.Classes;

namespace BlackjackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("dealers")]
        public IActionResult GetDealers()
        {
            return Ok(_gameService.GetDealers().Select(d => d.Value.ToDataTransferObject()));
        }

        [HttpPost("start")]
        public IActionResult StartGame(string playerName)
        {
            Dealer dealer =_gameService.StartGame();
            Player player = new Player(new DomainPrimitives.Name(playerName), new DomainPrimitives.Quantity(1000), dealer);
            dealer.PlayerJoin(player);
            player.Hit(); //Initial card
            return Ok(new { message = $"Player {playerName} joined the game.", dealer = dealer.ToDataTransferObject(), player = player.ToDataTransferObject() });
        }


        [HttpGet("hit/{dealerId}")]
        public IActionResult Hit(Guid dealerId)
        {
            CheckForDealerAndPlayer(dealerId, out Dealer? dealer, out Player? player);
            player.Hit();
            return Ok(new { message = "Player hit.", hand = player.Hand.ToDataTransferObject() });
        }

        // [HttpGet("stand/{dealerId}")]
        // public IActionResult Stand(Guid dealerId)
        // {
        //     CheckForDealerAndPlayer(dealerId, out Dealer? dealer, out Player? player);
        //     player.Stand();
        //     if(dealer.IsGameOver())
        //     {
        //         List<Player> winners = dealer.EvaluateVictory();
        //         return Ok(new { message = "Player stands. Game over.", winners = winners.Select(player => player.ToDataTransferObject()), dealerHand = dealer.Hand.ToDataTransferObject() });
        //     }
        //     return Ok(new { message = "Player stands.", hand = player.Hand.ToDataTransferObject() });
        // }

        private void CheckForDealerAndPlayer(Guid dealerId, out Dealer? dealer, out Player? player)
        {
            dealer = _gameService.GetDealers().FirstOrDefault(d => d.Value.Id == dealerId).Value;
            if (dealer == null)
            {
                throw new Exception("Dealer not found.");
            }

            player = dealer.GetPlayers().FirstOrDefault();
            if (player == null)
            {
                throw new Exception("Player not found.");
            }
        }
    }
}
