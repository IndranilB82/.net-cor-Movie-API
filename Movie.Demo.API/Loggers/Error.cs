using Newtonsoft.Json;
using System.Collections.Generic;

namespace Movie.Demo.API.Loggers
{
    public class Error
    {
        public int StatusCode { get; set; }
        public Dictionary<string, object> ErrorResponse { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
