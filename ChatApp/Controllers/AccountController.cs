﻿using ChatApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }
    
    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string userName, string password)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
        }

        
        return RedirectToAction("Login", "Account");
    }
    
    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(string userName, string password)
    {
        var user = new User()
        {
            UserName = userName
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            
            return RedirectToAction("Index", "Home");
        }
        
        return RedirectToAction("Register", "Account");
    }
    
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }
}