using System;
using System.Collections.Generic;
using PropertyChanged;

namespace POCO
{
    [Serializable]
    public class XMESRawMaterial : VAMESRawMaterial
    {

        // memory object
        public virtual IList<XRawMaterialDetail> oRMDetails { get; set; } = new List<XRawMaterialDetail>();
    }
}
