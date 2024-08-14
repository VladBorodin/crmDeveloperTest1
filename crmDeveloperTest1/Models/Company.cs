using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crmDeveloperTest1.Models {
    internal class Company {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string INN { get; set; }
        public string OGRN { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Addres { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
