using System.Data.Entity;

namespace Yo
{
    public partial class JDb : DbContext
    {
        public JDb()
            : base("name=JDb") {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
        }

    }

}
