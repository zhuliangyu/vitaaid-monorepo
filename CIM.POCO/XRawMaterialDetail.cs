using System;
using System.Collections.Generic;
using System.Linq;

namespace POCO
{
    [Serializable]
    public class XRawMaterialDetail : VARawMaterialDetail
    {
        //Memory Object
        public virtual XMESRawMaterial oXOwner { get; set; }

        public static string GenFullLocationInfo(IList<XRawMaterialDetail> RMDetailList)
        {
            try
            {
                if (RMDetailList == null || RMDetailList.Count == 0) return "";
                Dictionary<string, int> locationList = new Dictionary<string, int>();
                string key;
                foreach (XRawMaterialDetail rm in RMDetailList)
                {
                    key = rm.StockLocation + "*" + Math.Round(rm.StockWeight.Value, 3, MidpointRounding.AwayFromZero).ToString() + "kg";
                    if (locationList.Keys.Contains(key))
                        locationList[key] = locationList[key] + 1;
                    else
                        locationList.Add(key, 1);
                }
                key = locationList.Keys.ElementAt(0);
                string rtnStr = key + "*" + locationList[key];
                for (int i = 1; i < locationList.Keys.Count(); i++)
                {
                    key = locationList.Keys.ElementAt(i);
                    rtnStr += "," + key + "*" + locationList[key];
                }
                return rtnStr;
            }
            catch (Exception ex) { throw ex; }
        }

        public static string GenLocations(IList<XRawMaterialDetail> RMDetailList)
        {
            try
            {
                if (RMDetailList == null || RMDetailList.Count == 0) return "";
                IList<string> sLocs = new List<string>();
                foreach (XRawMaterialDetail rm in RMDetailList)
                {
                    if (sLocs.Contains(rm.StockLocation))
                        continue;
                    sLocs.Add(rm.StockLocation);
                }
                string rtnStr = sLocs[0];
                for (int i = 1; i < sLocs.Count(); i++)
                {
                    rtnStr += "," + sLocs[i];
                }
                return rtnStr;
            }
            catch (Exception ex) { throw ex; }
        }
        public static string GenLocationsPlusKg(IList<XRawMaterialDetail> RMDetailList)
        {
            try
            {
                if (RMDetailList == null || RMDetailList.Count == 0) return "";
                IList<string> sLocs = new List<string>();
                double dTotalKg = 0.0;
                foreach (XRawMaterialDetail rm in RMDetailList)
                {
                    dTotalKg += rm.StockWeight.Value + rm.ReserveWeight.Value;
                    if (sLocs.Contains(rm.StockLocation))
                        continue;
                    sLocs.Add(rm.StockLocation);
                }
                string rtnStr = sLocs[0];
                for (int i = 1; i < sLocs.Count(); i++)
                {
                    rtnStr += "," + sLocs[i];
                }
                rtnStr += "(" + Math.Round(dTotalKg, 2, MidpointRounding.AwayFromZero).ToString() + "KG)";
                return rtnStr;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
