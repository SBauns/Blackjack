using BlackjackAPI.Classes;
using System.Collections.Concurrent;

namespace BlackjackAPI.Services
{
    public class GameService
    {

        private static ConcurrentDictionary<string, Dealer> Dealers = new();



        public GameService()
        {
            
        }

        public Dealer JoinDealer(string dealerId)
        {
            Dealer dealer = Dealers.GetOrAdd(dealerId, id => new Dealer());

            return dealer;
        }

        public Dealer GetDealer(string dealerId)
        {
            Dealers.TryGetValue(dealerId, out var dealer);
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
