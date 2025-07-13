using HardWare;
using HardWare.Enums;
using System.Text.Encodings.Web;
using System.Text.Json;

//args = ["f:Fan"];
//args = ["k:gpunvidia0power0"];
args = ["v"];
var argument = new ArgumentHandler(args.ToList());
argument.SetArguments();

if (argument.Mode is Modes.Help)
{
    Console.WriteLine(argument.Help());
    return;
}

Settings settings;
try
{
    using var sr = new StreamReader("appsettings.json", new FileStreamOptions { Access = FileAccess.Read });
    var json = sr.ReadToEnd();
    settings = JsonSerializer.Deserialize<Settings>(json)
               ?? throw new ArgumentNullException("settings", "Не найден или не валидно заполнен appsettings.json");
}
catch (Exception exception)
{
    Console.WriteLine(exception.Message);
    return;
}

if (argument.Mode is Modes.Version)
{
    Console.WriteLine(settings.Version);
    return;
}

var io = new Io();
var path = string.IsNullOrWhiteSpace(settings.PathData)
    ? "data.json"
    : settings.PathData;

var isRead = io.Read(path, out var jsonSource);
if (!isRead)
{
    return;
}

var sensorModels = JsonSerializer.Deserialize<List<SensorModel>>(jsonSource) ?? new();

var source = argument.Mode switch
{
    Modes.Key => sensorModels.FirstOrDefault(x => x.Key == argument.Key)?.Value ?? "Ключ не найден",
    Modes.Filter => ((Func<List<SensorModel>, string>)(sensors =>
    {
        var jsonSerializerOptions = new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
        sensors = sensors.Where(sensor => argument.Filters.Contains(sensor.Type)).ToList();
        return JsonSerializer.Serialize(sensors, jsonSerializerOptions);
    }))(sensorModels),
    _ => argument.Help()
};

Console.WriteLine(source);