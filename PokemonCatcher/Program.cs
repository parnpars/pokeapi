using PokemonCatcher;
using PokemonCatcher.Interfaces;
using PokemonCatcher.Models;
using PokemonCatcher.Services;

var appSettings = new AppSettings();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Configuration.Bind(appSettings);
builder.Services.AddTransient<HttpClient>();
builder.Services.AddSingleton<PokeApiService>();
builder.Services.AddTransient<IPokemonService, PokemonService>();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();