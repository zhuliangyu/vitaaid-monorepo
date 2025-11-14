using System;

namespace ECSServerObj
{
    public class WS_Employee
    {
        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Account { get; set; }
        public virtual string ShortName { get; set; }
        public virtual string PWD { get; set; }
        public virtual string Status { get; set; }
        public virtual AuthMap oAuth { get; set; }
        public virtual bool bForceChangePassword { get; set; }
        public virtual bool bQA { get; set; } = false;
        public virtual bool bJointAccount { get; set; }
        public virtual string Title { get; set; }
        public virtual string Email { get; set; }
        public virtual bool bLock { get; set; }
        public virtual int WrongLogonAttemp { get; set; }
        public virtual Byte[] Signature { get; set; }
        public virtual int CompanyId { get; set; }
        public virtual string CompanyName { get; set; } = "";
        public virtual string[] TeamGroups { get; set; }
    }
}
