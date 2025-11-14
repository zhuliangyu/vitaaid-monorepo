using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyToolkit.Base.Converters
{
    public sealed class BooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public BooleanToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed)
        { }
    }

    public sealed class NullToVisibilityConverter : NullObjConverter<Visibility>
    {
        public NullToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed)
        { }
    }
}
