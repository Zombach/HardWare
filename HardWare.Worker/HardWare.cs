using HardWare.Worker.Enums;
using LibreHardwareMonitor.Hardware;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace HardWare.Worker;

public sealed class HardWare
{
    private readonly string _path;
    private readonly Io _io;

    public int Delay { get; private set; }

    public HardWare()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        _path = configuration.GetValue<string>("PathData") ?? "data.json";

        var delay = configuration.GetValue<string>("Delay") ?? "5000";

        Delay = int.TryParse(delay, out int result)
            ? result
            : 5000;

        _io = new();
    }

    public bool Start(List<IHardware> hardwares)
    {
        var models = new List<SensorModel>();

        hardwares.ForEach(hardware =>
        {
            hardware.SubHardware
                .ToList()
                .ForEach(subHardware =>
                {
                    subHardware.Sensors
                        .ToList()
                        .ForEach(sensor =>
                        {
                            var sensorModel = new SensorModel(sensor);
                            models.Add(sensorModel);
                        });
                });

            hardware.Sensors
                .ToList()
                .ForEach(sensor =>
                {
                    var sensorModel = new SensorModel(sensor);
                    models.Add(sensorModel);
                });
        });

        models = models.Where(model => model.Value != string.Empty
               && model.Type is not TypeEnum.Throughput
               && model.Type is not TypeEnum.Level)
            .ToList();

        models.Sort(new Comparer());
        var json = JsonSerializer.Serialize(models, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });

        return _io.Write(_path, json);
    }
}