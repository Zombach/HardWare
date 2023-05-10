using System.Text.Encodings.Web;
using System.Text.Json;

using HardWare.Worker.Enums;

using LibreHardwareMonitor.Hardware;

namespace HardWare.Worker;

public class HardWare
{
    private readonly string _path;
    private readonly Io _io;
    public int Delay { get; private set; }
    public HardWare()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        _path = configuration.GetValue<string>("PathData") ?? "data.json";
        string delay = configuration.GetValue<string>("Delay") ?? "5000";
        Delay = int.TryParse(delay, out int result) ? result: 5000;
        _io = new();
    }

    public bool Start(List<IHardware> hardwares)
    {
        List<SensorModel> models = new();

        hardwares.ForEach(hardware =>
        {
            hardware.SubHardware.ToList().ForEach(subHardware =>
            {
                subHardware.Sensors.ToList().ForEach(sensor =>
                {
                    SensorModel sensorModel = new(sensor);
                    models.Add(sensorModel);
                });
            });
            hardware.Sensors.ToList().ForEach(sensor =>
            {
                SensorModel sensorModel = new(sensor);
                models.Add(sensorModel);
            });
        });

        models = models.Where(model => model.Value != string.Empty && model.Type != TypeEnum.Throughput && model.Type != TypeEnum.Level).ToList();
        models.Sort(new Comparer());
        string json = JsonSerializer.Serialize(models, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
        return _io.Write(_path, json);
    }
}