namespace P2PAuction.Auction.Contracts
{
    public interface IAuctionService
    {
        Task<List<Auction>> GetOpenAuctions();

        Task<List<Auction>> GetFinalizedAuctions();

        Task<List<Auction>> GetAllAuctions();

        Task<List<Bid>> GetBidds(Guid auctionId);


        #region Open auction
        Task<Auction> OpenAuction(
            string itemName,
            string description,
            decimal openingPrice);

        /// <summary>
        /// GRPC
        /// </summary>
        Task NotifyAuctionOpened(Auction auctionNotifiation);
        #endregion

        #region Finalize auction
        Task FinalizeAuction(Guid auctionId, Guid bidId);

        /// <summary>
        /// GRPC
        /// </summary>
        Task NotifyAuctionFinalized(AuctionFinalization finalization);
        #endregion

        #region Place bid
        Task<Bid> PlaceBid(Guid auctionId, decimal amount);

        Task NotifyBidPlaced(Bid bid);
        #endregion
    }
}
