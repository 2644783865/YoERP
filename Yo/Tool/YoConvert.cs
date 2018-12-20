using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Yo
{
    public class YoConvert
    {
        static public bool ToBool(ref object value) {
            bool result = true;
            while (true) {
                if (value is string) {
                    if (string.IsNullOrWhiteSpace(value as string)) {
                        value = false;
                        break;
                    }
                }

                try {
                    value = Convert.ToBoolean(value);
                }
                catch {
                    result = false;
                }

                break;
            }
            return result;
        }

        static public bool ToInt(ref object value) {
            bool result = true;
            while (true) {
                if (value is string) {
                    if (string.IsNullOrWhiteSpace(value as string)) {
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

        static public string ToMD5(string str) {
            if (string.IsNullOrEmpty(str)) {
                return "";
            }

            MD5 md5 = new MD5CryptoServiceProvider();
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var temp = new char[bytes.Length];
            Array.Copy(bytes, temp, bytes.Length);
            return new String(temp);
        }

        static public T Dict2Class<T>(Dictionary<string, object> dict) where T : class {
            var type = typeof(T);
            var obj = Activator.CreateInstance(type) as T;
            var propertyList = type.GetProperties();
            foreach (var property in propertyList) {
                var name = property.Name;
                var propertyType = property.PropertyType;
                if (!dict.ContainsKey(name)) {
                    continue;
                }

                var value = dict[name];
                if (ToType(property.PropertyType, ref value)) {
                    property.SetValue(obj, value);
                }
            }
            return obj;
        }

        static public Dictionary<string, T> List2Dict<T>(List<T> objList, string name, Func<T, T> func = null) where T : class {
            var dict = new Dictionary<string, T>();
            while (true) {
                if (objList == null) {
                    break;
                }

                var type = typeof(T);
                var property = type.GetProperty(name);
                if(property == null) {
                    break;
                }

                for (var i = 0; i < objList.Count; ++i) {
                    var obj = objList[i];
                    var key = property.GetValue(obj);
                    if (key == null) {
                        continue;
                    }
                    
                    if(func != null) {
                        obj = func(obj);
                    }
                    dict.Add(key.ToString(), obj);
                }

                break;
            }
            return dict;
        }

        static public bool ToType(Type type, ref object value) {
            var result = false;
            if (type == typeof(int)) {
                result = ToInt(ref value);
            }
            else if (type == typeof(string)) {
                result = ToString(ref value);
            }
            else if (type == typeof(DateTime)) {
                result = ToDatetime(ref value);
            }
            else if (type == typeof(double)) {
                result = ToDouble(ref value);
            }
            return result;
        }

        static public List<string> ToList(string value, char split = ',') {
            var result = new List<string>();
            while (true) {
                if (string.IsNullOrEmpty(value)) {
                    break;
                }

                var items = value.Split(split);
                foreach(var item in items) {
                    result.Add(item.Trim());
                }
                break;
            }
            return result;
        }

    }

}
