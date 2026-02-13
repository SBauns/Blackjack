using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using BlackjackAPI.Classes;

//WARNING AI GENERATED CODE PLEASE CHECK TODO
namespace BlackjackAPI.Services
{
    /// <summary>
    /// This is a hub for managing actions in real time between the server dealer and a number of players
    public class DealerHub : Hub
    {
        private readonly GameService _gameService;
        private readonly ILogger<DealerHub> _logger;
        public DealerHub(GameService gameService, ILogger<DealerHub> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new dealer and adds the player to it. The player is identified by their
        /// connection id.
        public async Task CreateDealer(string playerName)
        {
            try
            {
                Dealer dealer = _gameService.CreateDealer();

                await Groups.AddToGroupAsync(Context.ConnectionId, dealer.Id.ToString());

                Player player = new Player(new DomainPrimitives.Name(playerName), new DomainPrimitives.Quantity(1000), dealer, Context.ConnectionId);

                dealer.PlayerJoin(player);
                
                await Clients.Caller.SendAsync("DealerUpdated", dealer.ToDataTransferObject(), player.Id);
            }
            catch (System.Exception e)
            {
                await Clients.Caller.SendAsync("Error", e.Message);
            }
        }

        /// <summary>
        /// Adds the player to an existing dealer. The player is identified by their connection id.
        public async Task JoinDealer(string dealerId, string playerName)
        {
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, dealerId);

                Dealer dealer = _gameService.JoinDealer(dealerId);

                Player player = new Player(new DomainPrimitives.Name(playerName), new DomainPrimitives.Quantity(1000), dealer, Context.ConnectionId);

                dealer.PlayerJoin(player);

                await Clients.Group(dealerId).SendAsync("DealerUpdated", dealer.ToDataTransferObject(), player.Id);
            }
            catch (System.Exception e)
            {
                await Clients.Caller.SendAsync("Error", e.Message);
            }
        }

        /// <summary>
        /// This method allows the player to hit.
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

        /// <summary>
        /// This method allows the player to stand.
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
                dealer.EvaluateVictory();
                await Clients.Group(dealerId).SendAsync("GameOver", dealer.ToDataTransferObject());
            }
            else
            {
                await Clients.Group(dealerId).SendAsync("DealerUpdated", dealer.ToDataTransferObject(), player.Id);    
            }
        }

        /// <summary>
        /// This method starts a new game by resetting the dealer and all players, shuffling the deck and dealing new cards.
        public async Task NewGame(string dealerId)
        {
            Dealer? dealer = _gameService.GetDealer(dealerId);
            if (dealer == null)
                return;

            dealer.SetupGame();

            await Clients.Group(dealerId).SendAsync("DealerUpdated", dealer.ToDataTransferObject(), null);
        }

        /// <summary>
        /// This method is called when a player disconnects. It removes the player from any dealers they are part of.
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            foreach (Dealer dealer in _gameService.GetDealers().Values)
            {
                dealer.PlayerLeave(Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
