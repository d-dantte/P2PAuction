namespace P2PAuction.Peer.Contracts
{
    public interface IDiscoveryService
    {
        Task<JoinNetworkResponse> JoinNetwork(JoinNetworkRequest request);

        Task UpdateNodeLocation(UpdateLocationRequest request);
    }
}
