using System;
using System.Collections.Generic;
using GSA.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace GSA.Data.Scaffolded;

public partial class PnlContext : DbContext
{
    public PnlContext()
    {
    }

    public PnlContext(DbContextOptions<PnlContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Pnl> Pnls { get; set; }

    public virtual DbSet<StrategyPnl> StrategyPnls { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("data source=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=PNL;App=EntityFramework").EnableSensitiveDataLogging();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pnl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pnl__3214EC07EECCA5C5");

            entity.ToTable("Pnl");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.StrategyPnl).WithMany(p => p.Pnls)
                .HasForeignKey(d => d.StrategyPnlId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pnl__StrategyPnl__4316F928");
        });

        modelBuilder.Entity<StrategyPnl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Strategy__3214EC07DF0D8819");

            entity.ToTable("StrategyPnl");

            entity.Property(e => e.Strategy)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
