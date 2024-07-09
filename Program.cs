using ApiIdentityEndpoint.Data;
using ApiIdentityEndpoint.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=databse.db"));

// builder.Services.AddAuthentication();
builder.Services.AddAuthorization(); // A partir da versão 8 essa linha traz automaticamente o AddAuthentication

builder.Services
    .AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapSwagger();

app.MapGet("/", () => "Hello World!").RequireAuthorization();

app.MapIdentityApi<User>();

app.MapPost("/logout", async (SignInManager<User> signInManager, [FromBody] object empty) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok();
});

app.Run();
