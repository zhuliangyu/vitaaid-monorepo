using POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DBPO
{
    public class PackageTypeHelper
    {
        /*
            enum eFORMTYPE
                CAPSULE = 1,        // INCLUDE BOTTLE, CAP, LABEL, CAPSULE, DESICCANT
                LIQUID = 2,         // INCLUDE BOTTLE, LABEL
                LIQUID_DROPPER = 3, // INCLUDE BOTTLE, CAP(Dropper), LABEL
                POWDER_BOTTLE =4,   // INCLUDE BOTTLE, CAP, LABEL, DESICCANT, SCOOP
                POWDER_BAG = 5,     // INCLUDE LABEL, DESICCANT, SCOOP
                BULK = 0            // NONE
         */
        public static Dictionary<eFORMTYPE, IList<ePACKAGETYPE>> FormTypeToPackageTypes =
            new Dictionary<eFORMTYPE, IList<ePACKAGETYPE>> {
                { eFORMTYPE.BULK, new List<ePACKAGETYPE>()},
                { eFORMTYPE.CAPSULE, new List<ePACKAGETYPE> { ePACKAGETYPE.BOTTLE, ePACKAGETYPE.CAP, ePACKAGETYPE.LABEL, ePACKAGETYPE.CAPSULE, ePACKAGETYPE.DESICCANT } },
                { eFORMTYPE.LIQUID, new List<ePACKAGETYPE> { ePACKAGETYPE.BOTTLE,ePACKAGETYPE.LABEL} },
                { eFORMTYPE.LIQUID_DROPPER, new List<ePACKAGETYPE> { ePACKAGETYPE.BOTTLE, ePACKAGETYPE.CAP, ePACKAGETYPE.LABEL} },
                { eFORMTYPE.POWDER_BOTTLE, new List<ePACKAGETYPE> { ePACKAGETYPE.BOTTLE, ePACKAGETYPE.CAP, ePACKAGETYPE.LABEL, ePACKAGETYPE.DESICCANT, ePACKAGETYPE.SCOOPS } },
                { eFORMTYPE.POWDER_BAG, new List<ePACKAGETYPE> { ePACKAGETYPE.LABEL, ePACKAGETYPE.DESICCANT, ePACKAGETYPE.SCOOPS } },
            };

    }
}
