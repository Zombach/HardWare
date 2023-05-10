using HardWare.Enums;

namespace HardWare;

public class SensorModel
{
    public string Name { get; set; }
    public TypeEnum Type { get; set; }
    public string Value { get; set; }
    public string ValueType { get; set; }
    public string Key { get; set; }

    public SensorModel()
    {
        Name = string.Empty;
        Type = TypeEnum.Unknown;
        Value = string.Empty;
        ValueType = string.Empty;
        Key = string.Empty;
    }
}