using Microsoft.EntityFrameworkCore;

namespace ToDo_API.Entities
{
    public class ToDoDbContext : DbContext
    {
        public DbSet<ToDo> ToDos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Set max lenght of Title to 70 and make it required
            modelBuilder.Entity<ToDo>()
                 .Property(t => t.Title)
                 .IsRequired()
                 .HasMaxLength(70);
            //Set max lenght of Description to 500 and make it required
            modelBuilder.Entity<ToDo>()
                 .Property(t => t.Description)
                 .IsRequired()
                 .HasMaxLength(500);

        }
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options)
        {
        }
    }
}
