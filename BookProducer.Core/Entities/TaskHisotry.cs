using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookProducer.Core.Entities
{
    [Table("taskhistory")]
    public class TaskHistory
    {

        public Guid Id { get; set; }
        public string Requested { get; set; }
        public string Finish { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public byte StatusId { get; set; }
        public Status Status { get; set; }

        public Book Book { get; set; }
    }
}
