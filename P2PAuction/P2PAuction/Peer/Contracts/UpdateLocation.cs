using P2PAuction.Network;

namespace P2PAuction.Peer.Contracts
{
    public class UpdateLocationRequest
    {
        public Guid NodeId { get; set; }

        public Location Location { get; set; }
    }
}
