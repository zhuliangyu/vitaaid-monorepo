using MySystem.Base;
using MySystem.Base.Extensions;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyHibernateUtil.Extensions
{
	public static class SessionExtension
	{
		public static IQueryable<T> QueryDataElement<T>(this ISession session) 
		{
			if (typeof(T).IsSubclassOf(typeof(DataElement)))
				return session.Query<T>().Where(DataElement.HideDeletedDataExpression<T>());
			return session.Query<T>();
		}
		public static void SimpleSaveObj(this ISession session, POCOBase obj, bool bPermanent = false)
		{
			if (obj.iState == eOPSTATE.INIT)
			{
				return;
			}

			ITransaction xact = null;
			try
			{
				xact = session.BeginTransaction();
				if (obj.iState == eOPSTATE.DELETE)
				{
					session.DeleteObj(obj, bPermanent);
				}
				else
				{
					session.SaveObj(obj);
				}

				xact.Commit();
			}
			catch (Exception ex)
			{
				xact?.Rollback();
				throw ex;
			}
		}
		public static void SaveObj(this ISession session, POCOBase obj, string overridedUpdatedID = null)
		{
			try
			{
				if (obj == null) return;

				if (obj.getID() == 0)
				{
					if (obj is DataElement)
					{
						((DataElement)obj).PreAdd(overridedUpdatedID);
					}

					session.Save(obj);
				}
				else
				{
					if (obj is DataElement)
					{
						((DataElement)obj).PreUpdate(overridedUpdatedID);
					}

					session.Update(obj);
				}
			}
			catch (Exception ex) { throw ex; }
		}

		public static void DeleteObj(this ISession session, POCOBase obj, bool bPermanent = true, string overridedUpdatedID = null)
		{
			try
			{
				if (obj == null || obj.getID() == 0)
				{
					return;
				}

				if (obj is DataElement && bPermanent == false)
				{
					((DataElement)obj).PreDelete(overridedUpdatedID);
					session.Update(obj);
				}
				else
				{
					session.Delete(obj);
				}
			}
			catch (Exception ex) { throw ex; }
		}

		public static IList<T> GetAllObj<T>(this ISession session, string sOrderby = "", int iTake = 0)
		{
			try
			{
				return session.GetXObjs<T>("", sOrderby, iTake);
			}
			catch (Exception ex) { throw ex; }
		}

		public static IList<T> GetXObjs<T>(this ISession session, string sWhere, string sOrderby = "", int iTake = 0)
		{
			try
			{
				string sFinalWhere = "";
				if (typeof(T).IsSubclassOf(typeof(DataElement)))
				{
					sFinalWhere = DataElement.sHideDeletedDataWhere();
				}

				if (string.IsNullOrWhiteSpace(sWhere) == false)
				{
					if (string.IsNullOrWhiteSpace(sFinalWhere) == false)
					{
						sFinalWhere = " WHERE " + sFinalWhere + " AND (" + sWhere + ") ";
					}
					else
					{
						sFinalWhere = " WHERE " + sWhere + " ";
					}
				}
				else
				{
					if (string.IsNullOrWhiteSpace(sFinalWhere) == false)
					{
						sFinalWhere = " WHERE " + sFinalWhere + " ";
					}
				}


				if (string.IsNullOrWhiteSpace(sOrderby) == false)
				{
					sOrderby = " Order by " + sOrderby;
				}

				if (iTake > 0)
				{
					return session.CreateQuery("from " + typeof(T).Name + " x " + sFinalWhere + sOrderby + " take " + iTake.ToString()).List<T>();
				}
				else
				{
					return session.CreateQuery("from " + typeof(T).Name + " x " + sFinalWhere + sOrderby).List<T>();
				}
			}
			catch (Exception ex) { throw ex; }
		}

		public static T GetObj<T>(this ISession session, int ID, string sIDFieldName = "ID")
		{
			try
			{
				return session.CreateQuery("from " + typeof(T).Name + " where " + sIDFieldName + " = " + ID.ToString()).UniqueResult<T>();
			}
			catch (Exception ex) { throw ex; }
		}

		public static Boolean IsDirtyEntity(this SessionProxy oSession, Object entity) => oSession.session.IsDirtyEntity(entity);
		public static Boolean IsDirtyEntity(this ISession session, Object entity)
		{

			ISessionImplementor sessionImpl = session.GetSessionImplementation();
			IPersistenceContext persistenceContext = sessionImpl.PersistenceContext;
			EntityEntry oldEntry = persistenceContext.GetEntry(entity);

			if ((oldEntry == null) && (entity is INHibernateProxy))
			{
				INHibernateProxy proxy = entity as INHibernateProxy;
				Object obj = sessionImpl.PersistenceContext.Unproxy(proxy);
				oldEntry = sessionImpl.PersistenceContext.GetEntry(obj);
			}
			if (oldEntry == null)
				return false;
			String className = oldEntry.EntityName; 
			IEntityPersister persister = sessionImpl.Factory.GetEntityPersister(className);


			Object[] oldState = oldEntry.LoadedState;
			Object[] currentState = persister.GetPropertyValues(entity);//, sessionImpl.EntityMode);
			Int32[] dirtyProps = oldState.Select((o, i) => (oldState[i].LogicalEqual(currentState[i])) ? -1 : i).Where(x => x >= 0).ToArray();

			return (dirtyProps.Any());
		}
		public static Boolean IsDirtyProperty(this SessionProxy oSession, Object entity, String propertyName) => oSession.session.IsDirtyProperty(entity, propertyName);

		public static Boolean IsDirtyProperty(this ISession session, Object entity, String propertyName)
		{

			ISessionImplementor sessionImpl = session.GetSessionImplementation();
			IPersistenceContext persistenceContext = sessionImpl.PersistenceContext;
			EntityEntry oldEntry = persistenceContext.GetEntry(entity);
			String className = oldEntry.EntityName;
			IEntityPersister persister = sessionImpl.Factory.GetEntityPersister(className);

			if ((oldEntry == null) && (entity is INHibernateProxy))
			{

				INHibernateProxy proxy = entity as INHibernateProxy;

				Object obj = sessionImpl.PersistenceContext.Unproxy(proxy);

				oldEntry = sessionImpl.PersistenceContext.GetEntry(obj);

			}

			Object[] oldState = oldEntry.LoadedState;
			Object[] currentState = persister.GetPropertyValues(entity);//, sessionImpl.EntityMode);
			Int32[] dirtyProps = persister.FindDirty(currentState, oldState, entity, sessionImpl);
			Int32 index = Array.IndexOf(persister.PropertyNames, propertyName);

			Boolean isDirty = (dirtyProps != null) ? (Array.IndexOf(dirtyProps, index) != -1) : false;

			return (isDirty);

		}

		public static Object GetOriginalEntityProperty(this ISession session, Object entity, String propertyName)
		{

			ISessionImplementor sessionImpl = session.GetSessionImplementation();

			IPersistenceContext persistenceContext = sessionImpl.PersistenceContext;

			EntityEntry oldEntry = persistenceContext.GetEntry(entity);

			String className = oldEntry.EntityName;

			IEntityPersister persister = sessionImpl.Factory.GetEntityPersister(className);



			if ((oldEntry == null) && (entity is INHibernateProxy))
			{

				INHibernateProxy proxy = entity as INHibernateProxy;

				Object obj = sessionImpl.PersistenceContext.Unproxy(proxy);

				oldEntry = sessionImpl.PersistenceContext.GetEntry(obj);

			}



			Object[] oldState = oldEntry.LoadedState;

			Object[] currentState = persister.GetPropertyValues(entity);//, sessionImpl.EntityMode);

			Int32[] dirtyProps = persister.FindDirty(currentState, oldState, entity, sessionImpl);

			Int32 index = Array.IndexOf(persister.PropertyNames, propertyName);



			Boolean isDirty = (dirtyProps != null) ? (Array.IndexOf(dirtyProps, index) != -1) : false;



			return ((isDirty == true) ? oldState[index] : currentState[index]);

		}

	}
}
