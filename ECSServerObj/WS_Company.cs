using System;

namespace ECSServerObj
{
    public class WS_Company
    {
        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Address { get; set; }
        public virtual string City { get; set; }
        public virtual string Province { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string Country { get; set; }
        public virtual string Tel { get; set; }
        public virtual string Fax { get; set; }
        public virtual string Email { get; set; }
        public virtual string Website { get; set; }
        public virtual string GSTNo { get; set; }
        public virtual Byte[] Logo { get; set; }
        public virtual string BankInfo { get; set; }
    }

}
