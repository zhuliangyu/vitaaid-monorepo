using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHibernateUtil
{
  [Serializable]
  public class AttachedFileBase : POCOBase
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual Byte[] FileBody { get; set; }
  }
}
