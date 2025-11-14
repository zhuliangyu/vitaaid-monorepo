using MyHibernateUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace WebDB.DBBO
{
    [Serializable]
    public class HubCouponRule
    {
        public virtual int ID { get; set; }
        [Required]
        public virtual string RuleType { get; set; }
        public virtual string RuleDetails { get; set; }
        [Required]
        public virtual DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public virtual DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        [JsonIgnore]
        public virtual HubCoupon oCoupon { get; set; }
    }
}

