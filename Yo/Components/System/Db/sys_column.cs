using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yo
{
    [Table("sys_column")]
    public partial class sys_column
    {
        public int id { get; set; }

        public int sys_table_id { get; set; }

        [Required]
        [StringLength(32)]
        public string column_name { get; set; }

        public DataType datatype { get; set; }

        [StringLength(15)]
        public string format { get; set; }

        [StringLength(15)]
        public string refer { get; set; }

        [StringLength(127)]
        public string set { get; set; }

        public int sort { get; set; }
    }
}
