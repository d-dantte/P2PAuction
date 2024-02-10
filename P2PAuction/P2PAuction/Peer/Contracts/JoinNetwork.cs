using P2PAuction.Network;

namespace P2PAuction.Peer.Contracts
{
    public class JoinNetworkRequest
    {
        public Guid NodeId { get; set; }

        public string? Name { get; set; }

        public Location Location { get; set; }
    }

    /// <summary>
    /// Respond with a list of all nodes on the network, minus the node that made the request
    /// </summary>
    public class JoinNetworkResponse
    {
        public List<NodeInfo> Nodes { get; } = new List<NodeInfo>();
    }
}
