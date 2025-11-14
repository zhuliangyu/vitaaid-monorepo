using MyHibernateUtil;
using System;

namespace POCO
{
  public class ReceivingNo_v
  {
    public virtual string ReceivingNo { get; set; }
    public static string GetNextReceivingNo(SessionProxy oSession, string sDate = null)
      => GetNextReceivingNo(oSession.session, sDate);
    public static string GetNextReceivingNo(NHibernate.ISession session, string sDate = null)
    {
      if (string.IsNullOrEmpty(sDate))
        return session.GetNamedQuery("NextReceivingNo").SetParameter("sDate", DateTime.Now.ToString("yyyyMMdd")).List<string>()[0];
      return session.GetNamedQuery("NextReceivingNo").SetParameter("sDate", sDate).List<string>()[0];
    }
    public static bool Exists(SessionProxy oSession, string sReceivingNo)
      => Exists(oSession.session, sReceivingNo);
    public static bool Exists(NHibernate.ISession session, string sReceivingNo)
    {
      int iExist = session.GetNamedQuery("ExistReceivingNo").SetParameter("sTargetNo", sReceivingNo).List<int>()[0];
      return (iExist == 1);
    }
  }
}
