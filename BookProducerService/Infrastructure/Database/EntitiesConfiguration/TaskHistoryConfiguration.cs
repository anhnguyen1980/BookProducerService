using BookProducer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookProducerService.Infrastructure.Database.EntitiesConfiguration
{
    public class TaskHistoryConfiguration : IEntityTypeConfiguration<TaskHistory>
    {
        public void Configure(EntityTypeBuilder<TaskHistory> builder)
        {
            builder.ToTable("taskhistory");
            builder.HasKey(e => e.Id);
            // builder.HasIndex(e => e.statusId).HasName("fk_task_status");
            builder.HasOne(e => e.Book)
                .WithOne(e => e.TaskHistory)
                .HasForeignKey<Book>(e => e.TaskId);

            builder.HasOne(t => t.Status)
              .WithMany(s => s.TaskHistory)
              .HasForeignKey(t => t.StatusId);
            builder.Property(t => t.Requested).HasColumnName("requested");
            builder.Property(t => t.Finish).HasColumnName("finish");
            builder.Property(t => t.Id).HasColumnName("id");
            builder.Property(e => e.Id).IsRequired();
            builder.Property(e => e.CreatedDate).IsRequired().HasDefaultValue(DateTime.Now)
                .HasColumnType("datetime");
            builder.Property(e => e.UpdatedDate)
                .HasColumnType("datetime");
            builder.Property(e => e.StatusId).IsRequired()
                 .HasColumnType("tinyint(4) unsigned")
                 .IsRequired();
        }
        public TaskHistoryConfiguration()
        {



        }
    }
}
