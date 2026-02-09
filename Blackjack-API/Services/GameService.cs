using BlackjackAPI.Classes;

namespace BlackjackAPI.Services
{
    public class GameService
    {
        public List<Dealer> Dealers = new List<Dealer>();
        public GameService()
        {
            
        }

        public Dealer StartGame()
        {
            Dealer dealer = new Dealer(new DomainPrimitives.Name(RandomDealerName()));
            Dealers.Add(dealer);
            return dealer;
        }

        public void EndGame(Dealer dealer)
        {
            Dealers.Remove(dealer);
        }

        private string RandomDealerName()
        {
            string[] names = { "Alice", "Bob", "Charlie", "Diana", "Eve" };
            Random rand = new Random();
            int index = rand.Next(names.Length);
            return names[index];
        }
    }
}
