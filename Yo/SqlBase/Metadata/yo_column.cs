namespace Yo
{
    public class yo_column
    {
        public string column_name { get; set; }
        public string data_type { get; set; }
        public string column_type { get; set; }
        public string column_comment { get; set; }

        public string _translation { get; set; }
        public object _comment { get; set; }
        public DataType _datatype { get; set; }
        public object _info { get; set; }
    }
}
