using System.Security.Claims;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

List<Merchendise> merchRepo = new List<Merchendise>()
{
    new() {Series = "HP", MerchName = "Statue", Description = "Stock", MerchPrice = 5.99}
};

app.MapGet("merch", () => merchRepo);

app.MapPost("merch", (Merchendise m) => merchRepo.Add(m));

app.MapPut("merch", (MerchendiseUpdateDTO dto) =>
{
    var game = merchRepo.FirstOrDefault(o => o.MerchName == dto.MerchName);
    if (game != null)
    {
        game.Description = dto.Description;
        game.MerchPrice = dto.MerchPrice;
        return Results.Json(game);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapDelete("merch", (string merchName) =>
{
    Merchendise? merch = merchRepo.FirstOrDefault(o => o.MerchName == merchName);
    if (merch != null) 
    { 
        merchRepo.Remove(merch);
        return Results.Json(merch);
    }
    else
    {
        return Results.NotFound();
    }
});

app.Run();

record class MerchendiseUpdateDTO(string MerchName, string Description, double MerchPrice);

public class Merchendise
{
    public string Series { get; set; }
    public string MerchName { get; set; }
    public string Description { get; set; }
    public double MerchPrice { get; set; }
}
