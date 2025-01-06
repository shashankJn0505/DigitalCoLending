using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CoLending.Core.Models;

public partial class DigitalcolendingContext : DbContext
{
    public DigitalcolendingContext()
    {
    }

    public DigitalcolendingContext(DbContextOptions<DigitalcolendingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblUserDetail> TblUserDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-E6EN4LC;Database=digitalcolending;User Id=sa;Password=Jain@123;;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblUserDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_User__3214EC07F6A01A4E");

            entity.ToTable("tbl_UserDetails");

            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
