using System;
using System.Collections.Generic;
using System.Text;
using BookProducer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookProducerService.Infrastructure.Database.EntitiesConfiguration
{
    public class BookGenreConfiguration : IEntityTypeConfiguration<BookGenre>
    {
        public void Configure(EntityTypeBuilder<BookGenre> builder)
        {
            builder.ToTable("bookgenre");
            builder.HasKey(e => new { e.BookId, e.GenreId });

            builder.HasOne(bg => bg.Book)
                .WithOne(b => b.BookGenre)
                .HasForeignKey<BookGenre>(bg => bg.BookId);
            builder.HasOne(bg => bg.Genre)
                   .WithOne(g => g.BookGenre)
                   .HasForeignKey<BookGenre>(bg => bg.GenreId);
            //builder.HasOne(u => u.Genre)
            //    .WithOne()
            //    .IsRequired()
            //    .HasForeignKey<Genre>(u => u.Id)
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.BookId).IsRequired();
            builder.Property(e => e.GenreId).IsRequired();
        }
        public BookGenreConfiguration()
        {

        }
    }
}
