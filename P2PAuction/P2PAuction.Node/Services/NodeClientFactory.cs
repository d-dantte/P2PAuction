using Grpc.Net.Client;
using P2PAuction.Client;
using P2PAuction.Node.Clients;
using P2PAuction.Peer;

namespace P2PAuction.Node.Services
{
    public class NodeClientFactory : INodeClientFactory
    {
        public Task<NodeClient> CreateClient(NodeInfo nodeInfo)
        {
            var ip = nodeInfo.Location!.Value.GetIPAddress();
            var port = nodeInfo.Location!.Value.Port;

            var discoveryService = new DiscoveryService.DiscoveryServiceClient(
                GrpcChannel.ForAddress($"http://{ip}:{port}"));

            var auctionService = new AuctionService.AuctionServiceClient(
                GrpcChannel.ForAddress($"http://{ip}:{port}"));

            return Task.FromResult(new NodeClient(
                new DiscoveryClient(discoveryService),
                new AuctionClient(auctionService)));
        }
    }
}
