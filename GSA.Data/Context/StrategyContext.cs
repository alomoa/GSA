using System;
using System.Collections.Generic;
using GSA.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace GSA.Data.Context;

public partial class StrategyContext : DbContext
{
    public StrategyContext()
    {
    }

    public StrategyContext(DbContextOptions<StrategyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Capital> Capitals { get; set; }

    public virtual DbSet<Pnl> Pnls { get; set; }

    public virtual DbSet<Strategy> Strategies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("data source=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=Strategy;App=EntityFramework");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Capital>(entity =>
        {
            entity.HasKey(e => e.CapitalId).HasName("PK__Capital__D3DFD19334AAD56E");

            entity.ToTable("Capital");

            entity.Property(e => e.CapitalId).HasColumnName("CapitalID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.StrategyId).HasColumnName("StrategyID");

            entity.HasOne(d => d.Strategy).WithMany(p => p.Capitals)
                .HasForeignKey(d => d.StrategyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Capital__Strateg__3D5E1FD2");
        });

        modelBuilder.Entity<Pnl>(entity =>
        {
            entity.HasKey(e => e.PnlId).HasName("PK__Pnl__0212A20B2B19A5CA");

            entity.ToTable("Pnl");

            entity.Property(e => e.PnlId).HasColumnName("PnlID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.StrategyId).HasColumnName("StrategyID");

            entity.HasOne(d => d.Strategy).WithMany(p => p.Pnls)
                .HasForeignKey(d => d.StrategyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pnl__StrategyID__3A81B327");
        });

        modelBuilder.Entity<Strategy>(entity =>
        {
            entity.HasKey(e => e.StrategyId).HasName("PK__Strategy__459B980C39930BA5");

            entity.ToTable("Strategy");

            entity.Property(e => e.StrategyId).HasColumnName("StrategyID");
            entity.Property(e => e.Region)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.StratName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
