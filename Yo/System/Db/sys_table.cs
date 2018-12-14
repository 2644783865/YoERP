using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yo
{
    [Table("sys_table")]
    public partial class sys_table
    {
        public int id { get; set; }

        [Required]
        [StringLength(32)]
        public string table_name { get; set; }

        public bool init_title { get; set; }

        [StringLength(128)]
        public string title { get; set; }

        [StringLength(128)]
        public string refer { get; set; }

        [NotMapped]
        public string tranlation { get; set; }
    }
}
