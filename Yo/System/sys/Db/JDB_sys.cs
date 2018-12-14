using System.Data.Entity;

namespace Yo
{
    public partial class JDb : DbContext
    {
        public virtual DbSet<sys_column> sys_column { get; set; }
    }

}
