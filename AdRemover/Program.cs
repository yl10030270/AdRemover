using System.Web;
using AdRemover.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var blockList = await BlockListParser.Parse(@".\Data\blocklist.txt");
builder.Services.AddSingleton<IUrlCleaner, UrlCleaner>(_ => new UrlCleaner(blockList));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapGet("/removead", ([FromQuery] string url, [FromServices] IUrlCleaner remover) =>
{
    var resultHtml = remover.RemoveFromWebPage(new Uri(HttpUtility.UrlDecode(url)));
    return Results.Content(resultHtml, "text/html");
});


app.Run();

