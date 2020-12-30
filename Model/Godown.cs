using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.models
{
    public class Godown
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public string Code { get; set; }

        public string Address { get; set; }

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }
    }
}