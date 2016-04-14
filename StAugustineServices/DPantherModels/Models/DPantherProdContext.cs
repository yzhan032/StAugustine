using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DPantherModels.Models
{
    public partial class DPantherProdContext : DbContext
    {
        static DPantherProdContext()
        {
            Database.SetInitializer<DPantherProdContext>(null);
        }

        public DPantherProdContext()
            : base("Name=DPantherProdContext")
        {
        }
              
        public DbSet<DP_Applicatons> DP_Applicatons { get; set; }
        
        public DbSet<DP_DirectoryType> DP_DirectoryType { get; set; }
      
        public DbSet<DP_Identifier> DP_Identifier { get; set; } 
        public DbSet<DP_Logs> DP_Logs { get; set; }
        public DbSet<DP_LogTypes> DP_LogTypes { get; set; }
      
        public DbSet<dpView_appCollection> dpView_appCollection { get; set; }
       
     

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
         
        }
    }
}
