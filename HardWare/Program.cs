using System.Text.Encodings.Web;
using System.Text.Json;

using HardWare;
using HardWare.Enums;

//args = new[] { "f:Fan" };
ArgumentHandler argument = new(args);
argument.SetArguments();
if (argument.Mode is ModeEnum.Help)
{
    Console.WriteLine(argument.Help());
    return;
}

string path;
try
{
    using StreamReader sr = new("appsettings.json", new FileStreamOptions { Access = FileAccess.Read });
    string json = sr.ReadToEnd();
    Settings? settings = JsonSerializer.Deserialize<Settings>(json);
    path = settings?.PathData ?? "data.json";
}
catch
{
    Console.WriteLine("Не найден файл настроек, либо не указан путь к data.json");
    return;
}


Io io = new();
bool isRead = io.Read(path, out string jsonSource);
if (!isRead) { return; }
List<SensorModel> sensorModels = JsonSerializer.Deserialize<List<SensorModel>>(jsonSource) ?? new();

string source = argument.Mode switch
{
    ModeEnum.Key => sensorModels.FirstOrDefault(x => x.Key == argument.Key)?.Value ?? "Ключ не найден",
    ModeEnum.Filter => ((Func<List<SensorModel>, string>)((List<SensorModel> sensors) =>
    {
        JsonSerializerOptions jsonSerializerOptions = new() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
        sensors = sensors.Where(sensor => argument.Filters.Contains(sensor.Type)).ToList();
        return JsonSerializer.Serialize(sensors, jsonSerializerOptions);
    }))(sensorModels),
    _ => argument.Help()
};
Console.WriteLine(source);