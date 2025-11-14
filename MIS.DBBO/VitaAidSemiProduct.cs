namespace MIS.DBBO
{
    public class VitaAidSemiProduct : VitaAidProduct
    {
        public virtual double StockWeight { get; set; }
        public virtual double SupplyWeight { get; set; }
        public override bool IsFinishProduct { get => false; set { } }
    }
}
