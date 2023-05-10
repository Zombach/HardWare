namespace HardWare.Worker;

public class Comparer : IComparer<SensorModel>
{
    public int Compare(SensorModel? first, SensorModel? second)
    {
        if (ReferenceEquals(first, second)) return 0;
        if (ReferenceEquals(null, second)) return 1;
        if (ReferenceEquals(null, first)) return -1;
        if (first.Type == second.Type) return 0;
        if (first.Type < second.Type) return 1;
        if (first.Type > second.Type) return -1;
        return 0;
    }
}