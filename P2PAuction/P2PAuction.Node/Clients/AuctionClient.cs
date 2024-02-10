using P2PAuction.Auction.Contracts;

namespace P2PAuction.Node.Clients
{
    public class AuctionClient : IAuctionService
    {
        private readonly AuctionService.AuctionServiceClient _client;

        public AuctionClient(AuctionService.AuctionServiceClient client)
        {
            _client = client;
        }

        public Task FinalizeAuction(Guid auctionId, Guid bidId)
        {
            throw new NotImplementedException();
        }

        public Task<List<P2PAuction.Auction.Contracts.Auction>> GetAllAuctions()
        {
            throw new NotImplementedException();
        }

        public Task<List<P2PAuction.Auction.Contracts.Bid>> GetBidds(Guid auctionId)
        {
            throw new NotImplementedException();
        }

        public Task<List<P2PAuction.Auction.Contracts.Auction>> GetFinalizedAuctions()
        {
            throw new NotImplementedException();
        }

        public Task<List<P2PAuction.Auction.Contracts.Auction>> GetOpenAuctions()
        {
            throw new NotImplementedException();
        }

        public Task NotifyAuctionFinalized(P2PAuction.Auction.Contracts.AuctionFinalization finalization)
        {
            throw new NotImplementedException();
        }

        public Task NotifyAuctionOpened(P2PAuction.Auction.Contracts.Auction auctionNotifiation)
        {
            throw new NotImplementedException();
        }

        public Task NotifyBidPlaced(P2PAuction.Auction.Contracts.Bid bid)
        {
            throw new NotImplementedException();
        }

        public Task<P2PAuction.Auction.Contracts.Auction> OpenAuction(string itemName, string description, decimal openingPrice)
        {
            throw new NotImplementedException();
        }

        public Task<P2PAuction.Auction.Contracts.Bid> PlaceBid(Guid auctionId, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
