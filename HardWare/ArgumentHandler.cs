using System.Text;
using System.Text.RegularExpressions;
using HardWare.Enums;

namespace HardWare;

public class ArgumentHandler
{
    private const string K = "k";
    private const string F = "f";
    private readonly List<string> _arguments;
    public ModeEnum Mode { get; private set; }
    public string Key { get; private set; }
    public List<ValueType> Filters { get; private set; }

    public ArgumentHandler(IEnumerable<string> argument)
    {
        _arguments = argument.ToList();
        Mode = ModeEnum.Help;
        Filters = new List<ValueType>();
        Key = string.Empty;
    }

    public void SetArguments()
    {
        if (_arguments.Count is 0) { return; }
        Regex regex = new("[/\\\\-]");
        string arguments = string.Join("|", _arguments).ToLower();
        arguments = regex.Replace(arguments, string.Empty);

        if (arguments.StartsWith(K) && arguments.Contains(':'))
        {
            int index = arguments.IndexOf(':') + 1;
            Key = arguments[index..];
            Mode = ModeEnum.Key;
        }
        else if (arguments.StartsWith(F))
        {
            List<ValueType> filter = new();
            if (IsUse(ref arguments, TypeEnum.Temperature, out TypeEnum result)) { filter.Add(result); }
            if (IsUse(ref arguments, TypeEnum.Fan, out result)) { filter.Add(result); }
            if (IsUse(ref arguments, TypeEnum.Voltage, out result)) { filter.Add(result); }
            if (IsUse(ref arguments, TypeEnum.Load, out result)) { filter.Add(result); }
            if (IsUse(ref arguments, TypeEnum.Data, out result)) { filter.Add(result); }
            if (IsUse(ref arguments, TypeEnum.SmallData, out result)) { filter.Add(result); }
            if (IsUse(ref arguments, TypeEnum.Power, out result)) { filter.Add(result); }
            if (IsUse(ref arguments, TypeEnum.Control, out result)) { filter.Add(result); }
            if (IsUse(ref arguments, TypeEnum.Clock, out result)) { filter.Add(result); }
            Filters = filter;
            Mode = ModeEnum.Filter;
        }
    }

    public string Help()
    {
        StringBuilder sb = new("Аргументы можно указать через символы / \\ или - либо без символов");
        sb = sb.Append("Язык ENG, Регистр не имеет значения");
        sb = sb.Append("Аргументы");
        sb = sb.Append("K или k: ключ");
        sb = sb.Append("F или f: фильтр");
        sb = sb.Append("Пример указания аргументов");
        sb = sb.Append(".\\HardWare.exe k:cpu01");
        return $"{sb}";
    }

    private bool IsUse(ref string value, TypeEnum type, out TypeEnum result)
    {
        result = TypeEnum.Unknown;
        if (!value.Contains($"{type}", StringComparison.OrdinalIgnoreCase)) return false;
        result = type;
        return true;
    }
}