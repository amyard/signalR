using System.Net.WebSockets;
using System.Text;

List<WebSocket> _connections = new();

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseWebSockets(new() {KeepAliveInterval = TimeSpan.FromSeconds(30)});

#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints.Map("/ws", async ctx =>
    {
        var buffer = new byte[1024 * 4];
        var webSocket = await ctx.WebSockets.AcceptWebSocketAsync();
        _connections.Add(webSocket);

        var result = await webSocket.ReceiveAsync(new(buffer), CancellationToken.None);
        int i = 0;
        while (!result.CloseStatus.HasValue)
        {
            var message = Encoding.UTF8.GetBytes($"message index {i++}");
            foreach (var c in _connections)
            {
                await c.SendAsync(new(message, 0, message.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);
            }

            result = await webSocket.ReceiveAsync(new(buffer), CancellationToken.None);

            Console.WriteLine($"Received: {Encoding.UTF8.GetString(buffer[..result.Count])}");
        }

        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        _connections.Remove(webSocket);
    });
});
#pragma warning restore ASP0014

//app.MapGet("/", () => "Hello World!");

app.Run();