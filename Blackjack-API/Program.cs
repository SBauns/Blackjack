var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddSingleton<BlackjackAPI.Services.GameService>();

builder.Services.AddSignalR();

// Add CORS policy TODO actually create policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigin", policy =>
    {
    policy.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapHub<BlackjackAPI.Services.DealerHub>("/dealerhub");
app.UseRouting();
app.MapControllers();
app.UseHttpsRedirection();
app.UseCors("AllowAllOrigin");

app.Run();
