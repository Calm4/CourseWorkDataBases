using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lab_06.Models;

public partial class NotePlannerDbContext : DbContext
{
    public NotePlannerDbContext()
    {
    }

    public NotePlannerDbContext(DbContextOptions<NotePlannerDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public virtual DbSet<Note> Notes { get; set; }
    
    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<NoteStatus> NoteStatuses { get; set; }

    public virtual DbSet<NoteTag> NoteTags { get; set; }

    public virtual DbSet<SubNote> SubNotes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();
        string connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }
    
}
