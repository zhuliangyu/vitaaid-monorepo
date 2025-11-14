using System.ComponentModel;

namespace POCO
{
    public enum eSTATE
    {
        NONE = 1,
        NEW = 2,
        DIRTY = 4,
        DELETE = 16,
        OTHER = 32,
    }
    public abstract class DBBOBase : INotifyPropertyChanged
    {
        public virtual int ID { get; set; } = 0;
        private eSTATE _iState = eSTATE.NONE;
        public virtual eSTATE iState
        {
            get
            {
                return _iState;
            }
            set
            {
                _iState = value;
            }
        }
        public virtual event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
