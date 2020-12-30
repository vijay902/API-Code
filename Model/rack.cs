using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.models
{
    public class Rack
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public string Code { get; set; }

        public int Capacity { get; set; }

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }

        public int GoDownId { get; set; }

    }
}