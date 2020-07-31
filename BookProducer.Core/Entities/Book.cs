using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookProducer.Core.Entities
{
    [Table("book")]
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public Guid TaskId { get; set; }
        public Guid AuthorId { get; set; }

        public TaskHistory TaskHistory{ get; set; }
        public Author Author{ get; set; }
        public  BookGenre BookGenre { get; set; }
    }
}
