using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMongoLoad.Models
{
    public class FixtureData
    {
        
            public int Id { get; set; }
            public string HomeName { get; set; }
            public string AwayName { get; set; }
            public string HomeLogo { get; set; }
            public string AwayLogo { get; set; }
            public string Date { get; set; }
            public string City { get; set; }
            public string Stadium { get; set; }
            public string Round { get; set; }
            public string Result { get; set; }
        
    }
}
