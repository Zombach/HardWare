namespace HardWare;

public class Io
{
    public bool Read(string path, out string json)
    {
        try
        {
            using StreamReader sr = new(path, new FileStreamOptions { Access = FileAccess.Read });
            json = sr.ReadToEnd();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            json = string.Empty;
            return false;
        }
    }
}