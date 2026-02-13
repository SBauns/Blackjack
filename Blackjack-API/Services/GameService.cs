using BlackjackAPI.Classes;
using System.Collections.Concurrent;

namespace BlackjackAPI.Services
{
    /// <summary>
    /// This service manages the game state for all dealers and players. 
    /// It allows for creating and joining dealers, starting and ending games, 
    /// and retrieving the current state of dealers and players. 
    /// It uses a concurrent dictionary to store the dealers, 
    /// allowing for thread-safe access in a multi-threaded environment such as a web server.
    /// This service could have access to a database in the future for persistence, but currently it is in-memory only.
    /// </summary>
    public class GameService
    {

        private static ConcurrentDictionary<string, Dealer> Dealers = new();



        public GameService()
        {
            
        }

        public Dealer CreateDealer()
        {
            Dealer dealer = new Dealer();
            Dealers.TryAdd(dealer.Id.ToString(), dealer);
            return dealer;
        }

        public Dealer JoinDealer(string dealerId)
        {
            Dealer dealer = Dealers.First(dealerIdValue => dealerIdValue.Key == dealerId).Value;

            return dealer;
        }

        public void LeaveDealer(string dealerId, string playerId)
        {
            if (Dealers.TryGetValue(dealerId, out var dealer))
            {
                dealer.PlayerLeave(playerId);
            }
        }

        public Dealer GetDealer(string dealerId)
        {
            Dealers.TryGetValue(dealerId, out var dealer);
            if (dealer == null)
            {
                throw new Exception("Dealer not found.");
            }
            return dealer;
        }

        public ConcurrentDictionary<string, Dealer> GetDealers()
        {
            return Dealers;
        }

        public Dealer StartGame()
        {
            Dealer dealer = new Dealer();
            Dealers.AddOrUpdate(dealer.Id.ToString(), dealer, (key, existingDealer) => dealer);
            return dealer;
        }

        public void EndGame(Dealer dealer)
        {
            Dealers.TryRemove(dealer.Id.ToString(), out _);
        }
    }
}
