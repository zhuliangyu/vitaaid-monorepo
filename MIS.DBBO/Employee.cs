using MyHibernateUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIS.DBBO
{
    // memory loaded from ECSServer - WS_Employee
    public class Employee: POCOBase
    {
        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Account { get; set; }
        public virtual string ShortName { get; set; }
        public virtual string PWD { get; set; }
        public virtual string Status { get; set; }
        public virtual bool bForceChangePassword { get; set; }
        public virtual bool bQA { get; set; } = false;
        public virtual bool bJointAccount { get; set; }
        public virtual string Title { get; set; }
        public virtual string Email { get; set; }
        // Memory object
        public virtual SalesCommissionConfig oCommissionConfigure { get; set; }
        public override int getID()
        {
            return ID;
        }
    }
}
