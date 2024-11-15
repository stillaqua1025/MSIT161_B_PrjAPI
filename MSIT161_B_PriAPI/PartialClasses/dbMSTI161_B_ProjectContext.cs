using Microsoft.EntityFrameworkCore;

namespace MSIT161_B_PriAPI.Models
{
    public partial class dbMSTI161_B_ProjectContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
                optionsBuilder.UseSqlServer(config.GetConnectionString("dbMSTI161_B_Project"));
            }
        }
    }
}
