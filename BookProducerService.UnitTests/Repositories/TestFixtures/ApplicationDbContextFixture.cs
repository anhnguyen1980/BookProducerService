using AutoFixture;
using BookProducer.Core.Entities;
using BookProducerService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BookProducerService.UnitTests.Repositories.TestFixtures
{
    public class ApplicationDbContextFixture : IDisposable
    {
        public ApplicationDbContext AppDbContext { get; }
        private Fixture _fixture;
        public ApplicationDbContextFixture()
        {
            _fixture = new Fixture();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("BookStore_test")
                .Options;
            AppDbContext = new ApplicationDbContext(options);
              InitData();
           // OneTimeSetup();
        }
        private void InitData()
        {
            var authors = _fixture.Build<Author>().Without(i => i.Books).CreateMany(10).ToList();
            List<Status> status = new List<Status>()
            {
                new Status(){ Id=1, Name= "Queued" },
                new Status(){ Id = 2, Name = "Completed" }
            };
            //var status1 = fixture.Build<Status>().With(i => i.Id, 1).Without(i => i.TaskHistory).Create();
            //var status2 = fixture.Build<Status>().With(i => i.Id, 2).Without(i => i.TaskHistory).Create();
            var genre = _fixture.Build<Genre>().Without(i => i.BookGenre).CreateMany(7).ToList();

            var tasks = _fixture.Build<TaskHistory>().Without(i => i.Book).Without(i => i.Status).Without(i => i.StatusId).CreateMany(5).ToList();
            var random = new Random();
            foreach (var item in tasks)
            {
                item.StatusId = Convert.ToByte(status[random.Next(status.Count)].Id);
            }
            var books = _fixture.Build<Book>().Without(i => i.TaskHistory).Without(i => i.TaskId).Without(i => i.BookGenre)
                                            .Without(i => i.Author).Without(i => i.AuthorId).CreateMany(5);
            List<BookGenre> bookGenre = new List<BookGenre>();
            foreach (var item in books)
            {
                item.TaskId = tasks[random.Next(tasks.Count)].Id;
                item.AuthorId = authors[random.Next(authors.Count)].Id;

                bookGenre.Add(new BookGenre() { BookId = item.Id, GenreId = genre[random.Next(genre.Count)].Id });
            }
            AppDbContext.Status.AddRange(status);
            AppDbContext.Authors.AddRange(authors);
            AppDbContext.Genres.AddRange(genre);
            AppDbContext.TaskHis.AddRange(tasks);
            AppDbContext.Books.AddRange(books);
            AppDbContext.BookGenres.AddRange(bookGenre);

            AppDbContext.SaveChanges();
        }
        private void OneTimeSetup()
        {
            var random = new Random();

            var authors = _fixture.Build<Author>()
                .Without(i => i.Books)
                .CreateMany(10)
                .ToList();

            var genres = _fixture.Build<Genre>()
                .Without(i => i.BookGenre)
                .CreateMany(10)
                .ToList();

            var status1 = _fixture.Build<Status>()
                .With(i => i.Id, 1)
                .Without(i => i.TaskHistory)
                .Create();

            var tasks1 = _fixture.Build<TaskHistory>()
                .With(i => i.StatusId, status1.Id)
                .Without(j => j.Status)
                .Without(j => j.Book)
                .CreateMany(5)
                .ToList();

            var status2 = _fixture.Build<Status>()
                .With(i => i.Id, 2)
                .Without(i => i.TaskHistory)
                .Create();

            var tasks2 = _fixture.Build<TaskHistory>()
                .With(i => i.StatusId, status2.Id)
                .Without(i => i.Status)
                .Without(i => i.Book)
                .CreateMany(50)
                .ToList();

            foreach (var task in tasks2)
            {
                var authorIndex = random.Next(0, authors.Count);
                tasks2.FirstOrDefault(i => i.Id == task.Id).Book
                    =(_fixture.Build<Book>()
                        .With(i => i.TaskId, task.Id)
                        .With(i => i.AuthorId, authors[authorIndex].Id)
                        .With(i => i.Author, authors[authorIndex])
                        .Without(i => i.TaskHistory)
                        .Without(i => i.Author)
                        .Without(i => i.BookGenre)
                        .Create()
                    );
            }

            var books = tasks2.Select(i => i.Book);

            var bookGenres = new List<BookGenre>();

            foreach (var book in books)
            {
                var genre = genres[random.Next(0, genres.Count)];

                bookGenres.Add(new BookGenre() { BookId = book.Id, GenreId = genre.Id });
            }

            status1.TaskHistory = tasks1;
            status2.TaskHistory = tasks2;

            AppDbContext.Status.Add(status1);
            AppDbContext.Status.Add(status2);

            AppDbContext.BookGenres.AddRange(bookGenres);

            AppDbContext.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    AppDbContext.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ApplicationDbContextFixture()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(AppDbContext);
        }
        #endregion
    }
}
