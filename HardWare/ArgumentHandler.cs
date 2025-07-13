using HardWare.Enums;
using System.Text;
using System.Text.RegularExpressions;

namespace HardWare;

public class ArgumentHandler(List<string> arguments)
{
    private const string K = "k";
    private const string F = "f";
    private const string V = "v";

    public Modes Mode { get; private set; } = Modes.Help;
    public string Key { get; private set; } = string.Empty;
    public List<ValueType> Filters { get; private set; } = new();

    public void SetArguments()
    {
        if (arguments.Count is 0)
        {
            return;
        }

        var regex = new Regex("[/\\\\-]");
        var argument = string.Join("|", arguments).ToLower();
        argument = regex.Replace(argument, string.Empty);

        if (argument.StartsWith(K) && argument.Contains(':'))
        {
            var index = argument.IndexOf(':') + 1;
            Key = argument[index..];
            Mode = Modes.Key;
        }
        else if (argument.StartsWith(F))
        {
            var filter = new List<ValueType>();
            if (IsUse(ref argument, Types.Temperature, out var result)) { filter.Add(result); }
            if (IsUse(ref argument, Types.Fan, out result)) { filter.Add(result); }
            if (IsUse(ref argument, Types.Voltage, out result)) { filter.Add(result); }
            if (IsUse(ref argument, Types.Load, out result)) { filter.Add(result); }
            if (IsUse(ref argument, Types.Data, out result)) { filter.Add(result); }
            if (IsUse(ref argument, Types.SmallData, out result)) { filter.Add(result); }
            if (IsUse(ref argument, Types.Power, out result)) { filter.Add(result); }
            if (IsUse(ref argument, Types.Control, out result)) { filter.Add(result); }
            if (IsUse(ref argument, Types.Clock, out result)) { filter.Add(result); }
            Filters = filter;
            Mode = Modes.Filter;
        }
        else if (argument.StartsWith(V))
        {
            Mode = Modes.Version;
        }
    }

    public string Help()
    {
        var sb = new StringBuilder("Аргументы можно указать через символы / \\ или - либо без символов");
        sb = sb.Append("Язык ENG, Регистр не имеет значения");
        sb = sb.Append("Аргументы");
        sb = sb.Append("F или f: фильтр - по общей дате, например Fan, отобразит список всех вентиляторов");
        sb = sb.Append("K или k: ключ - достает значение метрики по ключу");
        sb = sb.Append("V или v: версия библиотеки LibreHardwareMonitorLib");
        sb = sb.Append("Пример указания аргументов");
        sb = sb.Append(".\\HardWare.exe k:cpu01");
        return $"{sb}";
    }

    private bool IsUse(ref string arguments, Types type, out Types result)
    {
        result = Types.Unknown;
        if (!arguments.Contains($"{type}", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        result = type;
        return true;
    }
}