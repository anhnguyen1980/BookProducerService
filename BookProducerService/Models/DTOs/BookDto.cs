using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookProducerService.Models.DTOs
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public Guid TaskId { get; set; }
        public Guid AuthorId { get; set; }
    }
}
