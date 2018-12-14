using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yo
{
    [Table("sys_user")]
    public partial class sys_user
    {
        public int id { get; set; }

        [Required]
        public string uname { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string mobile { get; set; }
    }

}
