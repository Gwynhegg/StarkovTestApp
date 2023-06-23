
namespace StarkovTestApp.Services
{
    public class Logger
    {
        private static Logger _instance;
        private List<string> _messages = new List<string>();

        public static Logger Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Logger();

                return _instance;
            }
        }

        public void Log(string message) => _messages.Add(message);

        public string  GetLog() => String.Join("\n", _messages);

        public void Clear() => _messages.Clear();
    }
}
