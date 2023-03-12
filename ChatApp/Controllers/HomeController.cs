using System.Diagnostics;
using ChatApp.Context;
using ChatApp.Enums;
using Microsoft.AspNetCore.Mvc;
using ChatApp.Models;

namespace ChatApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _ctx;

    public HomeController(ILogger<HomeController> logger, AppDbContext ctx)
    {
        _logger = logger;
        _ctx = ctx;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom(string name)
    {
        _ctx.Chats.Add(new Chat()
        {
            Name = name,
            Type = ChatType.Room
        });
        
        await _ctx.SaveChangesAsync();
        
        return RedirectToAction("Index");
    }
}