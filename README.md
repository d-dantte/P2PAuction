# P2PAuction

### Unfinished Work
1. Subbing clients: I was unable to compete the implementation of the client stubs. what was left was to convert the `gRPC` requests/responses to the types used by the internal services.
2. CLI: I didn't get to start the CLI. I would have implemented a simple command line with the following instruction tree:
   1. List Auctions
     1. List Bids
     2. Place Bid
     3. Close Auction (for the owner of the auctino)
   2. Open Auction
   3. Exit
3. I didn't get to implement the repositories for the entities. This would have been a simple SQLite repository that sends instructions to the backing database.


 ### Limitations
 1. The discovery algorithm used is rudimentary and doesn't cater for a lot of edge-cases.
 2. Database calls can be optimized - using a better mechanism to cache the list of nodes so it isn't retrieved each time an operation needs to be made in the system.
