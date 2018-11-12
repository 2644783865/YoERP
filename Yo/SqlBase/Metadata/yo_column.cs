namespace Yo
{
    public class yo_column
    {
        public const string name = nameof(column_name);
        public const string datatype = nameof(data_type);
        public const string datatypeEx = nameof(column_type);
        public const string comment = nameof(column_comment);

        public string column_name { get; set; }
        public string data_type { get; set; }
        public string column_type { get; set; }
        public string column_comment { get; set; }

        public string display { get; set; }
    }
}
