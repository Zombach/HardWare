namespace HardWare.Worker;

public class Io
{
    public bool Write(string path, string json)
    {
        try
        {
            using StreamWriter sw = new(path, false);
            sw.WriteLine(json);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}