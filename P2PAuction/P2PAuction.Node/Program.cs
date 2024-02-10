using P2PAuction.Client;
using P2PAuction.Node.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSingleton<INodeClientFactory, NodeClientFactory>();

//add SQLite repositories
//builder.Services.AddSingleton<IAuctionRepository, AuctionRepository>();
//builder.Services.AddSingleton<INodeRepository, NodeRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GRPCAuctionServce>();
app.MapGrpcService<GRPCDiscoveryService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

// start the app in a background task and implement the CLI here
app.Run();

