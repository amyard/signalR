using System.Diagnostics;
using ChatApp.Context;
using ChatApp.Enums;
using Microsoft.AspNetCore.Mvc;
using ChatApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Controllers;

[Authorize]
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

    [HttpGet("{id:int}")]
    public IActionResult Chat(int id)
    {
        var chat = _ctx.Chats
            .Include(x => x.Messages)
            .FirstOrDefault(x => x.Id == id);
        return View(chat);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateMessage(int chatId, string message)
    {
        var messageModel = new Message()
        {
            ChatId = chatId,
            Text = message,
            Name = User?.Identity?.Name ?? "Default"
        };

        await _ctx.Messages.AddAsync(messageModel);
        await _ctx.SaveChangesAsync();
        
        return RedirectToAction("Chat", new {id = chatId});
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