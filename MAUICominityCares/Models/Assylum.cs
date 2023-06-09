using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUICominityCares.Models
{
    public class Assylum
    {
        public byte Id { get; set; }

        public string Name { get; set; } = null!;

        public string Nit { get; set; } = null!;

        public string RepresentativeName { get; set; } = null!;

        public string BussinessEmail { get; set; } = null!;

        public string CellphoneNumber { get; set; } = null!;

        public string Address { get; set; } = null!;

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public byte Status { get; set; }

        public DateTime RegisterDate { get; set; }

        public string Region { get; set; } = null!;
    }
}
