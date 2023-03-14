using Microsoft.AspNetCore.SignalR;
using SignalR_SqlTableDependency.Repositories;

namespace SignalR_SqlTableDependency.Hubs;

public class DashboardHub : Hub
{
    private readonly ProductRepository _productRepository;

    public DashboardHub(IConfiguration config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection");
        _productRepository = new ProductRepository(connectionString);
    }
    
    public async Task SendProducts()
    {
        var products = _productRepository.GetProducts();
        await Clients.All.SendAsync("ReceivedProducts", products);
    }
}