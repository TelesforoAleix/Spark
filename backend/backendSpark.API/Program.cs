using backendSpark.API.Middleware;
using backendSpark.Model.Entities;
using backendSpark.Model.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// For each repository created add the following line in program file to make the respository available to API Controller through dependency injection. 
builder.Services.AddScoped<EventRepository, EventRepository>();
builder.Services.AddScoped<AttendeeRepository, AttendeeRepository>();
builder.Services.AddScoped<MeetingRepository, MeetingRepository>();


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

app.MapControllers();
app.Run();

