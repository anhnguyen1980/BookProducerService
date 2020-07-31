using System;
using System.Collections.Generic;
using System.Text;
using BookProducer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookProducerService.Infrastructure.Database.EntitiesConfiguration
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.ToTable("genre");
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.BookGenre)
              .WithOne(e => e.Genre)
              .HasForeignKey<BookGenre>(e => e.GenreId);

            builder.Property(e => e.Id).IsRequired();
            builder.Property(e => e.Name).IsRequired()
                 .HasColumnType("varchar(50)");
        }
        public GenreConfiguration()
        {

        }
    }
}
