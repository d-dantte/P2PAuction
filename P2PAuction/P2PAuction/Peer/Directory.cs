namespace P2PAuction.Peer
{
    public class Directory
    {
        /// <summary>
        /// The primary node, loaded from the static file
        /// </summary>
        public NodeInfo PrimaryNode { get; set; }

        /// <summary>
        /// The local node, loaded form the static file.
        /// </summary>
        public NodeInfo LocalNode { get; set; }

        public Dictionary<Guid, NodeInfo> Participants { get; } = new Dictionary<Guid, NodeInfo>();

        public IEnumerable<NodeInfo> ExternalNodes => Participants.Values.Where(x => x.ID != LocalNode.ID);

        public bool IsPrimaryInstance => PrimaryNode.ID == LocalNode.ID;

        public Directory(NodeInfo primaryNode) : this(primaryNode, primaryNode)
        { }

        public Directory(NodeInfo primaryNode, NodeInfo localNode)
        {
            ArgumentNullException.ThrowIfNull(primaryNode);
            ArgumentNullException.ThrowIfNull(localNode);

            if (!primaryNode.IsPrimaryNode)
                throw new ArgumentException($"Invalid primary node");

            PrimaryNode = primaryNode;
            LocalNode = localNode;
        }
    }
}
