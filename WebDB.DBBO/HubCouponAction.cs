using MyHibernateUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace WebDB.DBBO
{
    [Serializable]
    public class HubCouponAction
    {
        public virtual int ID { get; set; }
        [Required]
        public virtual int SortOrder { get; set; } = 1;
        [Required]
        public virtual string ActionType { get; set; }
        public virtual string ActionDetails { get; set; }
        [Required]
        public virtual DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public virtual DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        [JsonIgnore]
        public virtual HubCoupon oCoupon { get; set; }
    }
}

