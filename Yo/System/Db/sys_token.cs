using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yo
{
    [Table("sys_token")]
    public partial class sys_token
    {
        [Key]
        [Column(TypeName = "char")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ticket { get; set; }
        
        public int uid { get; set; }

        public DateTime ctime { get; set; }

        public DateTime mtime { get; set; }
    }
}
