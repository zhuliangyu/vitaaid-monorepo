using System;
using System.Collections.Generic;
using MyHibernateUtil;

namespace MIS.DBBO
{
    [Serializable]
    public class DistributorSalesDetail : POCOBase
    {
        public virtual int ID { get; set; }
        public virtual CustomerAccount oDistributor { get; set; }
        public virtual string DistributorName { get => oDistributor?.CustomerName ?? ""; set { } }
        public virtual string City { get; set; }
        public virtual string Province { get; set; }
        public virtual DateTime OrdersDate { get; set; }
        public virtual string OrderNumber { get; set; }
        public virtual string ProductCode { get; set; }
        public virtual string ProductSKU { get; set; }
        public virtual string ProductName { get; set; }
        public virtual string UPC { get; set; }
        public virtual int QuantitySold { get; set; }
        public virtual decimal MSRP { get; set; }
        public virtual DateTime CommissionMonth { get; set; }
        public virtual string sCommissionMonth { get => CommissionMonth.ToString("MM-yyyy"); }
        public virtual double PriceRateForDistributor { get; set; } = 1;
        public virtual decimal DistributorPrice { get; set; }
        public virtual decimal NetSales { get; set; }
        public virtual decimal SalesActivity { get; set; }
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual string CreatedID { get; set; }
        private IList<Employee> _oSalesReps;
        public virtual IList<Employee> oSalesReps
        {
            get { return _oSalesReps; }
            set
            {
                _oSalesReps = value;
                string str = "";
                foreach (var x in _oSalesReps)
                    str += "[" + x.ShortName + "]";
                SalesReps = str;
                str = "";
                foreach (var x in _oSalesReps)
                    str += "[" + x.Account + "]";
                SalesRepAccounts = str;
            }
        }

        public virtual string SalesReps { get; set; }
        public virtual string SalesRepAccounts { get; set; }

        public override int getID()
        {
            return ID;
        }
    }
}
