using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crmDeveloperTest1.Models {
    internal class Contract {
        public int Id { get; set; }
        public Company Agent { get; set; }
        public Person MainPerson { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public string SigningDate {  get; set; }
    }
}
