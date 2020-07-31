using BookProducer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
namespace BookProducerService.Infrastructure.Database.EntitiesConfiguration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("book");
            builder.HasKey(e => e.Id);

            // configures one-to-many relationship
            builder.HasOne(e => e.Author)
                .WithMany(g => g.Books)
            .HasForeignKey(e => e.AuthorId);

            builder.HasOne(b => b.TaskHistory)
                .WithOne(t => t.Book)
                .HasForeignKey<Book>(b => b.TaskId);
            builder.HasOne(e => e.BookGenre)
               .WithOne(e => e.Book)
               .HasForeignKey<BookGenre>(e => e.BookId);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(b => b.Id).HasColumnName("id");
            builder.Property(b => b.Title).HasColumnName("title");
            builder.Property(b => b.Description).HasColumnName("description");
            builder.Property(b => b.Date).HasColumnName("date");
            builder.Property(e => e.Date)
                .HasColumnType("datetime").IsRequired();
            builder.Property(e => e.Title)
                .HasColumnType("varchar(255)").IsRequired();
            builder.Property(e => e.Description)
                .HasColumnType("varchar(1000)");
            builder.Property(e => e.TaskId).IsRequired();

            builder.Property(e => e.AuthorId).IsRequired();
        }
        public BookConfiguration()
        {
            //ToTable("book");
            //HasKey(e => e.Id);
            //HasIndex(e => e.TaskId).HasName("fk_book_task");
            //HasIndex(e => e.AuthorId).HasName("fk_book_author");
            //Property(e => e.Id)
            //    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)
            //    .IsRequired();
            //Property(e => e.date)
            //    .HasColumnType("datetime2").IsRequired();
            //Property(e => e.title)
            //    .HasColumnType("varchar(255)").IsRequired();
            //Property(e => e.description)
            //    .HasColumnType("varchar(1000)");
            //Property(e => e.TaskId).IsRequired();
            ////     .HasCharSet("latin1")
            //// .Hascollation();
            //Property(e => e.AuthorId).IsRequired();
            ////    .HasCharSet("latin1")
            ////.Hascollation();

            //////relationship  
            ////HasMany(t => t.Courses).WithMany(c => c.Students)
            ////                     .Map(t => t.ToTable("StudentCourse")
            ////                         .MapLeftKey("StudentId")
            ////                         .MapRightKey("CourseId"));
        }
    }
}
