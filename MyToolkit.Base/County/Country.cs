using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToolkit.Base.County
{

    public class Country
    {
        public string isoCode { get; set; }
        public string name { get; set; }
        public string phonecode { get; set; }
        public string flag { get; set; }
        public string currency { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public List<Timezone> timezones { get; set; }
    }


}
