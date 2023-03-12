using ChatApp.Context;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.ViewComponents;

public class RoomViewComponent : ViewComponent
{
    private readonly AppDbContext _ctx;

    public RoomViewComponent(AppDbContext ctx)
    {
        _ctx = ctx;
    }
    public IViewComponentResult Invoke()
    {
        var chats = _ctx.Chats.ToList();
        
        return View(chats);
    }
}