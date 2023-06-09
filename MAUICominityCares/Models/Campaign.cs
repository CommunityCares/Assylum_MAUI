using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUICominityCares.Models
{
    public class Campaign
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Requirement { get; set; } = null!;

        public DateTime InitialDate { get; set; }

        public DateTime CloseDate { get; set; }

        public byte Status { get; set; }

        public DateTime RegisterDate { get; set; }

        public byte IdAssylum { get; set; }

    }
}
