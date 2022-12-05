
# DistributedLock sample
With [DistributedLock](https://github.com/madelson/DistributedLock), synchronizing access to a region of code across multiple applications/machines is as simple as

    var myDistributedLock = synchronizationProvider.CreateLock($"name");
    using (myDistributedLock.Acquire())
    {

    }
For applications that use dependency injection, DistributedLock's providers make it easy to separate out the specification of a lock's name from its other settings. For example in an ASP.NET Core app you might do:

    builder.Services.AddSingleton<IDistributedLockProvider>(sp =>
    {
        var connection = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"));
        return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
    });

## Getting Started
Make sure you have installed **.NET 7** in your environment. After that, you can run the below commands from the **AliBayat.Concurrency.Api** directory.

    dotnet run
    
### Dependencies

* .NET 7
* SQL Server
* Redis

### Executing program

* Download & Install **.NET 7** from [here](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
* Configure connection strings
* Run the project

## Contributors
- [Dariush Tasdighi](https://www.linkedin.com/in/Tasdighi/)
- [Ali Bayat](https://www.linkedin.com/in/AliBayatgh)
