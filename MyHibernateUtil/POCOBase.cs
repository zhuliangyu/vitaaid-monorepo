using MyHibernateUtil.Extensions;
using MySystem.Base;
using NHibernate;
using System.ComponentModel;
using PropertyChanged;
using System.Xml.Serialization;

namespace MyHibernateUtil
{
  public abstract class POCOBase : INotifyPropertyChanged
  {
    public abstract int getID();

    public static bool bEditMode = false;
    [XmlIgnore]
    public virtual eOPSTATE iState { get; set; } = eOPSTATE.INIT;

    public virtual bool IsDirty => (iState == eOPSTATE.DELETE || iState == eOPSTATE.DIRTY || iState == eOPSTATE.NEW);
    public virtual bool IsEditable => (iState == eOPSTATE.DIRTY || iState == eOPSTATE.NEW);
    public virtual bool IsDeleted => iState == eOPSTATE.DELETE;

    public virtual event PropertyChangedEventHandler PropertyChanged;
    public virtual void OnPropertyChanged(string name)
    {
      if (bEditMode == true && iState == eOPSTATE.INIT)
      {
        iState = eOPSTATE.DIRTY;
      }

      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public virtual object ShallowCopy()
    {
      return MemberwiseClone();
    }


    // object persisence
    public virtual void SaveObj(ISession session, string overridedUpdatedID = null) => session.SaveObj(this, overridedUpdatedID);
    public virtual void DeleteObj(ISession session, bool bPermanent = true, string overridedUpdatedID = null) => session.DeleteObj(this, bPermanent, overridedUpdatedID);
    public virtual object Save(SessionProxy session) => session.session.Save(this);
    public virtual void SaveObj(SessionProxy session, string overridedUpdatedID = null) => session.SaveObj(this, overridedUpdatedID);
    public virtual void DeleteObj(SessionProxy session, bool bPermanent = true, string overridedUpdatedID = null) => session.DeleteObj(this, bPermanent, overridedUpdatedID);
  }
}
