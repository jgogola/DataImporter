using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Data;

public class AppDbContext : DbContext
{
    //public DbSet<ImportCode> ImportCodes { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer(@"Server=localhost;Database=DataImporter;Trusted_Connection=True;");
    //}
}
