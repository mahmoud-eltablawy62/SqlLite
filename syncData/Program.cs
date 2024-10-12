using Microsoft.EntityFrameworkCore;
using syncData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.





builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<userContext>
(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddDbContext<UserContextSqlLite>(options =>
       options.UseSqlite("Data Source=UserContextSqlLite.db"));

builder.Services.AddScoped<LocalStorageService>();

builder.Services.AddHostedService<ICheckServices>(); 

builder.Services.AddCors(options =>
{

    options.AddDefaultPolicy(builder =>
    {

        builder.AllowAnyOrigin()
       .AllowAnyHeader()
       .AllowAnyMethod();

    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
