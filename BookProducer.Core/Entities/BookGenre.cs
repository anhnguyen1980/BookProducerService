using System;
using System.Collections.Generic;
using System.Text;

namespace BookProducer.Core.Entities
{
    public class BookGenre
    {
        public Guid BookId { get; set; }
        public Guid GenreId { get; set; }

        public  Book Book { get; set; }
        public  Genre Genre { get; set; }
    }
}
