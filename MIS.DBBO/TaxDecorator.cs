namespace MIS.DBBO
{
    public class TaxDecorator
    {
        public virtual string TaxID { get; set; }
        public virtual string TaxName { get; set; }
        public virtual double TaxRate { get; set; } = 0;
    }
}
