using System;
using MyHibernateUtil;
using PropertyChanged;

namespace MIS.DBBO
{
    [Serializable]
    public class CustomerAddress : DataElement
    {
        public virtual int ID { get; set; }
        public virtual CustomerAccount oCustomer { get; set; }
        // Basic Info.
        public virtual string CustomerCode { get => oCustomer?.CustomerCode ?? ""; set { } }
        public virtual string CustomerName { get => oCustomer?.CustomerName ?? ""; set { } }
        public virtual bool DefaultBillingAddress { get; set; } = false;
        public virtual bool DefaultShippingAddress { get; set; } = false;
        public virtual string AddressPerson { get; set; }
        public virtual string AddressName { get; set; }
        public virtual string Address { get; set; }
        public virtual string City { get; set; }
        public virtual string Province { get; set; } = "";
        public virtual string PostalCode { get; set; }
        public virtual string AddressSecondLine { get { return City + ", " + Province + ", " + PostalCode; } }
        private string _Country = "";
        public virtual string Country { get => _Country.ToUpper(); set { _Country = value; } }
        public virtual string Tel { get; set; }
        public virtual string Fax { get; set; }
        public virtual string Desc { get { return AddressName + ":" + AddressPerson + ":" + Tel + ":" + Address + ", " + City + ", " + Province + ", " + PostalCode; } }
        public virtual string DescForReport { get { return AddressName + ", " + Address + ", " + City + ", " + Province + ", " + Country + " " + PostalCode; } }
        private string NewLineAndAppendStr(string src, string appendedStr)
        {
            if (string.IsNullOrWhiteSpace(appendedStr))
                return src;
            if (string.IsNullOrWhiteSpace(src))
                return appendedStr;
            return src + "\n" + appendedStr;
        }
        public virtual string DescOnInvoice
        {
            get
            {
                string sStr = string.IsNullOrWhiteSpace(AddressPerson) ? "" : AddressPerson;
                if (AddressPerson?.Equals(AddressName) ?? false)
                    sStr += "\n";
                else
                    sStr = NewLineAndAppendStr(sStr, AddressName);
                sStr = NewLineAndAppendStr(sStr, Address);
                sStr = NewLineAndAppendStr(sStr, City + ", " + Province + ", " + PostalCode);
                sStr = NewLineAndAppendStr(sStr, Country + ", Tel.:" + Tel);
                return sStr;
            }
        }

        public virtual string Comment { get; set; }
        //public virtual string DataStatus { get; set; } = "";
        //public virtual string CreatedID { get; set; }
        //public virtual DateTime CreatedDate { get; set; }
        //public virtual string UpdatedID { get; set; }
        //public virtual DateTime UpdatedDate { get; set; }

        private string MyTrim(string str) { return (string.IsNullOrWhiteSpace(str)) ? "" : str.Trim(); }
        private string DelSpace(string str)
        {
            string str1 = str;
            string str2 = str;
            while (true)
            {
                str2 = str1.Replace(" ", "").Replace("\t", "");
                if (str1.Length == str2.Length)
                    return str1;
                str1 = str2;
            }
        }
        public virtual bool EqualAddress(object Obj)
        {
            CustomerAddress other = (CustomerAddress)Obj;
            string s1 = DelSpace(MyTrim(this.AddressName) + ":" + MyTrim(this.AddressPerson) + ":" + MyTrim(this.Address) + ", " + MyTrim(this.City) + ", " + MyTrim(this.Province) + MyTrim(this.Tel) + MyTrim(this.Fax)).ToUpper();
            string s2 = DelSpace(MyTrim(other.AddressName) + ":" + MyTrim(other.AddressPerson) + ":" + MyTrim(other.Address) + ", " + MyTrim(other.City) + ", " + MyTrim(other.Province) + MyTrim(other.Tel) + MyTrim(other.Fax)).ToUpper();
            return s1.Equals(s2);
        }
        public virtual bool EqualsWithoutTel(object Obj)
        {
            CustomerAddress other = (CustomerAddress)Obj;
            string s1 = DelSpace(MyTrim(this.AddressName) + ":" + MyTrim(this.AddressPerson) + ":" + MyTrim(this.Address) + ", " + MyTrim(this.City) + ", " + MyTrim(this.Province)).ToUpper();
            string s2 = DelSpace(MyTrim(other.AddressName) + ":" + MyTrim(other.AddressPerson) + ":" + MyTrim(other.Address) + ", " + MyTrim(other.City) + ", " + MyTrim(other.Province)).ToUpper();
            return s1.Equals(s2);
        }

        public virtual string AddressType
        {
            get
            {
                return ((DefaultBillingAddress) ? "[Billing]" : "") +
                        ((DefaultShippingAddress) ? "[Shipping]" : "");
            }
        }

        // memory data
        [DoNotNotify]
        public virtual bool bOldDefaultShippingAddr { get; set; } = false;
        [DoNotNotify]
        public virtual bool bOldDefaultBillingAddr { get; set; } = false;
        [DoNotNotify]
        public virtual bool bEmpty
        {
            get
            {
                if (string.IsNullOrWhiteSpace(AddressPerson) == false ||
                    string.IsNullOrWhiteSpace(AddressName) == false ||
                    string.IsNullOrWhiteSpace(Address) == false ||
                    string.IsNullOrWhiteSpace(City) == false ||
                    string.IsNullOrWhiteSpace(Province) == false ||
                    string.IsNullOrWhiteSpace(PostalCode) == false ||
                    string.IsNullOrWhiteSpace(Country) == false)
                    return false;
                return true;
            }
        }

        //public virtual void Refresh()
        //      {
        //          OnPropertyChanged("Desc");
        //      }

        public override int getID()
        {
            return ID;
        }
    }
}
