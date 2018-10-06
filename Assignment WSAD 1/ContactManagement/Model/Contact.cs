using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Model
{
    public class Contact
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string GroupName { get; set; }

        public static List<string> ListFixedGroup = new List<string>()
        {
            "Friends", "Family", "WorkPartners", "FootballClub"
        };
    }
}
