using NHibernate;
using NLog;
using System;
using System.Collections.Generic;

namespace MyHibernateUtil
{
  public class ORMServer
  {
    public virtual bool bInitialed { get; set; } = false;
    public NHibernateHelper NHB { get; } = new NHibernateHelper(eST.SESSION0);
    //public ISession oSession => NHB.Session;

    public NHibernateHelper NHB1 { get; } = new NHibernateHelper(eST.SESSION1); // for read only session
    //public ISession oSession1 => NHB1.Session;

    public NHibernateHelper DLGNHB { get; } = new NHibernateHelper(eST.DIALOG); // for Dialog 
    //public ISession DLGNHBSession => DLGNHB.Session;

    public NHibernateHelper NHBReadOnly { get; } = new NHibernateHelper(eST.READONLY, true); // for read only session
    //public ISession oROSession => NHBReadOnly.Session;

    private string sConfigureFile = "";
    protected List<string> getFactoryList()
    {
      try
      {
        return (string.IsNullOrWhiteSpace(sConfigureFile)) ? new List<string>() : MyConfigurationExtensions.getFactoryList(sConfigureFile);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    public SessionProxy this[eST type]
    { get {
        var oSP = type switch
        {
          eST.SESSION0 => NHB.AvailableSessionInPool,
          eST.SESSION1 => NHB1.AvailableSessionInPool,
          eST.READONLY => NHBReadOnly.AvailableSessionInPool,
          eST.DIALOG => DLGNHB.AvailableSessionInPool,
          _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
        oSP.type = type;
                oSP.dbServer = this;
      return oSP;
      }
    }
    public void ClearAllSessions()
    {
      NHB.ClearAllSessions();
      NHB1.ClearAllSessions();
      NHBReadOnly.ClearAllSessions();
      DLGNHB.ClearAllSessions();
    }
    public string DataSource { get; set; } = "";
    public string Database { get; set; } = "";


    public ORMServer DoDBConnect(string sFileName, string FactoryName, bool bReadOnlyConnection = false)
    {
      try
      {
        NHibernateLogger.SetLoggersFactory(new NLogLoggerFactory());
        if (NHibernateHelper.Log == null)
          NHibernateHelper.Log = LogManager.GetLogger("MyHibernateUtil"); //NHibernateLogger.For(typeof(NHibernateHelper));
         
        sConfigureFile = sFileName;
        NHBReadOnly.Initialize(sFileName, FactoryName);
        if (!bReadOnlyConnection)
        {
          NHB.Initialize(sFileName, FactoryName);
          NHB1.Initialize(sFileName, FactoryName);
          DLGNHB.Initialize(sFileName, FactoryName);
          DataSource = NHB.DataSource;
          Database = NHB.Database;
        }
        else
        {
          DataSource = NHBReadOnly.DataSource;
          Database = NHBReadOnly.Database;
        }

        bInitialed = true;

        NHibernateHelper.Log.Info("Connect to {0}, Factory={1}", Database, FactoryName);
        return this;
      }
      catch (Exception ex) { throw ex; }
    }

    public void DoDBDispose()
    {
      try
      {
        NHB?.Dispose();
        NHB1?.Dispose();
        DLGNHB?.Dispose();
        NHBReadOnly?.Dispose();
        bInitialed = false;
      }
      catch (Exception ex) { throw ex; }
    }
    public void CloseAllSession()
    {
      NHB?.CloseSession();
      NHB1?.CloseSession();
      DLGNHB?.CloseSession();
      NHBReadOnly?.CloseSession();
    }
    public void CloseSession()
    {
      NHB?.CloseSession();
    }
    public void CloseSession1()
    {
      NHB1?.CloseSession();
    }
    public void CloseDialogSession()
    {
      DLGNHB?.CloseSession();
    }
    public void CloseReadOnlySession()
    {
      NHBReadOnly?.CloseSession();
    }
    public void CloseSession(SessionProxy oSP)
    {
      try
      {
        var result = oSP.type switch
        {
          eST.SESSION0 => NHB.CloseSession(oSP),
          eST.SESSION1 => NHB1.CloseSession(oSP),
          eST.READONLY => NHBReadOnly.CloseSession(oSP),
          eST.DIALOG => DLGNHB.CloseSession(oSP),
          _ => throw new ArgumentOutOfRangeException(nameof(oSP.type))
        };
      }
      catch (Exception)
      {
        throw;
      }
    }    
  }
}
