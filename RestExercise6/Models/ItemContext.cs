using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestExercise6.Models
{
    public class ItemContext : DbContext
    {
        //The constructor just calls the constructor in the DbContext class
        public ItemContext(DbContextOptions<ItemContext> options) : base(options) { }

        //We only have a single table with Items which is represented by this DbSet
        public DbSet<Item> Items { get; set; }
    }

}
