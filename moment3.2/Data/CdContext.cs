using Microsoft.EntityFrameworkCore;
using moment3._2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace moment3._2.Data
{
    public class CdContext:DbContext
    {
        public CdContext(DbContextOptions<CdContext> options):base(options)
        {

            }
        public DbSet<Cd> Cds { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<Track> Tracks { get; set; }


    }
}
