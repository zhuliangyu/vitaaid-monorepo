using System.Collections.Generic;

namespace ECSServerObj
{
    public class AuthMap
    {
        public virtual int FunID { get; set; }
        public virtual string FunName { get; set; }
        public virtual string FunCode { get; set; }
        public virtual bool Accessable { get; set; }
        public virtual bool Deny { get => !Accessable; }
        public virtual IList<AuthMap> SubAuths { get; set; } = new List<AuthMap>();
        private AuthMap lookupAuth(string sCode)
        {
            if (sCode == FunCode)
                return this;
            AuthMap oTarget = null;
            foreach (AuthMap oSub in SubAuths)
                if ((oTarget = oSub.lookupAuth(sCode)) != null)
                    return oTarget;
            return null;
        }
        public bool this[string sCode]    // Indexer declaration  
        {
            get
            {
                var oTarget = lookupAuth(sCode);
                return oTarget?.Accessable ?? false;
            }

            set
            {
                var oTarget = lookupAuth(sCode);
                if (oTarget != null)
                    oTarget.Accessable = value;
                
            }
        }
    }
}
