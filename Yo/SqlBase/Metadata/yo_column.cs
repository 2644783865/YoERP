using System.Collections.Generic;

namespace Yo
{
    public class yo_column
    {
        public string column_name { get; set; }
        public string data_type { get; set; }
        public string column_type { get; set; }
        public string column_comment { get; set; }

        public string _display { get; set; }
        public object _commentobj { get; set; }
        public DataType _datatype { get; set; }
        public Dictionary<string, string> _set { get; set; }
    }
}
