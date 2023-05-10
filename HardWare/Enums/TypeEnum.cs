using System.Text.Json.Serialization;

namespace HardWare.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TypeEnum
{
    Unknown = 0,
    Temperature = 1,
    Fan = 2,
    Voltage = 3,
    Load = 4,
    Data = 5,
    SmallData = 6,
    Power = 7,
    Control = 8,
    Clock = 9,
    Current = 10,
    Frequency = 11,
    Flow = 12,
    Level = 13,
    Factor = 14,
    Throughput = 15,
    TimeSpan = 16,
    Energy = 17,
    Noise = 18,
}