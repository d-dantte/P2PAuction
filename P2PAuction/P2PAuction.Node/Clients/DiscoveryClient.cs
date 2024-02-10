using P2PAuction.Peer.Contracts;

namespace P2PAuction.Node.Clients
{
    public class DiscoveryClient: IDiscoveryService
    {
        private readonly DiscoveryService.DiscoveryServiceClient _client;

        public DiscoveryClient(DiscoveryService.DiscoveryServiceClient client)
        {
            _client = client;
        }

        public Task<Peer.Contracts.JoinNetworkResponse> JoinNetwork(Peer.Contracts.JoinNetworkRequest request)
        {
            throw new NotImplementedException();
        }

        public Task UpdateNodeLocation(Peer.Contracts.UpdateLocationRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
