using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCO
{
  public class Constant
  {
    public const int OPIDIGIT = 4;
    public static int DIGIT = OPIDIGIT;

    public static string WIP = "WIP";
    public static string PROC = "PROC";
    public static string HOLD = "HOLD";

    public static List<string> WIPs = new List<string> { WIP, PROC, HOLD };
    public static List<string> OPIDs = new List<string> { "INIT", "1000", "1100", "1150", "1200", "1500", "2500", "3500", "4500", "5500", "6500", "7500", "FINISH" };

    public static string[] sLogLevel = { "INFO", "WARNING", "ERROR", "FATAL", "SYSTEM" };

    // Lot comment Type
    public static string MODELING = "MODELING";
    public static string PRODUCTION = "PRODUCTION";
    public static string RAWMATERIAL = "RAWMATERIAL";
    public static string PACKAGE = "PACKAGE";
    public static List<string> LOTCOMMENTTYPES = new List<string> { MODELING, PRODUCTION, RAWMATERIAL, PACKAGE };

    public static int MATERIAL_NOT_ENOUGH = 0x01;
    public static int MATERIAL_NOT_AVAILABLE = 0x02;
    public static int SCAN_WELL = 0x04;
    public static int OVER_RETESTDATE = 0x08;
    public static int SELECT_FABRMAPPLYRESULT = 0x10;
    public static int EMPTY_BOX = 0x40;
    public static int CALIBRATION_END = 0x80;
    public static int RETURN_RESV = 0x100;
    public static int NEW_ADDENDA = 0x200;
    public static int GET_SAMPLE = 0x400;
    public static int SAMPLE_DONE = 0x800;
    public static int MATERIAL_NOT_RELEASED = 0x1000;

    // rm log type
    public static string RESV = "RESV";
    public static string ROLLBACK = "ROLLBACK";
    public static string NEW = "NEW";
    public static string ADJUST = "ADJUST";
    public static string DELETE = "DELETE";
    public static string APPLY = "APPLY";
    public static string CALIBRATION = "CALI";

    // Product Stability Form
    public static string PASS = "PASS";
    public static string FAIL = "FAIL";
    public static string YES = "YES";
    public static string NO = "NO";
    public static string NA = "N/A";
    public static List<string> YNCHOICE = new List<string> { YES, NO, NA };
    public static List<string> PASSCHOICE = new List<string> { PASS, FAIL, NA };
    public static string CAPSULE = "CAPSULE";
    public static string POWDER = "POWDER";
    public static string LIQUID = "LIQUID";
    public static string TABLET = "TABLET";
    public static List<string> FORMTYPE = new List<string> { CAPSULE, POWDER, LIQUID, TABLET };

  }
}
