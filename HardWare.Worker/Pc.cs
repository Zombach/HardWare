using LibreHardwareMonitor.Hardware;

namespace HardWare.Worker;

public class Pc
{
    private readonly Computer _computer;
    public Pc()
    {
        _computer = new()
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true,
            IsMemoryEnabled = true,
            IsMotherboardEnabled = true,
            IsStorageEnabled = true,
            IsBatteryEnabled = true,
            IsPsuEnabled = false,
            IsNetworkEnabled = false,
            IsControllerEnabled = false
        };
    }

    public void Start()
    {
        _computer.Open();
        _computer.Accept(new UpdateVisitor());
    }

    public List<IHardware> GetHardware() => _computer.Hardware.ToList();

    public void Close() => _computer.Close();
}