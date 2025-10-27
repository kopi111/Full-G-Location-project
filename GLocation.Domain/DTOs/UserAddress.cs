using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLocation.Domain.DTOs
{
    public class UserAddress
    {
        public string city { get; set; }

        public string state { get; set; }

        public string country { get; set; }

        public string zipCode { get; set; }

        public string cityCode { get; set; }

    }
}
