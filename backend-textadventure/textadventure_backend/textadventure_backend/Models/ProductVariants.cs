using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Models
{
    public class ProductVariants
    {
        public string tier { get; set; }

        public ProductVariants(string tierName)
        {
            tier = tierName;
        }
    }
}
