using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebDB.DBBO;

namespace backend.vitaaid.com.Model
{
    public class BEProduct
    {
        public virtual int ID { get; set; }
        [Required(ErrorMessage = "ProductName is required")]
        public virtual string ProductName { get; set; }
        [Required(ErrorMessage = "ProductCode is required")]
        public virtual string ProductCode { get; set; }
        public virtual string Size { get; set; }
        public virtual string NPN { get; set; }
        public virtual string Tags { get; set; }
        public virtual bool Published { get; set; } = false;
        public virtual eWEBSITE VisibleSite { get; set; } = eWEBSITE.ALL;
        public virtual DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}