using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace MyToolkit.Base.Helpers
{
    public static class DispatcherHelper
    {
        public static void DoAction(bool IsAsync, DispatcherPriority priority, Action callback)
        {
            if (IsAsync)
                Application.Current.Dispatcher.BeginInvoke(callback, priority, null);
            else
                callback();
        }
    }
}
