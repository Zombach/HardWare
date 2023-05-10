namespace HardWare.Worker
{
    public class Worker : BackgroundService
    {
        private readonly HardWare _hardWare;
        private readonly ILogger<Worker> _logger;

        public Worker(HardWare hardWare, ILogger<Worker> logger)
        {
            _hardWare = hardWare;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Pc pc = new();
            while (!stoppingToken.IsCancellationRequested)
            {
                pc.Start();
                bool isDone = _hardWare.Start(pc.GetHardware()); 
                _logger.LogInformation($"update: {isDone}");
                await Task.Delay(_hardWare.Delay, stoppingToken);
            }
            pc.Close();
        }
    }
}