using HardWare.Worker.Enums;
using LibreHardwareMonitor.Hardware;
using System.Text.RegularExpressions;

namespace HardWare.Worker;

public sealed class SensorModel
{
    public string Name { get; set; }
    public TypeEnum Type { get; set; }
    public string Value { get; set; }
    public string ValueType { get; set; }
    public string Key { get; set; }

    public SensorModel(ISensor sensor)
    {
        Name = sensor.Name;
        Type = Enum.TryParse($"{sensor.SensorType}", out TypeEnum type)
            ? type
            : TypeEnum.Unknown;

        Value = sensor.Value.ToString() ?? string.Empty;

        ValueType = Type switch
        {
            TypeEnum.Voltage => "V",
            TypeEnum.Temperature => "°C",
            TypeEnum.Fan or TypeEnum.Control => "RPM",
            TypeEnum.Load => "%",
            TypeEnum.Power => "W",
            TypeEnum.SmallData => "MB",
            TypeEnum.Clock => "MHz",
            TypeEnum.Data => "GB",
            _ => string.Empty
        };

        var regex = new Regex("[/\\\\-]");
        Key = regex.Replace(sensor.Identifier.ToString(), string.Empty);
    }
}