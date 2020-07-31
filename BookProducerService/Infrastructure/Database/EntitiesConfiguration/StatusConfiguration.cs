using System;
using System.Collections.Generic;
using System.Text;
using BookProducer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookProducerService.Infrastructure.Database.EntitiesConfiguration
{
    public class StatusConfiguration:IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
           builder. ToTable("status");
            builder.HasKey(e => e.Id);
           
            builder.HasMany(s => s.TaskHistory)
              .WithOne(t => t.Status)
              .HasForeignKey(t => t.StatusId);

            builder.Property(e => e.Id).IsRequired()
                .HasColumnType("tinyint(4) unsigned");
            builder.Property(e => e.Name).IsRequired()
                .HasColumnType("varchar(50)");
        }
        public StatusConfiguration()
        {
            
        }
    }
}
