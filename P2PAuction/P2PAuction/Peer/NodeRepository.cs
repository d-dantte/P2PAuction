using P2PAuction.Network;

namespace P2PAuction.Peer
{
    public interface INodeRepository
    {
        /// <summary>
        /// Loads the directory from the DB, and local files (for local and primary instances).
        /// Node that this method is also responsible for setting the IsPrimaryInstance on the relevant node
        /// </summary>
        /// <returns></returns>
        public Task<Directory> GetDirectory();

        public Task<Directory> PersistNodes(IEnumerable<NodeInfo> participantNodes);

        public Task UpdateLocation(Guid nodeId, Location location);
    }
}
