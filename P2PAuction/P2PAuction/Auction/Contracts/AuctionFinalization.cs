namespace P2PAuction.Auction.Contracts
{
    public class AuctionFinalization
    {
        public Guid AuctionId { get; set; }

        public Guid ClosingBidId { get; set; }
    }
}
