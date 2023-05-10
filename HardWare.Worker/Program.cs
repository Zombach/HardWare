using HardWare.Worker;

IHost host = Host.CreateDefaultBuilder(args).ConfigureServices(services =>
{
    services.AddWindowsService(options => { options.ServiceName = ".NET HardWare Service"; });
    services.AddHostedService<Worker>();
    services.AddSingleton<HardWare.Worker.HardWare>();
}).Build();

host.Run();