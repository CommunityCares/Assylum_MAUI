using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUICominityCares.Models
{
    public class Person
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public int status { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Ci { get; set; }
        public string PhoneNumber { get; set; }
    }
}
