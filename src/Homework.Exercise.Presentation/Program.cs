using Homework.Exercise.Infrastructure;
using Homework.Exercise.Application;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
          .ConfigureServices((_, services) =>
           {
               services.AddApplication();
               services.AddInfrastructure();
           })
          .Build()
          .StartAsync();
