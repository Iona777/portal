using System;
using System.Collections.Generic;
using System.Text;

using  FileHelpers;

namespace NewQuoteTool.TestData.Models
{
    [DelimitedRecord(",")]
    public class ElectricMatrix
    {
        public string MonthDuration { get; set; }
        public string DNO { get; set; }
        public string StandingChargePence { get; set; }
        public string UnitRatePence { get; set; }
    }

}
