using HardWare.Enums;

namespace HardWare;

public sealed class SensorModel
{
    public string Name { get; set; } = string.Empty;
    public Types Type { get; set; } = Types.Unknown;
    public string Value { get; set; } = string.Empty;
    public string ValueType { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
}