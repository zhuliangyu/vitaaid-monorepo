using MyHibernateUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDB.DBBO
{
    public class JwtRefreshToken: DataElement
    {
        public virtual int ID { get; set; }
        public override int getID() => ID;
        public virtual string RefreshToken { get; set; }
        public virtual string UserID { get; set; }
        public virtual string UserName { get; set; }
        public virtual int Expired { get; set; }
        public virtual string HostName { get; set; }
        public virtual string AppName { get; set; }
    }
}
