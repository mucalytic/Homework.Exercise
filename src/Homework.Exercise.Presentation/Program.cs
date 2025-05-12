using Microsoft.Extensions.Configuration;
using Homework.Exercise.Infrastructure;
using Homework.Exercise.Presentation;
using Homework.Exercise.Application;
using Microsoft.Extensions.Hosting;
using Serilog;

await Host.CreateDefaultBuilder(args)
          .ConfigureAppConfiguration((_, config) =>
          {
              config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
          })
          .UseSerilog((ctx, _, config) =>
          {
              config.ReadFrom.Configuration(ctx.Configuration);
          })
          .ConfigureServices((ctx, services) =>
          {
              services.AddPresentation(ctx);
              services.AddApplication();
              services.AddInfrastructure();
          })
          .Build()
          .StartAsync();
