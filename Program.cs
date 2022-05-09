using Microsoft.EntityFrameworkCore;
using SharedExpensesApi;
using SharedExpensesApi.Models;

static void InsertData(WebApplication app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        using (var context = scope.ServiceProvider.GetService<ApplicationDbContext>())
        {
            // Creates the database if not exists
            context.Database.EnsureCreated();

            if (!context.User.Any())
            {
                // Adds a users
                context.User.Add(new ApplicationUser
                {
                    Id = 1,
                    FullName = "Mariner Books"
                });

                // Saves changes
                context.SaveChanges();
            }
        }
    }
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
InsertData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
