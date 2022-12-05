using AliBayat.Concurrency.Api.Data;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicatrionDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaltConnection")));

builder.Services.AddSingleton<IDistributedLockProvider>(sp =>
{
    var connection = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"));
    return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/payment", async (ApplicatrionDbContext applicatrionDbContext, IDistributedLockProvider synchronizationProvider, int amount) =>
{
    var id = 1;
    var myDistributedLock = synchronizationProvider.CreateLock($"User{id}");
    using (myDistributedLock.Acquire())
    {
        var balance = applicatrionDbContext.Users.AsNoTracking().Select(x => x.Balance).First();
        if (balance >= amount)
        {
            await Task.Delay(1000);
            var user = applicatrionDbContext.Users.First(x => x.Id == id);
            user.Balance -= amount;
            user.Withdrawn += amount;

           await applicatrionDbContext.SaveChangesAsync();
        }
    }

    return Results.Ok();
})
.WithName("payment")
.WithOpenApi();

app.Run();