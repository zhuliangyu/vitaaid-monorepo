using MyHibernateUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace WebDB.DBBO
{
  [Serializable]
  public class TherapeuticProtocol : DataElement
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual int Category { get; set; }
    public virtual string Author { get; set; }
    public virtual string Issue { get; set; }
    public virtual string Topic { get; set; }
    public virtual string BlogContent { get; set; }
    public virtual string Banner { get; set; }
    public virtual string Tags { get; set; }
    public virtual string PDFFile { get; set; }
    public virtual eWEBSITE VisibleSite { get; set; } = eWEBSITE.ALL;
    public virtual bool Published { get; set; } = false;

    // memory object
    public virtual byte[] BannerImage { get; set; }
    public virtual IList<Product> RelatedProducts { get; set; } = new List<Product>();
  }
}
