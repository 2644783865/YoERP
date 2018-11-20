using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Yo
{
    public class YoConvert
    {
        static public bool ToInt(ref object value) {
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

        static public bool ToDouble(ref object value) {
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

        static public bool ToString(ref object value) {
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

        static public bool ToDatetime(ref object value) {
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

        static public JObject Text2Json(string content) {
            JObject result = null;
            while (true) {
                if (string.IsNullOrWhiteSpace(content)) {
                    break;
                }

                content = content.Trim();
                if (!content.StartsWith("{")) {
                    content = "{" + content + "}";
                }

                try {
                    result = JsonConvert.DeserializeObject(content) as JObject;
                }
                catch { }

                break;
            }
            return result;
        }

        static public string Json2Text(JObject obj) {
            string result = null;
            while (true) {
                if (obj == null) {
                    break;
                }

                try {
                    result = JsonConvert.SerializeObject(obj);
                }
                catch { }

                break;
            }
            return result;
        }

    }

}
