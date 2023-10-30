using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Radar.Api.Models;

namespace Radar.Api.Data;

public partial class RadarContext : DbContext
{
    public virtual DbSet<Local> Locals { get; set; }

    public virtual DbSet<Pessoa> Pessoas { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Seguidore> Seguidores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=tcp:radar-dev-sql-server.database.windows.net,1433;Initial Catalog=radar-dev-database;Persist Security Info=False;User ID=radar-admin;Password=R@dar123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Local>(entity =>
        {
            entity.HasKey(e => e.LocalId).HasName("PK__Local__499359DB74B80007");

            entity.ToTable("Local");

            entity.Property(e => e.LocalId)
                .ValueGeneratedNever()
                .HasColumnName("LocalID");
            entity.Property(e => e.Descricao).HasColumnType("text");
            entity.Property(e => e.Endereco)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Verificado)
                .HasMaxLength(14)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasKey(e => e.PessoaId).HasName("PK__Pessoa__2F5F56321380B512");

            entity.ToTable("Pessoa");

            entity.HasIndex(e => e.Login, "UQ__Pessoa__5E55825BD0831999").IsUnique();

            entity.Property(e => e.PessoaId)
                .ValueGeneratedNever()
                .HasColumnName("PessoaID");
            entity.Property(e => e.DataNascimento).HasColumnType("date");
            entity.Property(e => e.Descricao)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Senha)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Post__AA1260389B6BAA69");

            entity.ToTable("Post");

            entity.Property(e => e.PostId)
                .ValueGeneratedNever()
                .HasColumnName("PostID");
            entity.Property(e => e.Conteudo).HasColumnType("text");
            entity.Property(e => e.DataPostagem).HasColumnType("datetime");
            entity.Property(e => e.Likes).HasDefaultValueSql("((0))");
            entity.Property(e => e.LocalId).HasColumnName("LocalID");
            entity.Property(e => e.PessoaId).HasColumnName("PessoaID");

            entity.HasOne(d => d.Local).WithMany(p => p.Posts)
                .HasForeignKey(d => d.LocalId)
                .HasConstraintName("FK__Post__LocalID__68487DD7");

            entity.HasOne(d => d.Pessoa).WithMany(p => p.Posts)
                .HasForeignKey(d => d.PessoaId)
                .HasConstraintName("FK__Post__PessoaID__6754599E");
        });

        modelBuilder.Entity<Seguidore>(entity =>
        {
            entity.HasKey(e => e.SeguidorId).HasName("PK__Seguidor__EAE128AFA2CC30E4");

            entity.Property(e => e.SeguidorId)
                .ValueGeneratedNever()
                .HasColumnName("SeguidorID");
            entity.Property(e => e.PessoaIdseguida).HasColumnName("PessoaIDSeguida");
            entity.Property(e => e.PessoaIdseguidor).HasColumnName("PessoaIDSeguidor");

            entity.HasOne(d => d.PessoaIdseguidaNavigation).WithMany(p => p.SeguidorePessoaIdseguidaNavigations)
                .HasForeignKey(d => d.PessoaIdseguida)
                .HasConstraintName("FK__Seguidore__Pesso__5FB337D6");

            entity.HasOne(d => d.PessoaIdseguidorNavigation).WithMany(p => p.SeguidorePessoaIdseguidorNavigations)
                .HasForeignKey(d => d.PessoaIdseguidor)
                .HasConstraintName("FK__Seguidore__Pesso__60A75C0F");
        });
    }
}
