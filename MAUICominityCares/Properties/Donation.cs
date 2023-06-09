using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUICominityCares.Properties
{
    public class Donation
    {
        public int Id { get; set; }

        public string? DescriptionItems { get; set; }

        public decimal? DescriptionMonto { get; set; }

        public byte Status { get; set; }

        public DateTime RegisterDate { get; set; }

        public string? IsAnonymus { get; set; }

        public string? IsReceived { get; set; }

        public int IdCollect { get; set; }

        public string Hour { get; set; } = null!;

        public int IdCampaign { get; set; }

        public int IdDonnors { get; set; }
    }
}
