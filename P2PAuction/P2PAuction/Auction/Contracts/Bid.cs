namespace P2PAuction.Auction.Contracts
{
    public class Bid
    {
        public Guid ID { get; set; }

        public Guid OwnerId { get; set; }

        public Guid AuctionId { get; set; }

        public decimal BidAmount { get; set; }

        public DateTimeOffset BidTime { get; set; }
    }
}
