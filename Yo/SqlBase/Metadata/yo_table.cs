using System;

namespace Yo
{
    public class yo_table
    {
        public const string name = nameof(table_name);
        public const string rows = nameof(table_rows);
        public const string comment = nameof(table_comment);

        public string table_name { get; set; }
        public UInt64 table_rows { get; set; }
        public string table_comment { get; set; }

        public string display { get; set; }
    }
}
