using System.Collections.Generic;

namespace Yo
{
    public class yo_ui_column
    {
        public string key { get; set; }
        public string keyDisplay { get; set; }

        public object value { get; set; }
        public object valueDisplay { get; set; }

        public bool isPK { get; set; }
        public string refer { get; set; }
        public Dictionary<string, string> _set { get; set; }
    }
}
