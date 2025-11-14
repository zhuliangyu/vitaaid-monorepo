using System.ComponentModel;

namespace POCO
{
    public class PMSafetyCount : INotifyPropertyChanged
    {
        private string _category = "";
        public virtual string category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged("category");
            }
        }

        private ePACKAGETYPE _packagetype;
        public virtual ePACKAGETYPE packagetype
        {
            get { return _packagetype; }
            set
            {
                _packagetype = value;
                OnPropertyChanged("packagetype");
            }
        }

        private int _SafetyCount;
        public virtual int SafetyCount
        {
            get { return _SafetyCount; }
            set
            {
                _SafetyCount = value;
                OnPropertyChanged("SafetyCount");
            }
        }

        private int _whstockcount;
        public virtual int whstockcount
        {
            get { return _whstockcount; }
            set
            {
                _whstockcount = value;
                OnPropertyChanged("whstockcount");
            }
        }

        private double _whstockweight;
        public virtual double whstockweight
        {
            get { return _whstockweight; }
            set
            {
                _whstockweight = value;
                OnPropertyChanged("whstockweight");
            }
        }

        public virtual string PackageMaterialName { get; set; }

        public virtual string Comment { get; set; } = "";
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
