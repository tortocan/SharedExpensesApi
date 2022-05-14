using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SharedExpenses.Storage;
using SharedExpenses.Storage.Abstraction;
using SharedExpenses.Storage.Implementation;
using SharedExpenses.Storage.Models;

static void InsertData(WebApplication app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        using (var context = scope.ServiceProvider.GetService<ApplicationDbContext>())
        {
            // Creates the database if not exists
            context.Database.EnsureCreated();
            var expenseGroup = new ExpenseGroup{
                Id = 1
            };

            var users = new List<ApplicationUser>{
                new ApplicationUser
                {
                    Id = 1,
                    ExpenseGroup = new List<ExpenseGroup>(){expenseGroup},
                    FullName = "Francisco Buyo"
                },
                new ApplicationUser {
                    Id = 2,
                    ExpenseGroup = new List<ExpenseGroup>(){expenseGroup},
                    FullName = "Alfonso Pérez"
                },
                new ApplicationUser {
                    Id = 3,
                    ExpenseGroup = new List<ExpenseGroup>(){expenseGroup},
                    FullName = "Raúl González"
                },
                new ApplicationUser {
                    Id = 4,
                    ExpenseGroup = new List<ExpenseGroup>(){expenseGroup},
                    FullName = "José María Gutiérrez"
                },
                new ApplicationUser {
                    Id = 5,
                    FullName = "George Tortocan"
                }
            };
            if (!context.User.Any())
            {
                context.User.AddRange(users);
                context.SaveChanges();
            }
            var expenses = new List<Expense>() {
                new Expense {
                    Payment = new Payment {
                        Amount = 100,
                        Description ="Cena",
                        Date= DateTime.UtcNow.AddMinutes(-2)
                    },
                    User = users[0],// Francisco Buyo
                    ExpenseGroup = expenseGroup
                },
                new Expense {
                    Payment = new Payment {
                        Amount = 10,
                        Description ="Taxi",
                        Date= DateTime.UtcNow.AddHours(-22)
                    },
                    User = users[1],// Alfonso Pérez
                    ExpenseGroup = expenseGroup
                },
                new Expense {
                    Payment = new Payment {
                        Amount = 53.40,
                        Description ="Compra",
                        Date= DateTime.UtcNow.AddDays(-1)
                    },
                    User = users[1],// Alfonso Pérez"
                    ExpenseGroup = expenseGroup
                }
            };

            if (!context.Expense.Any())
            {
                context.Expense.AddRange(expenses);
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
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IExpensesService, ExpensesService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers().AddControllersAsServices().AddJsonOptions(jsonOptions =>
        {
            jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
        });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();
InsertData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
