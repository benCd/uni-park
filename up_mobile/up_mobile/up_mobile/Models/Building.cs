using System;
using System.Collections.Generic;
using System.Text;

namespace up_mobile.Models
{
    public class Building
    {
        public int Building_id { get; set; }
        public int University_id { get; set; }
        public string Building_name { get; set; }
        public string Building_code { get; set; }
        public string Address { get; set; }
    }
}
