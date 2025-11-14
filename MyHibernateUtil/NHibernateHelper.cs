using NHibernate;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Impl;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MySystem.Base.Extensions;
using NHibernate.Engine;

namespace MyHibernateUtil
{
    public class NHibernateHelper
    {
        public static IList<NHibernateHelper> ms_Helpers = new List<NHibernateHelper>();
        private static readonly _InitNHibernateHelperSys ms_oInitNHibernateHelperSys = new _InitNHibernateHelperSys();
        public static Logger Log = null; // INHibernateLogger Log = null;
        private class _InitNHibernateHelperSys
        {
            public _InitNHibernateHelperSys()
            {
                //NHibernate.AdoNet.Util.SqlStatementLogger.CloseSessionCallback = NHibernateHelper.CloseSessionCallback;
            }
        }

        private const int MAX_SESSION = 20;

        private readonly SessionProxy[] sessionPool = new SessionProxy[MAX_SESSION];
        public MyCacheOfClient myCacheOfClient { get; set; } = new MyCacheOfClient();
        private Configuration Configuration { get; set; }
        public ISessionFactory SessionFactory { get; set; }
        public virtual string FactoryName { get; set; } = "Development";
        public string DataSource { get; set; } = "";
        public string Database { get; set; } = "";
        public static string DataSources { get => string.Join(";", ms_Helpers.Where(x => x.SessionFactory != null).Select(x => x.DataSource).Distinct()); }
        public static string Factories { get => string.Join(";", ms_Helpers.Where(x => x.SessionFactory != null).Select(x => x.FactoryName).Distinct()); }
        public eST SessionType { get; set; } = eST.SESSION0;
        public string MappingAssembly { get; set; } = "";
        //private ISession session = null;
        //private IStatelessSession statelessSession = null;
        public bool DefaultReadOnly { get; set; } = false;
        public NHibernateHelper(eST _SessionType, bool DefaultReadOnly = false)
        {
            SessionType = _SessionType;
            this.DefaultReadOnly = DefaultReadOnly;
            ms_Helpers.Add(this);
        }

        private void initSessionPool()
        {
            lock (sessionPool)
            {
                if (sessionPool[0] != null)
                {
                    return;
                }

                for (int i = 0; i < MAX_SESSION; i++)
                {
                    sessionPool[i] = new SessionProxy();
                    sessionPool[i].type = SessionType;
                }
            }
        }

        public void CloseSessionPool()
        {
            lock (sessionPool)
            {
                for (int i = 0; i < MAX_SESSION; i++)
                {
                    SessionProxy sp = sessionPool[i];
                    if (sp == null || sp.session == null || sp.tid == 0 || sp.startDT == null)
                    {
                        continue;
                    }

                    ITransaction currXact = sp.session.GetCurrentTransaction();//.Transaction;
                    if (currXact != null)
                    {
                        if (currXact.IsActive)
                        {
                            currXact.Rollback();
                        }

                        currXact.Dispose();
                        currXact = null;
                    }
                    sp.session.Dispose();
                    sp.session = null;
                    sp.startDT = null;
                    Log?.Debug("{0, -8}:Close session:{1}", Database, sp.tid);
                    sp.tid = 0;
                }
            }
        }


        private Configuration ConfigureNHibernate(string assembly)
        {
            Configuration = new Configuration();
            Configuration.AddAssembly(assembly);
            return Configuration;
        }

        private void addMyFlushEvent(Configuration config)
        {
            IList<IFlushEventListener> newEventListenerList = new List<IFlushEventListener>();
            config.EventListeners.FlushEventListeners?.ToList<IFlushEventListener>()?.ForEach(x => newEventListenerList.Add(x));
            newEventListenerList.Add(myCacheOfClient);
            config.EventListeners.FlushEventListeners = newEventListenerList.ToArray();
        }
        public void Initialize(string fileName, string factoryName)
        {
            FactoryName = factoryName;
            Configuration = new Configuration();
            Configuration = MyConfigurationExtensions.Configure(Configuration, factoryName, fileName);
            addMyFlushEvent(Configuration);
            SessionFactory = Configuration.BuildSessionFactory();
            try
            {
                DataSource = (SessionFactory as NHibernate.Impl.SessionFactoryImpl).ConnectionProvider.GetConnection()?.ConnectionString.Split(';').Where(x => x.StartsWith("Data Source")).FirstOrDefault().Split('=')[1];
                Database = (SessionFactory as NHibernate.Impl.SessionFactoryImpl).ConnectionProvider.GetConnection()?.ConnectionString.Split(';').Where(x => x.StartsWith("Database")).FirstOrDefault().Split('=')[1];
                MappingAssembly = Configuration.ClassMappings.FirstOrDefault()?.MappedClass?.Namespace ?? "";
            }
            catch (Exception) { }
            initSessionPool();
        }

