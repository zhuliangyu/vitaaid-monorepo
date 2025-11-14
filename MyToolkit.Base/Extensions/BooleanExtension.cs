using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyToolkit.Base.Extensions
{
    public static class BooleanExtension
    {
        public static Visibility ToVisibility(this Boolean self, Visibility FalseBehavior = Visibility.Hidden) => (self) ? Visibility.Visible : FalseBehavior;
    }
}
