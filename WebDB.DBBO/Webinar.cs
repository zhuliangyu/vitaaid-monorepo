using MyHibernateUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace WebDB.DBBO
{
    [Serializable]
    public class Webinar : DataElement
    {
        public virtual int ID { get; set; }
        public override int getID() => ID;
        public virtual int Category { get; set; }
        [Required]
        public virtual string Issue { get; set; }
        public virtual string Reference { get; set; }
        [Required]
        public virtual string Author { get; set; }
        //public virtual DateTime WebinarDate { get; set; } = DateTime.Now;
        [Required]
        public virtual string Topic { get; set; }
        public virtual string WebinarContent { get; set; }
        public virtual string VideoLink { get; set; }
        public virtual string Tags { get; set; }
        public virtual string Thumbnail { get; set; }
        public virtual int MaxRegistrants { get; set; }
        public virtual bool Published { get; set; } = false;
        // memory object
        //[Required]
        //public virtual string sCategory { get; set; }
        public virtual byte[] ThumbnailImage { get; set; }
        public virtual IList<Product> RelatedProducts { get; set; } = new List<Product>();
    }
}
