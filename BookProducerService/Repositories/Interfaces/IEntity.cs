using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repositories.Interfaces
{
   public interface IEntity
    {
        public Guid Guid { get; set; }
    }
}
