
namespace MIS.DBBO
{
    public class DistributorCommissionConfigure : SharedCommissionConfigure
    {
        public virtual CustomerAccount oDistributor { get; set; } = null;
        public virtual string DistributorAccount
        {
            get { return oDistributor?.CustomerCode ?? ""; }
            set { }
        }
        public virtual string DistributorName
        {
            get { return oDistributor?.CustomerName ?? ""; }
            set { }
        }
        private string _CountryName = null;
        public virtual string CountryName
        {
            get
            {
                return (string.IsNullOrWhiteSpace(_CountryName)) ? oDistributor?.CustomerCountry1 ?? ""
                                                                : _CountryName;
            }
            set { _CountryName = value; }
        }
        public virtual string ProvinceID { get; set; }
    }
}
