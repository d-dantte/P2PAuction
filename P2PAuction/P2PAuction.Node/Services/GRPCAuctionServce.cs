using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using P2PAuction.Auction.Contracts;

namespace P2PAuction.Node.Services
{
    public class GRPCAuctionServce: AuctionService.AuctionServiceBase
    {
        private IAuctionService _auctionService;

        public GRPCAuctionServce(IAuctionService auctionService)
        {
            ArgumentNullException.ThrowIfNull(auctionService);

            _auctionService = auctionService;
        }

        public override async Task<Empty> NotifyAuctionFinalized(
            AuctionFinalization request,
            ServerCallContext context)
        {
            var rrequest = new P2PAuction.Auction.Contracts.AuctionFinalization
            {
                AuctionId = Guid.Parse(request.AuctionId),
                ClosingBidId = Guid.Parse(request.ClosingBidId)
            };

            await _auctionService.NotifyAuctionFinalized(rrequest);
            return new Empty();
        }

        public override async Task<Empty> NotifyAuctionOpened(
            Auction request,
            ServerCallContext context)
        {
            var rrequest = new P2PAuction.Auction.Contracts.Auction
            {
                ItemName = request.ItemName,
                Description = request.Description,
                ID = Guid.Parse(request.ID),
                OwnerId = Guid.Parse(request.OwnerId),
                OpeningPrice = request.OpeningPrice/100m,
                OpeningTime = new DateTimeOffset(request.OpeningTime, TimeSpan.Zero)
            };

            await _auctionService.NotifyAuctionOpened(rrequest);
            return new Empty();
        }

        public override async Task<Empty> NotifyBidPlaced(
            Bid request,
            ServerCallContext context)
        {
            var rrequest = new P2PAuction.Auction.Contracts.Bid
            {
                ID = Guid.Parse(request.ID),
                AuctionId = Guid.Parse(request.AuctionId),
                BidAmount = request.BidAmount/100m,
                OwnerId = Guid.Parse(request.OwnerId),
                BidTime = new DateTimeOffset(request.BidTime, TimeSpan.Zero)
            };

            await _auctionService.NotifyBidPlaced(rrequest);
            return new Empty();
        }
    }
}