        public void Initialize(StreamReader sr, string factoryName)
        {
            FactoryName = factoryName;
            Configuration = new Configuration();
            Configuration = MyConfigurationExtensions.Configure(Configuration, factoryName, sr);
            SessionFactory = Configuration.BuildSessionFactory();
            initSessionPool();
        }
        public void Initialize(string assembly)
        {
            Configuration = ConfigureNHibernate(assembly);
            SessionFactory = Configuration.BuildSessionFactory();
            initSessionPool();
        }

        public void Dispose()
        {
            CloseSessionPool();
            lock (SessionFactory)
            {
                if (SessionFactory != null)
                {
                    SessionFactory.Close();
                    SessionFactory.Dispose();
                    SessionFactory = null;
                }
            }
        }
        public void CloseSession()
        {
            lock (sessionPool)
            {
                int tid = System.Threading.Thread.CurrentThread.ManagedThreadId;
                SessionProxy sp = null;
                int i;
                for (i = 0; i < MAX_SESSION; i++)
                {
                    sp = sessionPool[i];
                    if (sp == null || sp.session == null || sp.tid == 0 || sp.startDT == null)
                    {
                        continue;
                    }

                    if (sp.tid == tid)
                    {
                        break;
                    }
                }
                if (i == MAX_SESSION)
                {
                    return; // NOT FOUND
                }

                ITransaction currXact = sp.session.GetCurrentTransaction();//Transaction;
                if (currXact != null)
                {
                    if (currXact.IsActive)
                    {
                        currXact.Rollback();
                    }

                    currXact.Dispose();
                    currXact = null;
                }
                sp.session.Clear();
                sp.session.Dispose();
                sp.session = null;
                sp.startDT = null;
                Log?.Debug("{0, -8}:Close {1}:{2}", Database, sp.type, sp.tid);
                sp.tid = 0;
            }
        }

        public ISession Session
        {
            get
            {
                lock (sessionPool)
                {
                    int tid = System.Threading.Thread.CurrentThread.ManagedThreadId;

                    SessionProxy sp = null;
                    int i;
                    for (i = 0; i < MAX_SESSION; i++)
                    {
                        sp = sessionPool[i];
                        if (sp == null || sp.session == null || sp.tid == 0 || sp.startDT == null)
                        {
                            continue;
                        }

                        if (sp.tid == tid)
                        {
                            break;
                        }
                    }
                    if (i == MAX_SESSION) // not found
                    {
                        for (i = 0; i < MAX_SESSION; i++)
                        {
                            sp = sessionPool[i];
                            if (sp == null || sp.session == null || sp.tid == 0 || sp.startDT == null)
                            {
                                break;
                            }
                        }
                        if (i == MAX_SESSION) // no spare session, free oldest session
                        {
                            Log?.Debug("No spare {0}, free oldest session, tid={1}", SessionType, tid);

                            DateTime now = DateTime.Now;
                            long max = 0, iTmp;
                            for (i = 0; i < MAX_SESSION; i++)
                            {
                                if ((iTmp = now.Ticks - sessionPool[i].startDT.Value.Ticks) > max)
                                {
                                    max = iTmp;
                                    sp = sessionPool[i];
                                }
                            }
                            // close session
                            Log?.Debug("{0, -8}:Close {1}:{2}", Database, sp.type, sp.tid);
                            if (sp.session.IsOpen)
                            {
                                ITransaction currXact = sp.session.GetCurrentTransaction();// Transaction;
                                if (currXact != null)
                                {
                                    if (currXact.IsActive)
                                    {
                                        currXact.Rollback();
                                    }

                                    currXact.Dispose();
                                    currXact = null;
                                }
                            }
                            sp.session.Dispose();
                            sp.session = null;
                            sp.startDT = null;
                            sp.tid = 0;
                        }
                    }
                    if (sp == null)
                    {
                        return null;
                    }

                    if (sp.session == null)
                    {
                        sp.session = SessionFactory?.OpenSession() ?? null;
                        sp.startDT = DateTime.Now;
                    }
                    sp.tid = tid;
                    sp.session.DefaultReadOnly = DefaultReadOnly;
                    Log?.Debug("{0, -8}:Use   {1}:{2}", Database, sp.type, sp.tid);
                    return sp.session;
                }
            }
        }

