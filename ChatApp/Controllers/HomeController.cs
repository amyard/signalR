using System.Diagnostics;
using System.Security.Claims;
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
        var chats = _ctx.Chats
            .Include(x => x.Users)
            .Where(x => x.Users.Any(y => y.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value))
            .ToList();
        
        return View(chats);
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
        var users = new List<ChatUser>()
        {
            new ChatUser
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                UserRole = UserRole.Admin
            }
        };
        
        var chat = new Chat()
        {
            Name = name,
            Type = ChatType.Room
        };
        chat.Users = users;

        _ctx.Chats.Add(chat);
        
        await _ctx.SaveChangesAsync();
        
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> JoinChat(int chatId)
    {
        var chatUser = new ChatUser()
        {
            ChatId = chatId,
            UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
            UserRole = UserRole.Member
        };
        
        _ctx.ChatUsers.Add(chatUser);
        
        await _ctx.SaveChangesAsync();
        
        return RedirectToAction("Chat", "Home", new {id = chatId});
    }
}