using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BookProducer.Core.Entities;
using BookProducerService.Infrastructure.Database;

namespace BookProducer.APITest.Helpers
{
    public class StoredDataHelper
    {
        const string BookFileName = "Books.json";
        const string pathDirectory = "Data";
        public ICollection<Book> GetBooks()
        {
            if (File.Exists(Path.Combine(pathDirectory, BookFileName)))
            {
                var content = File.ReadAllText(Path.Combine(pathDirectory, BookFileName));
                return JsonConvert.DeserializeObject<ICollection<Book>>(content);
            }
            return null;
        }
        public Book GetBookFirst()
        {
            var books = GetBooks();
            if (books != null)
                return books.FirstOrDefault<Book>();
            return null;
        }
        public void ExtractDataToFile(ApplicationDbContext context)
        {
            if (!System.IO.Directory.Exists(pathDirectory))
                System.IO.Directory.CreateDirectory(pathDirectory);

            var books = context.Books.AsNoTracking().ToList();
            //using (StreamWriter writer = new StreamWriter(Path.Combine(pathDirectory, BookFileName), false))
            //{
            //    writer.WriteLineAsync(JsonConvert.SerializeObject(books));
            //}
            File.WriteAllText(Path.Combine(pathDirectory, BookFileName), JsonConvert.SerializeObject(books));
        }       

    }
}
