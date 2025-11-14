using ECSServerObj;

namespace MIS.DBPO
{
    public static class EmployeeExtension
    {
        // memory loaded from ECSServer
        public static MIS.DBBO.Employee ToEmployee(this WS_Employee source)
        {
            return new MIS.DBBO.Employee()
            {
                ID = source.ID,
                Name = source.Name,
                Account = source.Account,
                ShortName = source.ShortName,
                PWD = source.PWD,
                Status = source.Status,
                bForceChangePassword = source.bForceChangePassword,
                bQA = source.bQA,
                bJointAccount = source.bJointAccount,
                Title = source.Title,
                Email = source.Email
            };
        }
    }
}
