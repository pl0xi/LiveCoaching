using Web_Application.Services;
using Web_Application.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient<IRiotService, RiotService>(client =>
{
 client.BaseAddress = new Uri("https://europe.api.riotgames.com");
 client.DefaultRequestHeaders.Add("X-Riot-Token", builder.Configuration["Riot:Token"]);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
 app.UseDeveloperExceptionPage();
}
else
{
 app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();