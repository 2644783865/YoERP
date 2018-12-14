using System.Data.Entity;

namespace Yo
{
    public partial class JDb : DbContext
    {
        public virtual DbSet<sys_table> sys_table { get; set; }
        public virtual DbSet<sys_column> sys_column { get; set; }

    }
}
