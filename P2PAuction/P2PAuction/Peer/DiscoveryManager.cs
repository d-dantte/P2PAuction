using P2PAuction.Client;
using P2PAuction.Peer.Contracts;

namespace P2PAuction.Peer
{

    public sealed class DiscoveryManager:
        IDiscoveryService,
        IDisposable
    {
        private readonly INodeRepository _repository;
        private readonly INodeClientFactory _nodeClientFactory;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        private Directory? _directory;

        private CancellationTokenSource _source = new CancellationTokenSource();

        public DiscoveryManager(
            INodeRepository repository,
            INodeClientFactory nodeClientFactory)
        {
            ArgumentNullException.ThrowIfNull(repository);
            ArgumentNullException.ThrowIfNull(nodeClientFactory);

            _repository = repository;
            _nodeClientFactory = nodeClientFactory;
        }

        public void Dispose()
        {
            _source.Cancel();
            _source.Dispose();
        }

        /// <summary>
        /// The primary node is not allowed to change location (for simplicity sake).
        /// </summary>
        /// <param name="connectionEstablishedCallback"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Init(Action<bool>? connectionEstablishedCallback = null)
        {
            _directory = await _repository.GetDirectory();

            if(_directory.IsPrimaryInstance)
            {
                connectionEstablishedCallback?.Invoke(true);
                return;
            }

            // not primary instance, proceed to join network
            if (_directory.Participants.Count == 0)
            {
                // start task that attempts to join the network
                await Task
                    .Run(async () =>
                    {
                        var primaryClient = await _nodeClientFactory.CreateClient(_directory.PrimaryNode);

                        var request = new JoinNetworkRequest
                        {
                            Location = _directory.LocalNode.Location!.Value,
                            Name = _directory.LocalNode.Name,
                            NodeId = _directory.LocalNode.ID,
                        };

                        // try 10 times to join the network, waiting 30 seconds between each attempt
                        var count = 10;
                        while (count > 0)
                        {
                            try
                            {
                                var response = await primaryClient.DiscoveryService.JoinNetwork(request);
                                _directory = await _repository.PersistNodes(response.Nodes.Append(_directory.LocalNode));
                            }
                            catch (Exception ex)
                            {
                                // log the exception
                            }

                            count--;
                            await Task.Delay(30000);

                            if (_source.IsCancellationRequested)
                                throw new Exception("Cancellation requested");
                        }

                        throw new Exception("Could not join the network");
                    })
                    .ContinueWith(t =>
                    {
                        if (t.Status == TaskStatus.RanToCompletion)
                            connectionEstablishedCallback?.Invoke(true);

                        else connectionEstablishedCallback?.Invoke(false);
                    });
            }

            // address changed
            else if (!_directory.Participants[_directory.LocalNode.ID].Location.Equals(_directory.LocalNode.Location))
            {
                // start task that attempts to update location
                await Task
                    .Run(async () =>
                    {
                        // first, update local directory with address change
                        _directory = await _repository.PersistNodes(new[] { _directory.LocalNode });

                        var request = new UpdateLocationRequest
                        {
                            Location = _directory.LocalNode.Location!.Value,
                            NodeId = _directory.LocalNode.ID,
                        };

                        // broadcast the message to everyone
                        foreach (var node in _directory.ExternalNodes.Append(_directory.PrimaryNode))
                        {
                            try
                            {
                                var client = await _nodeClientFactory.CreateClient(node);
                                await client.DiscoveryService.UpdateNodeLocation(request);
                            }
                            catch(Exception ex)
                            {
                                // log exceptions
                            }

                            if (_source.IsCancellationRequested)
                                throw new Exception("Cancellation requested");
                        }
                    })
                    .ContinueWith(t =>
                    {
                        if (t.Status == TaskStatus.RanToCompletion)
                            connectionEstablishedCallback?.Invoke(true);

                        else connectionEstablishedCallback?.Invoke(false);
                    });
            }
        }

        public async Task<JoinNetworkResponse> JoinNetwork(JoinNetworkRequest request)
        {
            if (_directory!.Participants.ContainsKey(request.NodeId))
                throw new Exception("Node already joined");

            await _semaphore.WaitAsync(_source.Token);
            _directory = await _repository.PersistNodes(new[] { new NodeInfo
            {
                ID = request.NodeId,
                Location = request.Location,
                Name = request.Name
            }});

            var response = new JoinNetworkResponse();
            response.Nodes.AddRange(_directory.ExternalNodes);

            // if this is the primary node, broadcast to all OTHER particiapnts
            if (_directory.IsPrimaryInstance)
            {
                await Task
                    .Run(async () =>
                    {
                        foreach (var node in _directory.ExternalNodes.Where(n => n.ID != request.NodeId))
                        {
                            var client = await _nodeClientFactory.CreateClient(node);
                            _ = await client.DiscoveryService.JoinNetwork(request);
                        }
                    });
            }

            return response;
        }

        public async Task UpdateNodeLocation(UpdateLocationRequest request)
        {
            // for now if the node doesn't exit, ignore the call
            if (!_directory!.Participants.ContainsKey(request.NodeId))
                return;

            await _repository.UpdateLocation(request.NodeId, request.Location);

            await _semaphore.WaitAsync(_source.Token);

            var node = _directory.Participants[request.NodeId];
            node.Location = request.Location;
        }
    }
}
