using P2PAuction.Peer;

namespace P2PAuction.Client
{
    /// <summary>
    /// A factory that generates node clients out of node info instances
    /// </summary>
    public interface INodeClientFactory
    {
        Task<NodeClient> CreateClient(NodeInfo nodeInfo);
    }
}
