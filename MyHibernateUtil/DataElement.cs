using System;
using System.Linq.Expressions;
using System.Xml.Serialization;

namespace MyHibernateUtil
{
  public abstract class DataElement : POCOBase
  {
    [XmlIgnore]
    public virtual string DataStatus { get; set; } = "";
    [XmlIgnore]
    public virtual string CreatedID { get; set; }
    [XmlIgnore]
    public virtual DateTime CreatedDate { get; set; } = DateTime.Now;
    [XmlIgnore]
    public virtual string UpdatedID { get; set; }
    [XmlIgnore]
    public virtual DateTime UpdatedDate { get; set; } = DateTime.Now;

    public static string sDefaultUserID { get; set; } = "";

    public virtual void PreAdd(string overridedUpdatedID = null)
    {
      DataStatus = eDATASTATUS.ACTIVE;
      CreatedDate = DateTime.Now;
      UpdatedDate = DateTime.Now;
      if (string.IsNullOrWhiteSpace(CreatedID))
        CreatedID = (overridedUpdatedID != null) ? overridedUpdatedID : sDefaultUserID;
      UpdatedID = (overridedUpdatedID != null) ? overridedUpdatedID : sDefaultUserID;
    }

    public virtual void PreUpdate(string overridedUpdatedID = null)
    {
      UpdatedDate = DateTime.Now;
      UpdatedID = (overridedUpdatedID != null) ? overridedUpdatedID : sDefaultUserID;
    }

    public virtual void PreDelete(string overridedUpdatedID = null)
    {
      UpdatedDate = DateTime.Now;
      UpdatedID = (overridedUpdatedID != null) ? overridedUpdatedID : sDefaultUserID;
      DataStatus = eDATASTATUS.DELETE;
    }

    public static string sHideDeletedDataWhere(string prefix = "") => " (" + prefix + "DataStatus IS NULL OR " + prefix + "DataStatus <> '" + eDATASTATUS.DELETE + "') ";
    public static Expression<Func<T, bool>> HideDeletedDataExpression<T>() => (x) => (x as DataElement).DataStatus == null || (x as DataElement).DataStatus != eDATASTATUS.DELETE;
  }
}
