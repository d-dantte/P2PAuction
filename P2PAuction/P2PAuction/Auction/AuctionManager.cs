using P2PAuction.Auction.Contracts;
using P2PAuction.Client;
using P2PAuction.Peer;

namespace P2PAuction.Auction
{
    public class AuctionManager : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly INodeRepository _nodeRepository;
        private readonly INodeClientFactory _nodeClientFactory;

        public AuctionManager(
            IAuctionRepository auctionRepository,
            INodeRepository nodeRepository,
            INodeClientFactory nodeClientFactory)
        {
            ArgumentNullException.ThrowIfNull(auctionRepository);
            ArgumentNullException.ThrowIfNull(nodeRepository);
            ArgumentNullException.ThrowIfNull(nodeClientFactory);

            _auctionRepository = auctionRepository;
            _nodeRepository = nodeRepository;
            _nodeClientFactory = nodeClientFactory;
        }

        public Task<List<Auction>> GetOpenAuctions() => _auctionRepository.GetOpenAuctions();

        public Task<List<Auction>> GetFinalizedAuctions() => _auctionRepository.GetFinalizedAuctions();

        public Task<List<Auction>> GetAllAuctions() => _auctionRepository.GetAllAuctions();

        public Task<List<Bid>> GetBidds(Guid auctionId) => _auctionRepository.GetBidds(auctionId);


        #region Open auction
        public async Task<Auction> OpenAuction(
            string itemName,
            string description,
            decimal openingPrice)
        {
            if (string.IsNullOrWhiteSpace(itemName))
                throw new ArgumentException($"Invalid name: {itemName}");

            if (openingPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(openingPrice));

            var directory = await _nodeRepository.GetDirectory();
            var auction = new Auction
            {
                ID = Guid.NewGuid(),
                ItemName = itemName,
                Description = description,
                OpeningPrice = openingPrice,
                OwnerId = directory.LocalNode.ID,
                OpeningTime = DateTimeOffset.Now
            };

            foreach (var node in directory.Participants.Values.Prepend(directory.PrimaryNode))
            {
                var service = node.ID != directory.LocalNode.ID
                    ? (await _nodeClientFactory.CreateClient(node)).AuctionService
                    : this;
                try
                {
                    await service.NotifyAuctionOpened(auction);
                }
                catch(Exception ex)
                {
                    // log exception
                }
            }

            return auction;
        }

        /// <summary>
        /// GRPC
        /// </summary>
        public async Task NotifyAuctionOpened(Auction auction)
        {
            ArgumentNullException.ThrowIfNull(auction);

            if ((await _auctionRepository.GetAuction(auction.ID)) is not null)
                throw new InvalidOperationException("Auction already exists");

            await _auctionRepository.CreateAuction(auction);
        }
        #endregion

        #region Finalize auction
        public async Task FinalizeAuction(Guid auctionId, Guid bidId)
        {
            if (auctionId == Guid.Empty)
                throw new ArgumentException($"Invalid auctionId: {auctionId}");

            if (bidId == Guid.Empty)
                throw new ArgumentException($"Invalid bidId: {bidId}");

            var directory = await _nodeRepository.GetDirectory();
            var finalization = new AuctionFinalization
            {
                AuctionId = auctionId,
                ClosingBidId = bidId,
            };

            foreach (var node in directory.Participants.Values.Prepend(directory.PrimaryNode))
            {
                var service = node.ID != directory.LocalNode.ID
                    ? (await _nodeClientFactory.CreateClient(node)).AuctionService
                    : this;
                try
                {
                    await service.NotifyAuctionFinalized(finalization);
                }
                catch (Exception ex)
                {
                    // log exception
                }
            }
        }

        /// <summary>
        /// GRPC
        /// </summary>
        public async Task NotifyAuctionFinalized(AuctionFinalization finalization)
        {
            ArgumentNullException.ThrowIfNull(finalization);

            var auction = await _auctionRepository.GetAuction(finalization.AuctionId)
                ?? throw new InvalidOperationException($"Auction does not exist");

            var closingBid = await _auctionRepository.GetBid(finalization.ClosingBidId)
                ?? throw new InvalidOperationException($"Bid does not exist");

            auction.ClosingBidId = finalization.ClosingBidId;
            auction.ClosingTime = DateTimeOffset.Now;
            auction.ClosingPrice = closingBid.BidAmount;

            await _auctionRepository.UpdateAuction(auction);
        }
        #endregion

        #region Place bid
        public async Task<Bid> PlaceBid(Guid auctionId, decimal amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var directory = await _nodeRepository.GetDirectory();
            var bid = new Bid
            {
                ID = Guid.NewGuid(),
                AuctionId = auctionId,
                BidAmount = amount,
                BidTime = DateTimeOffset.Now,
                OwnerId = directory.LocalNode.ID
            };

            foreach (var node in directory.Participants.Values.Prepend(directory.PrimaryNode))
            {
                var service = node.ID != directory.LocalNode.ID
                    ? (await _nodeClientFactory.CreateClient(node)).AuctionService
                    : this;
                try
                {
                    await service.NotifyBidPlaced(bid);
                }
                catch (Exception ex)
                {
                    // log exception
                }
            }

            return bid;
        }

        public async Task NotifyBidPlaced(Bid bid)
        {
            ArgumentNullException.ThrowIfNull(bid);

            if ((await _auctionRepository.GetAuction(bid.AuctionId)) is null)
                throw new InvalidOperationException($"Auction does not exist");

            if ((await _auctionRepository.GetBid(bid.ID)) is not null)
                throw new InvalidOperationException($"Bid already exists");


            _ = await _auctionRepository.CreateBid(bid);
        }
        #endregion
    }
}
