using MyToolkit.Base.Extensions;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MyToolkit.Base.Menu
{
    public class MenuItemVM : INotifyPropertyChanged
    {
        private string _name;
        public virtual string IconName { get; set; }

        private object _content;
        private object _badge;
        private ScrollBarVisibility _horizontalScrollBarVisibilityRequirement;
        private ScrollBarVisibility _verticalScrollBarVisibilityRequirement;
        private Thickness _marginRequirement = new Thickness(16);
        private bool _IsEnable = true;
        private Visibility _visibility = Visibility.Visible;
        private string _functionCode;

        public MenuItemVM(string name, string iconName, object content, string functionCode, Visibility visibility = Visibility.Visible)
        {
            _name = name;
            IconName = iconName;
            Content = content;
            _visibility = visibility;
            _functionCode = functionCode;
        }

        public string Name
        {
            get { return _name; }
            set { this.MutateVerbose(ref _name, value, RaisePropertyChanged()); }
        }

        public object Content
        {
            get { return _content; }
            set { this.MutateVerbose(ref _content, value, RaisePropertyChanged()); }
        }
        public object Badge
        {
            get { return _badge; }
            set { this.MutateVerbose(ref _badge, value, RaisePropertyChanged()); }
        }

        public ScrollBarVisibility HorizontalScrollBarVisibilityRequirement
        {
            get { return _horizontalScrollBarVisibilityRequirement; }
            set { this.MutateVerbose(ref _horizontalScrollBarVisibilityRequirement, value, RaisePropertyChanged()); }
        }

        public ScrollBarVisibility VerticalScrollBarVisibilityRequirement
        {
            get { return _verticalScrollBarVisibilityRequirement; }
            set { this.MutateVerbose(ref _verticalScrollBarVisibilityRequirement, value, RaisePropertyChanged()); }
        }

        public Thickness MarginRequirement
        {
            get { return _marginRequirement; }
            set { this.MutateVerbose(ref _marginRequirement, value, RaisePropertyChanged()); }
        }

        public bool IsEnable
        {
            get { return _IsEnable; }
            set { this.MutateVerbose(ref _IsEnable, value, RaisePropertyChanged()); }
        }

        public Visibility Visibility
        {
            get { return _visibility; }
            set { this.MutateVerbose(ref _visibility, value, RaisePropertyChanged()); }
        }

        public string FunctionCode
        {
            get { return _functionCode; }
            set { this.MutateVerbose(ref _functionCode, value, RaisePropertyChanged()); }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
