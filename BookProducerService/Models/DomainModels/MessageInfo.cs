using System;
using System.Collections.Generic;
using System.Text;
using BookProducerService.Models.DTOs;

namespace BookProducerService.Models.DomainModels
{
    public class MessageInfo
    {
        public string ActionType { get; set; }
        public BookDto BookDto { get; set; }

    }
}
