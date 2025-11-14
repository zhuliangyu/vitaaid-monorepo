using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHibernateUtil.Extensions;
using System.Linq.Expressions;

namespace MyHibernateUtil
{
  public class SessionProxy
  {
    public ISession session = null;
    public DateTime? startDT = null;
    public int tid = 0;
    public eST type;
    public ORMServer dbServer = null;

    public void Close() => dbServer.CloseSession(this);
    public ITransaction BeginTransaction() => session.BeginTransaction();
    public void Clear() => session.Clear();
    public IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> alias) where T : class
    {
        return session.QueryOver<T>(alias);
    }
    public IQueryable<T> QueryDataElement<T>() => session.QueryDataElement<T>();
    public IQueryable<T> Query<T>() => session.Query<T>();
    public IQuery CreateQuery(string queryString) => session.CreateQuery(queryString);
    public ISQLQuery CreateSQLQuery(string queryString) => session.CreateSQLQuery(queryString);
    public void Evict(object obj) => session.Evict(obj);
    public void SimpleSaveObj(POCOBase obj, bool bPermanent = false) => session.SimpleSaveObj(obj, bPermanent);
    public object Save(object obj) => session.Save(obj);
    public void SaveObj(POCOBase obj, string overridedUpdatedID = null) => session.SaveObj(obj, overridedUpdatedID);
    public void DeleteObj(POCOBase obj, bool bPermanent = true, string overridedUpdatedID = null) => session.DeleteObj(obj, bPermanent, overridedUpdatedID);

  }
}
