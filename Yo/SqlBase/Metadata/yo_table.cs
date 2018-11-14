using System;

namespace Yo
{
    public class yo_table
    {
        public string table_name { get; set; }
        public UInt64 table_rows { get; set; }
        public string table_comment { get; set; }

        public string _display { get; set; }
        public object _commentobj { get; set; }
    }
}
