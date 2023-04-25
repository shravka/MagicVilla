namespace MagicVilla_VillaAPI.Logging
{
    public class Logging : ILogging
    {
        public void Log(string message, string type)
        {
          if(type=="error")
                Console.WriteLine($"{message} {type}");
          Console.WriteLine(message);
        }
    }

    public interface ILogging
    {
        void Log(string message,string error);
      
    }
}
