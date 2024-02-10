namespace P2PAuction.Auction.Contracts
{
    public class Auction
    {
        public Guid ID { get; set; }

        /// <summary>
        ///  Node ID of the owner of the auction
        /// </summary>
        public Guid OwnerId { get; set; }

        public string? ItemName { get; set; }

        public string? Description { get; set; }

        public decimal OpeningPrice { get; set; }

        public decimal ClosingPrice { get; set; }

        public DateTimeOffset OpeningTime { get; set; }

        public DateTimeOffset? ClosingTime { get; set; }

        public Guid? ClosingBidId { get; set; }
    }
}
