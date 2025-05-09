using backendSpark.Model.Entities;
using backendSpark.Model.Repositories;
using backendSpark.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// For each repository created add the following line in program file to make the respository available to API Controller through dependency injection. 
builder.Services.AddScoped<EventRepository>();
builder.Services.AddScoped<AttendeeRepository, AttendeeRepository>();
builder.Services.AddScoped<MeetingRepository, MeetingRepository>();

builder.Services.AddScoped<UserRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<BasicAuthenticationMiddleware>();
// This is disabled because we are not using SSL certificate on our solution.
// app.UseHttpsRedirection();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();

app.MapControllers();
app.Run();

