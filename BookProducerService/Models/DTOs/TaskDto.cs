using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookProducerService.Models.DTOs
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Requested { get; set; }
        public string Finish { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public byte StatusId { get; set; }
    }
}
