using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace MyToolkit.Base.Extensions
{
    public static class ProgressBarExtension
    {
        public static readonly DependencyProperty SmoothProgressProperty = DependencyProperty.RegisterAttached("SmoothProgress",
            typeof(double), typeof(ProgressBarExtension), new UIPropertyMetadata(0.0, OnSmoothProgressChanged));

        public static void SetSmoothProgress(FrameworkElement target, double value)
        {
            target.SetValue(SmoothProgressProperty, value);
        }

        public static double GetSmoothProgress(FrameworkElement target)
        {
            return (double)target.GetValue(SmoothProgressProperty);
        }

        private static void OnSmoothProgressChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var progressBar = target as ProgressBar;
            if (progressBar != null)
            {
                progressBar.SetCurrentValue(RangeBase.ValueProperty, (double)e.NewValue);
            }
        }

        public static void UpdateProgress(this ProgressBar progressBar, int currentItem, int totalItems)
        {
            progressBar.SetCurrentValue(RangeBase.MinimumProperty, (double)0);
            progressBar.SetCurrentValue(RangeBase.MaximumProperty, (double)totalItems);

            var progressAnimation = new DoubleAnimation();
            progressAnimation.From = progressBar.Value;
            progressAnimation.To = currentItem;
            progressAnimation.DecelerationRatio = .2;
            progressAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(300));

            var storyboard = new Storyboard();
            storyboard.Children.Add(progressAnimation);
            Storyboard.SetTarget(progressAnimation, progressBar);
            Storyboard.SetTargetProperty(progressAnimation, new PropertyPath(SmoothProgressProperty));
            storyboard.Begin();
        }
    }
}
