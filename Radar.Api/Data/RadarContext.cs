using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Radar.Api.Data;

public partial class RadarContext : DbContext
{
    private IConfiguration _configuration;

    public RadarContext(DbContextOptions<RadarContext> options, IConfiguration configuration) : base(options) {
        _configuration = configuration;
    }

    public virtual DbSet<Curtidas> Curtida { get; set; }

    public virtual DbSet<Local> Locals { get; set; }

    public virtual DbSet<Pessoa> Pessoas { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Seguidores> Seguidores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetValue<string>("ConnectionStrings:SqlConnection"));
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Curtidas>(entity =>
        {
            entity.HasKey(e => e.CurtidaId).HasName("PK__Curtida__3D5455C4619058F5");

            entity.Property(e => e.CurtidaId).ValueGeneratedNever();

            entity.HasOne(d => d.PessoaIdCurtindoNavigation).WithMany(p => p.Curtidas).HasConstraintName("FK__Curtida__PessoaI__6FE99F9F");

            entity.HasOne(d => d.PostIdCurtidoNavigation).WithMany(p => p.Curtidas).HasConstraintName("FK__Curtida__PostIDC__70DDC3D8");
        });

        modelBuilder.Entity<Local>(entity =>
        {
            entity.HasKey(e => e.LocalId).HasName("PK__Local__499359DB74B80007");

            entity.Property(e => e.LocalId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasKey(e => e.PessoaId).HasName("PK__Pessoa__2F5F56321380B512");

            entity.Property(e => e.PessoaId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Post__AA1260389B6BAA69");

            entity.Property(e => e.PostId).ValueGeneratedNever();
            entity.Property(e => e.Likes).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.Local).WithMany(p => p.Posts).HasConstraintName("FK__Post__LocalID__68487DD7");

            entity.HasOne(d => d.Pessoa).WithMany(p => p.Posts).HasConstraintName("FK__Post__PessoaID__6754599E");
        });

        modelBuilder.Entity<Seguidores>(entity =>
        {
            entity.HasKey(e => e.SeguidorId).HasName("PK__Seguidor__EAE128AFA2CC30E4");

            entity.Property(e => e.SeguidorId).ValueGeneratedNever();

            entity.HasOne(d => d.PessoaIdSeguidaNavigation).WithMany(p => p.SeguidoresPessoaIdSeguidaNavigation).HasConstraintName("FK__Seguidore__Pesso__5FB337D6");

            entity.HasOne(d => d.PessoaIdSeguidorNavigation).WithMany(p => p.SeguidoresPessoaIdSeguidorNavigation).HasConstraintName("FK__Seguidore__Pesso__60A75C0F");
        });
    }
}