using System.Data.Entity;

namespace Yo
{
    public partial class JDb : DbContext
    {
        public JDb()
            : base("name=JDb") {
        }

        public virtual DbSet<sys_user> sys_user { get; set; }
        public virtual DbSet<sys_token> sys_token { get; set; }
        public virtual DbSet<sys_table> sys_table { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
        }

    }

}