        public SessionProxy AvailableSessionInPool
        {
            get
            {
                lock (sessionPool)
                {
                    int tid = System.Threading.Thread.CurrentThread.ManagedThreadId;

                    SessionProxy sp = null;
                    int i;
                    for (i = 0; i < MAX_SESSION; i++)
                    {
                        sp = sessionPool[i];
                        if (sp == null || sp.session == null || sp.tid == 0 || sp.startDT == null)
                        {
                            continue;
                        }

                        if (sp.tid == tid)
                        {
                            break;
                        }
                    }
                    if (i == MAX_SESSION) // not found
                    {
                        for (i = 0; i < MAX_SESSION; i++)
                        {
                            sp = sessionPool[i];
                            if (sp == null || sp.session == null || sp.tid == 0 || sp.startDT == null)
                            {
                                break;
                            }
                        }
                        if (i == MAX_SESSION) // no spare session, free oldest session
                        {
                            Log?.Debug("no spare {0}, free oldest session, tid={1}", SessionType, tid);

                            DateTime now = DateTime.Now;
                            long max = 0, iTmp;
                            for (i = 0; i < MAX_SESSION; i++)
                            {
                                if ((iTmp = now.Ticks - sessionPool[i].startDT.Value.Ticks) > max)
                                {
                                    max = iTmp;
                                    sp = sessionPool[i];
                                }
                            }
                            // close session
                            Log?.Debug("{0, -8}:Close {1}:{2}", Database, sp.type, sp.tid);
                            if (sp.session.IsOpen)
                            {
                                ITransaction currXact = sp.session.GetCurrentTransaction();// Transaction;
                                if (currXact != null)
                                {
                                    if (currXact.IsActive)
                                    {
                                        currXact.Rollback();
                                    }

                                    currXact.Dispose();
                                    currXact = null;
                                }
                            }
                            sp.session.Dispose();
                            sp.session = null;
                            sp.startDT = null;
                            sp.tid = 0;
                        }
                    }
                    if (sp == null)
                    {
                        return null;
                    }

                    if (sp.session == null)
                    {
                        sp.session = SessionFactory?.OpenSession() ?? null;
                        sp.startDT = DateTime.Now;
                    }
                    sp.tid = tid;
                    sp.session.DefaultReadOnly = DefaultReadOnly;
                    Log?.Debug("{0, -8}:Use   {1}:{2}", Database, sp.type, sp.tid);
                    return sp;
                }
            }
        }

        public bool CloseSession(SessionProxy oSession)
        {
            lock (sessionPool)
            {
                int tid = System.Threading.Thread.CurrentThread.ManagedThreadId;
                var target = sessionPool.Where(x => x == oSession)
                           .FirstOrDefault();
                if (target == null)
                {
                    Log?.Error("{0}({1}) not found.", oSession.type, (oSession.session as AbstractSessionImpl).SessionId);
                    return false;
                }
                if (target.tid != tid)
                {
                    Log?.Error("Thread-ID({0}) of {1} does not match current thread({2}).", target.tid, target.type, tid);
                    return false;
                }

                ITransaction currXact = target.session.GetCurrentTransaction();//Transaction;
                if (currXact != null)
                {
                    if (currXact.IsActive)
                    {
                        Log?.Warn("There is one uncommited transaction of Session.");
                        currXact.Rollback();
                    }
                    currXact.Dispose();
                    currXact = null;
                }
                target.session.Clear();
                target.session.Dispose();
                target.session = null;
                target.startDT = null;
                target.tid = 0;
                Log?.Debug("{0, -8}:Close {1}:{2}", Database, target.type, tid);
                return true;
            }
        }

