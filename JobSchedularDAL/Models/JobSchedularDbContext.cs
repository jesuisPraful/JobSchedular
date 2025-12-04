using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JobSchedularDAL.Models;

public partial class JobSchedularDbContext : DbContext
{
    public JobSchedularDbContext()
    {
    }

    public JobSchedularDbContext(DbContextOptions<JobSchedularDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ExecutionNode> ExecutionNodes { get; set; }

    public virtual DbSet<JobDefinition> JobDefinitions { get; set; }

    public virtual DbSet<JobExecutionLog> JobExecutionLogs { get; set; }

    public virtual DbSet<JobRetry> JobRetries { get; set; }

    public virtual DbSet<JobSchedule> JobSchedules { get; set; }

    public virtual DbSet<ResourceAllocation> ResourceAllocations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json");
        var config = builder.Build();
        var connectionString = config.GetConnectionString("JobSchedularDBConnection");
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExecutionNode>(entity =>
        {
            entity.HasKey(e => e.NodeId).HasName("PK__Executio__92881BD3EBEB7D03");

            entity.ToTable("ExecutionNode");

            entity.HasIndex(e => e.NodeIpaddress, "UQ__Executio__DB1A4747DF89BF5C").IsUnique();

            entity.Property(e => e.NodeId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nodeId");
            entity.Property(e => e.NodeIpaddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nodeIPAddress");
            entity.Property(e => e.NodeName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("nodeName");
            entity.Property(e => e.NodeStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nodeStatus");
        });

        modelBuilder.Entity<JobDefinition>(entity =>
        {
            entity.HasKey(e => e.JobId).HasName("PK__JobDefin__164AA1A8B09FAE11");

            entity.ToTable("JobDefinition");

            entity.Property(e => e.JobId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("jobId");
            entity.Property(e => e.JobDescription)
                .HasColumnType("text")
                .HasColumnName("jobDescription");
            entity.Property(e => e.JobName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("jobName");
            entity.Property(e => e.JobParameters).HasColumnName("jobParameters");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.Timestamps)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("timestamps");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.JobDefinitions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Job_User");
        });

        modelBuilder.Entity<JobExecutionLog>(entity =>
        {
            entity.HasKey(e => e.ExecutionLogId).HasName("PK__JobExecu__D2760962136BAC18");

            entity.ToTable("JobExecutionLog");

            entity.Property(e => e.ExecutionLogId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("executionLogId");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("endTime");
            entity.Property(e => e.ExecutionNodeId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("executionNodeId");
            entity.Property(e => e.ExecutionStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("executionStatus");
            entity.Property(e => e.JobId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("jobId");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("startTime");

            entity.HasOne(d => d.Job).WithMany(p => p.JobExecutionLogs)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK_Log_Job");
        });

        modelBuilder.Entity<JobRetry>(entity =>
        {
            entity.HasKey(e => e.RetryId).HasName("PK__JobRetry__79A6FF0C3B9FB827");

            entity.ToTable("JobRetry");

            entity.Property(e => e.RetryId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("retryId");
            entity.Property(e => e.JobId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("jobId");
            entity.Property(e => e.RetryAttemptNumber).HasColumnName("retryAttemptNumber");
            entity.Property(e => e.RetryStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("retryStatus");
            entity.Property(e => e.RetryTime)
                .HasColumnType("datetime")
                .HasColumnName("retryTime");

            entity.HasOne(d => d.Job).WithMany(p => p.JobRetries)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK_Retry_Job");
        });

        modelBuilder.Entity<JobSchedule>(entity =>
        {
            entity.HasKey(e => e.JobId).HasName("PK__JobSched__164AA1A8117EE4B4");

            entity.ToTable("JobSchedule");

            entity.Property(e => e.JobId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("jobId");
            entity.Property(e => e.NextRunTime)
                .HasColumnType("datetime")
                .HasColumnName("nextRunTime");
            entity.Property(e => e.SchedulePattern)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("schedulePattern");
            entity.Property(e => e.ScheduledExecutionTime)
                .HasColumnType("datetime")
                .HasColumnName("scheduledExecutionTime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasOne(d => d.Job).WithOne(p => p.JobSchedule)
                .HasForeignKey<JobSchedule>(d => d.JobId)
                .HasConstraintName("FK_Schedule_Job");
        });

        modelBuilder.Entity<ResourceAllocation>(entity =>
        {
            entity.HasKey(e => e.AllocationId).HasName("PK__Resource__992086884F4B6980");

            entity.ToTable("ResourceAllocation");

            entity.Property(e => e.AllocationId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("allocationId");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("endTime");
            entity.Property(e => e.ExecutionNodeId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("executionNodeId");
            entity.Property(e => e.JobId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("jobId");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("startTime");

            entity.HasOne(d => d.ExecutionNode).WithMany(p => p.ResourceAllocations)
                .HasForeignKey(d => d.ExecutionNodeId)
                .HasConstraintName("FK_Resource_Node");

            entity.HasOne(d => d.Job).WithMany(p => p.ResourceAllocations)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK_Resource_Job");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__CB9A1CFFAF7F1735");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534C1316908").IsUnique();

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userId");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
