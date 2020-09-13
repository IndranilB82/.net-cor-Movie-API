using Serilog.Core;

namespace Movie.Demo.API.Helpers
{
    public class ConfigLogParams
    {
        static ConfigLogParams()
        {
            LogLevel = new LoggingLevelSwitch(Serilog.Events.LogEventLevel.Information);
        }

        public static LoggingLevelSwitch LogLevel { get; set; }
    }
}
