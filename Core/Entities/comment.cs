using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#nullable disable

namespace Core.Entities
{
    public class comment : BaseEntity
    {
        public string Name { get; set; }

        public string Comment { get; set; }
    }
}