        public string DumpSessions()
        {
            string rtnStr = "{tid, startDate, IsConnected, IsOpen}\n";
            lock (sessionPool)
            {
                SessionProxy sp = null;
                int i;
                for (i = 0; i < MAX_SESSION; i++)
                {
                    sp = sessionPool[i];
                    if (sp == null || sp.startDT == null)
                    {
                        continue;
                    }

                    rtnStr += "{" + sp.tid + "," + sp.startDT.Value.ToString("yyyy/MM/dd hh:mm:ss" + "," + sp.session.IsConnected + "," + sp.session.IsOpen + "}\n");
                }
            }
            return rtnStr;
        }
        public void ClearAllSessions()
        {
            lock (sessionPool)
            {
                SessionProxy sp = null;
                int i;
                for (i = 0; i < MAX_SESSION; i++)
                {
                    sp = sessionPool[i];
                    sp?.session?.Clear();
                }
            }
        }
        /*
        public IStatelessSession StatelessSession
        {
            get
            {
                if (statelessSession == null)
                {
                    statelessSession = SessionFactory.OpenStatelessSession();
                }
                return statelessSession;
            }
        }
         */

        public IList<T> ExecuteICriteria<T>()
        {
            using (ITransaction transaction = Session.BeginTransaction())
            {
                try
                {
                    IList<T> result = Session.CreateCriteria(typeof(T)).List<T>();
                    transaction.Commit();
                    return result;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public static void CloseSessionCallback(string sLastAction)
        {
            try
            {
                foreach (NHibernateHelper obj in ms_Helpers)
                {
                    obj.Dispose();
                }

                ms_Helpers.Clear();
            }
            catch (Exception) { }
        }
        public static void CloseAllSessions()
        {
            try
            {
                foreach (NHibernateHelper obj in ms_Helpers)
                    obj.Dispose();
                ms_Helpers.Clear();
            }
            catch (Exception) { }
        }

        public bool IsEntityFor(POCOBase entity)
        {
            return GetSessionByEntity(entity) != null;
        }
        public SessionProxy GetSessionByEntity(POCOBase entity)
        {
            if (entity == null) return null;

            IPersistenceContext sessionContext = null;
            lock (sessionPool)
            {
                foreach (SessionProxy sp in sessionPool)
                {
                    if (sp == null || sp.session == null || sp.tid == 0 || sp.startDT == null)
                        continue;
                    sessionContext = sp.session.GetSessionImplementation().PersistenceContext;
                    if (sessionContext.IsEntryFor(entity))
                        return sp;
                }
            }
            return null;
        }
        public static SessionProxy GetSession(POCOBase entity)
        {
            if (entity == null || !ms_Helpers.Any()) return null;

            string assemblyName = entity.GetType().Namespace;
            SessionProxy sp = null;
            int i;

            foreach (var helper in ms_Helpers)
                if (helper.MappingAssembly == assemblyName &&
                    (sp = helper.GetSessionByEntity(entity)) != null)
                {
                    return sp;
                }
            return null;
        }

        public static NHibernateHelper GetReadOnlyHelperByEntity(POCOBase entity)
        {
            if (entity == null || !ms_Helpers.Any()) return null;
            string assemblyName = entity.GetType().Namespace;
            NHibernateHelper ReadOnlyHelper = null;
            ms_Helpers.Where(x => x.MappingAssembly == assemblyName && x.SessionType == eST.READONLY)
                      .ToList()
                      .Also(x =>
                      {
                          if (x.Count <= 1)
                          {
                              ReadOnlyHelper = x.FirstOrDefault();
                              return;
                          }
                          foreach (var helper in ms_Helpers)
                              if (helper.MappingAssembly == assemblyName && helper.IsEntityFor(entity))
                              {
                                  ReadOnlyHelper = ms_Helpers.Where(z => z.MappingAssembly == assemblyName &&
                                                                   z.Database == helper.Database &&
                                                                   z.SessionType == eST.READONLY)
                                                       .UniqueOrDefault();
                                  break;
                              }
                      });
            return ReadOnlyHelper;
        }

    }
}
