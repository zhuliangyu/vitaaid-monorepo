using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToolkit.Base.County
{
    public class CountryHelper
    {
        public static IList<Country> LoadFromJSON(string jsonFile)
        {
            using (StreamReader r = new StreamReader(jsonFile))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<List<Country>>(json);
            }
        }
        public static IList<State> LoadStateFromJSON(string jsonFile)
        {
            using (StreamReader r = new StreamReader(jsonFile))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<List<State>>(json);
            }
        }
    }
}
