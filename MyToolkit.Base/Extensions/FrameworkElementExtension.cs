using MySystem.Base.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace MyToolkit.Base.Extensions
{
    public static class FrameworkElementExtension
    {
        public static BindingExpressionBase SetBinding(this FrameworkElement self, DependencyProperty dp, object source, string propertyPath, BindingMode mode = BindingMode.TwoWay, bool NotifyOnTargetUpdated = false, ValidationRule rule = null, UpdateSourceTrigger updateSourceTrigger = UpdateSourceTrigger.Default)
        {
            Binding oBinding = new Binding();
            oBinding.Source = source;
            oBinding.Path = new PropertyPath(propertyPath);
            oBinding.Mode = mode;
            oBinding.NotifyOnTargetUpdated = NotifyOnTargetUpdated;
            oBinding.UpdateSourceTrigger = updateSourceTrigger; 
            if (rule != null)
            {
                oBinding.ValidationRules.Add(rule);
                oBinding.ValidatesOnDataErrors = true;
                oBinding.NotifyOnValidationError = true;
            }
            return BindingOperations.SetBinding(self, dp, oBinding);
        }
        public static FrameworkElement IsInputtable(this FrameworkElement self, bool bInputtable)
        {
            if (self is TextBox)
                ((TextBox)self).IsReadOnly = !bInputtable;
            else if (self is ComboBox)
                ((ComboBox)self).IsHitTestVisible = bInputtable;
            else if (self is ToggleButton)
                ((ToggleButton)self).IsHitTestVisible = bInputtable;
            else if (self is Button)
                ((Button)self).IsEnabled = bInputtable;
            else if (self is TextBoxBase)
                ((TextBoxBase)self).IsReadOnly = !bInputtable;
            else if (self is DataGrid)
                ((DataGrid)self).IsEnabled = bInputtable;
            else if (self is UIElement)
                ((UIElement)self).IsEnabled = bInputtable;
            return self;
        }
    }
}
