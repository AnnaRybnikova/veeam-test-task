// See https://aka.ms/new-console-template for more information

using CommandLine;
using FoldersSynchronization.Cli;
using FoldersSynchronization.Core;
using Microsoft.Extensions.Logging;
using Serilog;

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSerilog(new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File("logs.txt")
        .CreateLogger());
});
var synchronizer = new FoldersSynchronizer(loggerFactory.CreateLogger<FoldersSynchronizer>());

await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(async options =>
{
    await synchronizer.SynchronizeAsync(options.From, options.To);

    if (options.Interval.HasValue)
    {
        var periodicTimer = new PeriodicTimer(options.Interval.Value);

        while (await periodicTimer.WaitForNextTickAsync())
        {
            await synchronizer.SynchronizeAsync(options.From, options.To);
        }
    }
});