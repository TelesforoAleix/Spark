using backendSpark.API.Middleware;
using backendSpark.Model.Entities;
using backendSpark.Model.Repositories;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// For each repository created add the following line in program file to make the respository available to API Controller through dependency injection. 
builder.Services.AddScoped<EventRepository, EventRepository>();
builder.Services.AddScoped<AttendeeRepository, AttendeeRepository>();
builder.Services.AddScoped<MeetingRepository, MeetingRepository>();
builder.Services.AddScoped<UserRepository, UserRepository>();
builder.Services.AddScoped<BaseRepository, BaseRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

// This is disabled because we are not using SSL certificate on our solution.
// app.UseHttpsRedirection();

// This is the middleware for Header Authentication. Uncomment the line below to use it.
// app.UseHeaderAuthenticationMiddleware();
app.UseBasicAuthenticationMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

public partial class Program { }