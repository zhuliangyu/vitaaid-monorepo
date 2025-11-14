using MyHibernateUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebDB.DBBO
{
    [Serializable]
    public class CampaignUser: DataElement
    {
        public virtual int ID { get; set; }
        public override int getID() => ID;
        public virtual string ProgramName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string PrizeItem { get; set; } = "";
    }
}
