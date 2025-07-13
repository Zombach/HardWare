namespace HardWare.Worker;

public sealed class Worker(HardWare hardWare, ILogger<Worker> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var pc = new Pc();
        while (!stoppingToken.IsCancellationRequested)
        {
            pc.Start();
            var isDone = hardWare.Start(pc.GetHardware());
            logger.LogInformation($"update: {isDone}");
            await Task.Delay(hardWare.Delay, stoppingToken);
        }
        pc.Close();
    }
}