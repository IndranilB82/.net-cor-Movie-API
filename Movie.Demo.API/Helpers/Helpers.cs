using Newtonsoft.Json;
using Serilog.Events;
using System;
using System.Globalization;
using Serilog;
using System.Runtime.CompilerServices;

namespace Movie.Demo.API.Helpers
{
    public static class Helper
    {
        public static string LogContextControllerInfo(dynamic input)
        {
            string data = (ConfigLogParams.LogLevel.MinimumLevel == LogEventLevel.Debug) ? OperateData(input) : OperateInfoData(input);
            return string.Format(CultureInfo.InvariantCulture, ":{{\"Input\":{0}}}{1}", data, Environment.NewLine);
        }

        public static string LogContextErrorInfo(string layer, string method,
            dynamic input, string error, dynamic innerException, dynamic stackTrace)
        {
            string data;
            try
            {
                data = OperateData(input);
            }
            catch (Exception ex)
            {
                data = string.Format("Failed to Deserialize the Object {0} and exception message: {1}", input.GetType().Name, ex.GetBaseException().ToString());
            }
            return string.Format(CultureInfo.InvariantCulture, ":{{\"Layer\":\"{0}\",\"Method Name\":{1},\"Input\":{2},\"ErrorMessage\":\"{3}\",\"InnerException\":\"{4}\",\"StackTrace\":\"{5}\"}}{6}", layer, method, data, error, innerException, stackTrace, Environment.NewLine);
        }

        private static string OperateData(dynamic entity)
        {
            string result = string.Empty;
            try
            {
                if (entity != null)
                {
                    dynamic entityName = entity.GetType().Name;

                    switch (entityName)
                    {
                        case "String":
                            result = entity;
                            break;
                        default:
                            result = JsonConvert.SerializeObject(entity);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                string inputName = (entity != null) ? entity.GetType().Name : string.Empty;
                result = string.Format("Failed to Deserialize the Object {0} and exception message: {1}", inputName, ex.GetBaseException());
            }

            return result;
        }

        private static string OperateInfoData(dynamic entity)
        {
            string result = string.Empty;
            if (entity != null)
            {
                dynamic entityName = entity.GetType().Name;

                switch (entityName)
                {
                    case "String":
                        result = entity;
                        break;
                    default:
                        result = entityName;
                        break;
                }
            }
            return result;
        }
        public static ILogger Here(this ILogger logger,
  [CallerMemberName] string memberName = "",
  [CallerFilePath] string sourceFilePath = "",
  [CallerLineNumber] int sourceLineNumber = 0)
        {
            string layer = !string.IsNullOrEmpty(sourceFilePath) ? sourceFilePath.Substring(sourceFilePath.LastIndexOf("/", StringComparison.InvariantCulture) + 1, sourceFilePath.LastIndexOf(".", StringComparison.InvariantCulture) - sourceFilePath.LastIndexOf("/", StringComparison.InvariantCulture)) : string.Empty;
            logger.Information(string.Format(CultureInfo.InvariantCulture, "Calling Method {0} of Layer {1} at line number {2}", memberName, layer, sourceLineNumber));
            return logger
                .ForContext("MemberName", memberName)
                .ForContext("FilePath", sourceFilePath)
                .ForContext("LineNumber", sourceLineNumber);
        }

    }
}
