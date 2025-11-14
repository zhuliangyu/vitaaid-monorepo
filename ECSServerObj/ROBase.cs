namespace ECSServerObj
{
    public class ROBase<T>
    {
        public bool bResult { get; set; }
        public string sErrMessage { get; set; }
        public T oData { get; set; }
        public int iAddenda { get; set; }
        public string sAddenda { get; set; }

        public virtual ROBase<T> ShallowCopy()
        {
            return (ROBase<T>)this.MemberwiseClone();
        }
    }
}