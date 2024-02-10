using P2PAuction.Auction.Contracts;
using P2PAuction.Peer.Contracts;

namespace P2PAuction.Client
{
    /// <summary>
    /// Represents a client used to talk access the services of other nodes on the network
    /// </summary>
    public class NodeClient
    {
        public IDiscoveryService DiscoveryService { get; }

        public IAuctionService AuctionService { get; }


        public NodeClient(
            IDiscoveryService discoveryService,
            IAuctionService auctionService)
        {
            ArgumentNullException.ThrowIfNull(discoveryService);
            ArgumentNullException.ThrowIfNull(auctionService);

            DiscoveryService = discoveryService;
            AuctionService = auctionService;
        }
    }
}
