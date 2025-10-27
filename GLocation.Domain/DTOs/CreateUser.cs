using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLocation.Domain.DTOs
{
    public class CreateUser
    {
        public string email {  get; set; }

        public string password { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string phone { get; set; }

       public UserAddress  address { get; set; }

    }
}
