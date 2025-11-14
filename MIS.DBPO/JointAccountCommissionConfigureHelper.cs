using ECSServerObj;
using MIS.DBBO;
using MyHibernateUtil;
using MySystem.Base.Extensions;
using MyToolkit.Base.Extensions;
using System.Collections.Generic;
using System.Linq;
using static MIS.DBPO.DBPOServiceHelper;

namespace MIS.DBPO
{
  public static class JointAccountCommissionConfigureHelper
  {
    //public static IList<JointAccountCommissionConfigure> getCommissionConfigures(Employee oJointAccount = null)
    //  => getCommissionConfigures(MISDB[eST.SESSION0], oJointAccount);
    public static IList<JointAccountCommissionConfigure> getCommissionConfigures(SessionProxy oSession, Employee oJointAccount = null)
    {
      IList<JointAccountCommissionConfigure> commissionConfigures;
      if (oJointAccount == null)
        commissionConfigures = oSession.QueryDataElement<JointAccountCommissionConfigure>()
                                       .ToList();
      else
        commissionConfigures = oSession.QueryDataElement<JointAccountCommissionConfigure>()
                                            .Where(jac => jac.JointAccount == oJointAccount.Account && jac.DataStatus != "D")
                                            .ToList();

      // get employees batch for performance issue
      var SalesRepsOfAccount = RESTfullObject.listEmployees(commissionConfigures.Select(x => x.SalesRepAccount).Distinct())
                                             ?.oData?.Select(x => x.ToEmployee());
      commissionConfigures.Action(commissionConfigure =>
      {
        commissionConfigure.oJointAccount = oJointAccount;
        commissionConfigure.oSalesRep = SalesRepsOfAccount.Where(x => x.Account == commissionConfigure.SalesRepAccount)
                                                                .UniqueOrDefault();
      });
      return commissionConfigures;
    }
  }
}
