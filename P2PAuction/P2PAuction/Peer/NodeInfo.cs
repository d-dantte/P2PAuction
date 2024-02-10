using P2PAuction.Network;

namespace P2PAuction.Peer
{
    public class NodeInfo
    {
        public Guid ID { get; set; }

        public string? Name { get; set; }

        public bool IsPrimaryNode { get; set; }


        public Location? Location { get; set; }
    }
}
