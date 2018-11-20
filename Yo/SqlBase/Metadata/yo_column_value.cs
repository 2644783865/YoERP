namespace Yo
{
    public class yo_column_value
    {
        public object value { get; }
        public object valueDisplay { get; }

        public yo_column_value(object value, object valueDisplay) {
            this.value = value;
            this.valueDisplay = valueDisplay;
        }
    }
}
