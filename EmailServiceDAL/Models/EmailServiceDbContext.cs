using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EmailServiceDAL.Models;

public partial class EmailServiceDbContext : DbContext
{
    public EmailServiceDbContext()
    {
    }

    public EmailServiceDbContext(DbContextOptions<EmailServiceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Email> Emails { get; set; }

    public virtual DbSet<EmailLog> EmailLogs { get; set; }

    public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }

    public virtual DbSet<OutboxEmail> OutboxEmails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json");
        var config = builder.Build();
        var connectionString = config.GetConnectionString("EmailServiceDBConnection");
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Email>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Emails__3214EC07425513AA");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Bcc)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Body).HasColumnType("text");
            entity.Property(e => e.Cc)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SentAt).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Subject)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ToEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EmailLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EmailLog__3214EC07E574A133");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ErrorMessage).HasColumnType("text");
            entity.Property(e => e.LoggedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProviderResponse).HasColumnType("text");

            entity.HasOne(d => d.Email).WithMany(p => p.EmailLogs)
                .HasForeignKey(d => d.EmailId)
                .HasConstraintName("FK_EmailLogs_Emails");
        });

        modelBuilder.Entity<EmailTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EmailTem__3214EC07539B27D8");

            entity.HasIndex(e => e.Name, "UQ__EmailTem__737584F69A6DD8C7").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Body).HasColumnType("text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Subject)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OutboxEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OutboxEm__3214EC0791C66269");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProcessedAt).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
