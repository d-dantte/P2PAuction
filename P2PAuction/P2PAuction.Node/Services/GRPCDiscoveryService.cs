using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using P2PAuction.Peer.Contracts;

namespace P2PAuction.Node.Services
{
    public class GRPCDiscoveryService:
        DiscoveryService.DiscoveryServiceBase
    {
        private IDiscoveryService _discoveryService;

        public GRPCDiscoveryService(IDiscoveryService discoveryService)
        {
            ArgumentNullException.ThrowIfNull(discoveryService);

            _discoveryService = discoveryService;
        }

        public override async Task<JoinNetworkResponse> JoinNetwork(
            JoinNetworkRequest request,
            ServerCallContext context)
        {
            var rrequest = new Peer.Contracts.JoinNetworkRequest
            {
                Location = request.Location,
                Name = request.Name,
                NodeId = Guid.Parse(request.NodeId)
            };

            var result = await _discoveryService.JoinNetwork(rrequest);
            var response = new JoinNetworkResponse();
            result.Nodes
                .Select(n => new JoinNetworkResponse.Types.NodeInfo
                {
                    ID = n.ID.ToString(),
                    Location = n.Location,
                    Name = n.Name
                })
                .ToList()
                .ForEach(response.Nodes.Add);

            return response;
        }

        public override async Task<Empty> UpdateNetwork(
            UpdateLocationRequest request,
            ServerCallContext context)
        {
            var rrequest = new Peer.Contracts.UpdateLocationRequest
            {
                Location = request.Location,
                NodeId = Guid.Parse(request.NodeId)
            };

            await _discoveryService.UpdateNodeLocation(rrequest);

            return new Empty();
        }
    }
}
