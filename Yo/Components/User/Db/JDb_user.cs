using System.Data.Entity;

namespace Yo
{
    public partial class JDb : DbContext
    {
        public virtual DbSet<sys_user> sys_user { get; set; }
        public virtual DbSet<sys_token> sys_token { get; set; }

    }
}
