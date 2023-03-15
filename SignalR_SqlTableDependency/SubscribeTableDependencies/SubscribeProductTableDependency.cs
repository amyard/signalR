using SignalR_SqlTableDependency.Hubs;
using SignalR_SqlTableDependency.Models;
using TableDependency.SqlClient;

namespace SignalR_SqlTableDependency.SubscribeTableDependencies;

public class SubscribeProductTableDependency
{
    private readonly DashboardHub _hub;
    private readonly IConfiguration _config;
    private SqlTableDependency<Product> _tableDependency;

    public SubscribeProductTableDependency(DashboardHub hub, IConfiguration config)
    {
        _hub = hub;
        _config = config;
    }
    
    public void SubscribeTableDependency()
    {
        var connectionString = _config.GetConnectionString("DefaultConnection");
        _tableDependency = new SqlTableDependency<Product>(connectionString);
        _tableDependency.OnChanged += TableDependency_OnChanged;
        _tableDependency.OnError += TableDependency_OnError;
        _tableDependency.Start();
    }

    private void TableDependency_OnChanged(object sender,
        TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Product> e)
    {
        // when there is a change data, we call hub
        if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
        {
            _hub.SendProducts();
        }
    }

    public void TableDependency_OnError(object sender, 
        TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
    {
        Console.WriteLine($"{nameof(Product)} SqlTableDependency error: {e.Error.Message}");
    }
}