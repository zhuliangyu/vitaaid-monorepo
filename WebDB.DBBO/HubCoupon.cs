using MyHibernateUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace WebDB.DBBO
{
    [Serializable]
    public class HubCoupon
    {
        public virtual int ID { get; set; }
        [Required]
        public virtual string Code { get; set; }
        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual string DiscountType { get; set; }
        [Required]
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual int? MaxUsage { get; set; }
        public virtual int? PerCustomerUsage { get; set; }
        public virtual bool IsActive { get; set; } = true;
        [Required]
        public virtual DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public virtual DateTime UpdatedAt { get; set; } = DateTime.Now;
        public virtual string Notes { get; set; }
    }
}

