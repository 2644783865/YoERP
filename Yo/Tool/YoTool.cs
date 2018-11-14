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

        static public bool ParseInt(ref object value) {
            bool result = true;
            while (true) {
                if(value is string) {
                    if(string.IsNullOrWhiteSpace(value as string)) {
                        value = 0;
                        break;
                    }
                }

                try {
                    value = Convert.ToInt32(value);
                }
                catch {
                    result = false;
                }

                break;
            }
            return result;
        }

        static public bool ParseDouble(ref object value) {
            bool result = true;
            while (true) {
                if (value is string) {
                    if (string.IsNullOrWhiteSpace(value as string)) {
                        value = 0;
                        break;
                    }
                }

                try {
                    value = Convert.ToDouble(value);
                }
                catch {
                    result = false;
                }

                break;
            }
            return result;
        }

        static public bool ParseString(ref object value) {
            bool result = true;
            while (true) {
                if (value == null) {
                    value = "";
                    break;
                }

                try {
                    value = value.ToString().Trim();
                }
                catch {
                    result = false;
                }

                break;
            }
            return result;
        }

        static public bool ParseDatetime(ref object value) {
            bool result = true;
            while (true) {
                if (value == null) {
                    result = false;
                    break;
                }

                try {
                    value = Convert.ToDateTime(value);
                }
                catch {
                    result = false;
                }

                break;
            }
            return result;
        }

    }

}
