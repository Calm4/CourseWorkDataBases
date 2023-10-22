using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationLab03.Tables;

public partial class NotePlannerDbContext : DbContext
{
    public NotePlannerDbContext()
    {
    }

    public NotePlannerDbContext(DbContextOptions<NotePlannerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<NoteStatus> NoteStatuses { get; set; }

    public virtual DbSet<NoteTag> NoteTags { get; set; }

    public virtual DbSet<SubNote> SubNotes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=CALMBL4;Database=NotePlannerDB;Trusted_Connection=True;TrustServerCertificate=Yes");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.NoteId).HasName("PK__Notes__EACE357F8047C6AE");

            entity.Property(e => e.NoteId).HasColumnName("NoteID");
            entity.Property(e => e.CreateDate).HasColumnType("date");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.NoteDescription).IsUnicode(false);
            entity.Property(e => e.NoteStatusId).HasColumnName("NoteStatusID");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.TagId).HasColumnName("TagID");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.NoteStatus).WithMany(p => p.Notes)
                .HasForeignKey(d => d.NoteStatusId)
                .HasConstraintName("FK__Notes__NoteStatu__5535A963");

            entity.HasOne(d => d.Tag).WithMany(p => p.Notes)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("FK__Notes__TagID__5441852A");

            entity.HasOne(d => d.User).WithMany(p => p.Notes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Notes__UserID__534D60F1");
        });

        modelBuilder.Entity<NoteStatus>(entity =>
        {
            entity.HasKey(e => e.NoteStatusId).HasName("PK__NoteStat__F04E16C77EA0A525");

            entity.HasIndex(e => e.StatusName, "UQ__NoteStat__05E7698AAB56CC64").IsUnique();

            entity.Property(e => e.NoteStatusId).HasColumnName("NoteStatusID");
            entity.Property(e => e.StatusName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NoteTag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PK__NoteTags__657CFA4C6E2E5CC7");

            entity.HasIndex(e => e.TagName, "UQ__NoteTags__BDE0FD1D2F25DC20").IsUnique();

            entity.Property(e => e.TagId).HasColumnName("TagID");
            entity.Property(e => e.TagName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SubNote>(entity =>
        {
            entity.HasKey(e => e.SubNoteId).HasName("PK__SubNotes__42ED1B4BE18891FA");

            entity.Property(e => e.SubNoteId).HasColumnName("SubNoteID");
            entity.Property(e => e.NoteId).HasColumnName("NoteID");
            entity.Property(e => e.SubNoteDescription).IsUnicode(false);

            entity.HasOne(d => d.Note).WithMany(p => p.SubNotes)
                .HasForeignKey(d => d.NoteId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SubNotes__NoteID__5812160E");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACF2C5E259");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534C939CCF3").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__Users__C9F2845687F8B5C5").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserPassword)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
