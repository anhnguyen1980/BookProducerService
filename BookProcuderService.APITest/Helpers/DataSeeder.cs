using BookProducer.Core.Entities;
using BookProducerService.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookProducer.APITest.Helpers
{
    public class DataSeeder
    {
        public void Populate(ApplicationDbContext context)
        {

            if (!context.Status.Any())
            {
                var status = new Status[] {
                    new Status(){ Id=1, Name="Queued"},
                    new Status(){ Id=2, Name="Completed"}
                };
                context.Status.AddRange(status);
                context.SaveChanges();
            }
            if (!context.Genres.Any())
            {
                var genres = new Genre[]
            {
                    new Genre(){ Id=Guid.NewGuid(), Name="Horror"},
                    new Genre(){ Id=Guid.NewGuid(), Name="Romantic"},
                    new Genre(){ Id=Guid.NewGuid(), Name="Science Fiction"},
                    new Genre(){ Id=Guid.NewGuid(), Name="Funny"},
                    new Genre(){ Id=Guid.NewGuid(), Name="Love"},
                    new Genre(){ Id=Guid.NewGuid(), Name="Action"},
                    new Genre(){ Id=Guid.NewGuid(), Name="Gost"}
            };
                context.Genres.AddRange(genres);
                context.SaveChanges();
            }
            if (!context.Authors.Any())
            {
                var authors = new Author[]
            {
                    new Author(){ Id=Guid.NewGuid(), Name="Robert M. Pirsig"},
                    new Author(){ Id=Guid.NewGuid(), Name="Richard Adams"},
                    new Author(){ Id=Guid.NewGuid(), Name="Randy Pausch & Jeffrey Zaslow"},
                    new Author(){ Id=Guid.NewGuid(), Name="Bill Bryson"},
                    new Author(){ Id=Guid.NewGuid(), Name="Viktor Frankl"},
                    new Author(){ Id=Guid.NewGuid(), Name="Joe Haldeman"},
                    new Author(){ Id=Guid.NewGuid(), Name="Carl Sagan"}
            };
                context.Authors.AddRange(authors);
                context.SaveChanges();
            }
            if (!context.TaskHis.Any())
            {
                string DateRequest = DateTime.Now.ToString("yyyyMMddHHmmss");
                string DateFinish = DateTime.Now.ToString("yyyyMMddHHmmsstt");
                var tasks = new TaskHistory[]
                {

                    new TaskHistory(){ Id=Guid.NewGuid(), StatusId=1, CreatedDate=DateTime.Now, UpdatedDate=DateTime.Now,
                                    Requested=DateRequest, Finish=DateFinish},
                    new TaskHistory(){ Id=Guid.NewGuid(), StatusId=2, CreatedDate=DateTime.Now, UpdatedDate=DateTime.Now,
                                    Requested=DateRequest, Finish=DateFinish},
                    new TaskHistory(){ Id=Guid.NewGuid(), StatusId=1, CreatedDate=DateTime.Now, UpdatedDate=DateTime.Now,
                                    Requested=DateRequest, Finish=DateFinish},
                    new TaskHistory(){ Id=Guid.NewGuid(), StatusId=2, CreatedDate=DateTime.Now, UpdatedDate=DateTime.Now,
                                    Requested=DateRequest, Finish=DateFinish},
                    new TaskHistory(){ Id=Guid.NewGuid(), StatusId=1, CreatedDate=DateTime.Now, UpdatedDate=DateTime.Now,
                                    Requested=DateRequest, Finish=DateFinish},
                    new TaskHistory(){ Id=Guid.NewGuid(), StatusId=2, CreatedDate=DateTime.Now, UpdatedDate=DateTime.Now,
                                    Requested=DateRequest, Finish=DateFinish},
                    new TaskHistory(){ Id=Guid.NewGuid(), StatusId=1, CreatedDate=DateTime.Now, UpdatedDate=DateTime.Now,
                                    Requested=DateRequest, Finish=DateFinish}
                };
                context.TaskHis.AddRange(tasks);
                context.SaveChanges();
            }
            if (!context.Books.Any())
            {
                var contextAuthors = context.Authors.ToList();
                var contextTasks = context.TaskHis.ToList();

                var books = new List<Book>();
                int n = 1;
                foreach (var item in contextTasks)
                {

                    books.Add(new Book()
                    {
                        Id = Guid.NewGuid(),
                        Date = DateTime.Now,
                        Description = "Description " + n.ToString(),
                        Title = "Title " + n.ToString(),
                        AuthorId = contextAuthors[0].Id,
                        TaskId = item.Id
                    });
                    n++;
                }

                context.Books.AddRange(books);
                context.SaveChanges();
            }
            if (!context.BookGenres.Any())
            {
                var contextBooks = context.Books.ToList();
                var contextGenres = context.Genres.ToList();
                var bookgenres = new List<BookGenre>();
                for (int i = 0; i < contextBooks.Count; i++)
                {
                    bookgenres.Add(
                        new BookGenre()
                        {
                            BookId = contextBooks[i].Id,
                            GenreId = i < contextGenres.Count ? contextGenres[i].Id : contextGenres[contextGenres.Count].Id
                        }
                        );
                }
                context.BookGenres.AddRange(bookgenres);
                context.SaveChanges();
            }
            new BookProducer.APITest.Helpers.StoredDataHelper().ExtractDataToFile(context);
        }


    }
}
