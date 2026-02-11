using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using BlackjackAPI.Classes;

//WARNING AI GENERATED CODE PLEASE CHECK TODO
namespace BlackjackAPI.Services
{
    public class DealerHub : Hub
    {
        private readonly GameService _gameService;
        private readonly ILogger<DealerHub> _logger;
        public DealerHub(GameService gameService, ILogger<DealerHub> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        public async Task JoinDealer(string dealerId, string playerName)
        {
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, dealerId);

                Dealer dealer = _gameService.JoinDealer(dealerId);

                Player player = new Player(new DomainPrimitives.Name(playerName), new DomainPrimitives.Quantity(1000), dealer);

                dealer.PlayerJoin(player);

                dealer.SetupGame();

                await Clients.Group(dealerId).SendAsync("DealerUpdated", dealer.ToDataTransferObject(), player.Id);
            }
            catch (System.Exception e)
            {
                await Clients.Caller.SendAsync("Error", e.Message);
            }
        }

        public async Task Hit(string dealerId, string playerId)
        {
            Dealer? dealer = _gameService.GetDealer(dealerId);
            if (dealer == null)
                return;

            Player? player = dealer.GetPlayers().FirstOrDefault(p => p.Id.ToString() == playerId);

            if (player == null)
                return;

            if(!player.Hand.IsBusted() && !player.IsStanding)
            {
                player.Hit();
            }
            else
            {
                player.Stand();
            }


            if(dealer.IsGameOver())
            {
                dealer.EvaluateVictory();
                await Clients.Group(dealerId).SendAsync("GameOver", dealer.ToDataTransferObject());
            }
            else
            {
                await Clients.Group(dealerId).SendAsync("DealerUpdated", dealer.ToDataTransferObject(), player.Id);         
            }
        }

        public async Task Stand(string dealerId, string playerId)
        {
            Dealer? dealer = _gameService.GetDealer(dealerId);
            if (dealer == null)
                return;

            Player? player = dealer.GetPlayers().FirstOrDefault(p => p.Id.ToString() == playerId);

            if (player == null)
                return;

            player.Stand();

            if(dealer.IsGameOver())
            {
                _logger.LogInformation($"Game over for dealer {dealerId}.");
                dealer.EvaluateVictory();
                await Clients.Group(dealerId).SendAsync("GameOver", dealer.ToDataTransferObject());
            }
            else
            {
                _logger.LogInformation($"Player {playerId} stands for dealer {dealerId}.");
                await Clients.Group(dealerId).SendAsync("DealerUpdated", dealer.ToDataTransferObject(), player.Id);    
            }
        }

        // public override async Task OnDisconnectedAsync(Exception? exception)
        // {
        //     foreach (var dealer in Dealers.Values)
        //     {
        //         dealer.PlayerLeave(Context.ConnectionId);
        //     }

        //     await base.OnDisconnectedAsync(exception);
        // }
    }
}
