using System;
using System.Collections.Generic;
using System.Text;
using BookProducer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookProducerService.Infrastructure.Database.EntitiesConfiguration
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("author");
            builder.HasKey(e => e.Id);
            builder.HasMany(e => e.Books)
                .WithOne(e => e.Author)
                .HasForeignKey(e => e.AuthorId);

            builder.Property(e => e.Id).IsRequired();
            builder.Property(e => e.Name).IsRequired()
                  .HasColumnType("varchar(100)");
        }
        public AuthorConfiguration()
        {

        }
    }
}
