using P2PAuction.Auction.Contracts;

namespace P2PAuction.Auction
{
    public interface IAuctionRepository
    {

        public Task<List<Contracts.Auction>> GetOpenAuctions();

        public Task<List<Contracts.Auction>> GetFinalizedAuctions();

        public Task<List<Contracts.Auction>> GetAllAuctions();

        public Task<List<Bid>> GetBidds(Guid auctionId);

        public Task<Bid> GetBid(Guid bidId);

        public Task<Contracts.Auction> GetAuction(Guid auctionId);

        public Task<Bid> CreateBid(Bid bid);

        public Task<Contracts.Auction> CreateAuction(Contracts.Auction auction);

        public Task<Contracts.Auction> UpdateAuction(Contracts.Auction auction);
    }
}
