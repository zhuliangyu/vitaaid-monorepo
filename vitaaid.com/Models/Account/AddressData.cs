using MIS.DBBO;

namespace vitaaid.com.Models.Account
{
    public class AddressData
    {
        public virtual int ID { get; set; } = 0;
        public virtual bool DefaultBillingAddress { get; set; } = false;
        public virtual bool DefaultShippingAddress { get; set; } = false;
        public virtual string AddressPerson { get; set; } = "";
        public virtual string AddressName { get; set; } = "";
        public virtual string Address { get; set; } = "";
        public virtual string City { get; set; } = "";
        public virtual string Province { get; set; } = "";
        public virtual string PostalCode { get; set; } = "";
        public virtual string Country { get; set; } = "";
        public virtual string Tel { get; set; } = "";
        public virtual string Fax { get; set; } = "";
        public virtual string DescOnInvoice { get; set; } = "";

        public AddressData() { }
        public AddressData(CustomerAddress vaAddress)
        {
            if (vaAddress == null)
                return;
            ID = vaAddress.ID;
            DefaultBillingAddress = vaAddress.DefaultBillingAddress;
            DefaultShippingAddress = vaAddress.DefaultShippingAddress;
            AddressPerson = vaAddress.AddressPerson;
            AddressName = vaAddress.AddressName;
            Address = vaAddress.Address;
            City = vaAddress.City;
            Province = vaAddress.Province;
            PostalCode = vaAddress.PostalCode;
            Country = vaAddress.Country;
            Tel = vaAddress.Tel;
            Fax = vaAddress.Fax;
            DescOnInvoice = vaAddress.DescOnInvoice;
        }

    }
}
