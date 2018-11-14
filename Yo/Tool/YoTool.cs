using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Yo
{
    public class YoTool
    {
        static public JObject GetJObject(string content) {
            JObject result = null;
            while (true) {
                if (string.IsNullOrEmpty(content)) {
                    break;
                }

                content = "{" + content + "}";

                try {
                    result = JsonConvert.DeserializeObject(content) as JObject;
                }
                catch { }

                break;
            }

            return result;
        }

    }

}
