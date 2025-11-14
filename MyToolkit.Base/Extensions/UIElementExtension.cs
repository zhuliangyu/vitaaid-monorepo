using MyToolkit.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

namespace MyToolkit.Base.Extensions
{
    public static class UIElementExtension
    {
        public static void SetVisible(this UIElement obj, bool bVisible)
        {
            obj.Visibility = (bVisible) ? Visibility.Visible : Visibility.Hidden;

        }
        /// <summary>
        /// Removes all event handlers subscribed to the specified routed event from the specified element.
        /// </summary>
        /// <param name="element">The UI element on which the routed event is defined.</param>
        /// <param name="routedEvent">The routed event for which to remove the event handlers.</param>

        public static void RemoveRoutedEventHandlers(this UIElement element, RoutedEvent routedEvent)
        {
            // Get the EventHandlersStore instance which holds event handlers for the specified element.
            // The EventHandlersStore class is declared as internal.
            var eventHandlersStoreProperty = typeof(UIElement).GetProperty(
                "EventHandlersStore", BindingFlags.Instance | BindingFlags.NonPublic);
            object eventHandlersStore = eventHandlersStoreProperty.GetValue(element, null);

            if (eventHandlersStore == null) return;

            // Invoke the GetRoutedEventHandlers method on the EventHandlersStore instance 
            // for getting an array of the subscribed event handlers.
            var getRoutedEventHandlers = eventHandlersStore.GetType().GetMethod(
                "GetRoutedEventHandlers", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var routedEventHandlers = (RoutedEventHandlerInfo[])getRoutedEventHandlers.Invoke(
                eventHandlersStore, new object[] { routedEvent });

            // Iteratively remove all routed event handlers from the element.
            foreach (var routedEventHandler in routedEventHandlers)
                element.RemoveHandler(routedEvent, routedEventHandler.Handler);
        }

        private static Action EmptyDelegate = delegate () { };

        public static void Refresh(this UIElement uiElement)
        {
            //uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));
        }

        public static IEnumerable<BindingExpression> EnumerateBindingExressions(this DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            LocalValueEnumerator lve = element.GetLocalValueEnumerator();

            while (lve.MoveNext())
            {
                LocalValueEntry entry = lve.Current;

                if (BindingOperations.IsDataBound(element, entry.Property))
                {
                    yield return (entry.Value as BindingExpression);
                }
            }
        }
        public static IEnumerable<Binding> EnumerateBindings(this DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            LocalValueEnumerator lve = element.GetLocalValueEnumerator();

            while (lve.MoveNext())
            {
                LocalValueEntry entry = lve.Current;

                if (BindingOperations.IsDataBound(element, entry.Property))
                {
                    Binding binding = (entry.Value as BindingExpression).ParentBinding;
                    yield return binding;
                }
            }
        }

        public static List<T> GetVisualChildCollection<T>(this UIElement self) 
        {
            List<T> visualCollection = new List<T>();
            
            Action<UIElement, List<T>> getChildCollection = null;
            getChildCollection = new Action<UIElement, List<T>> ((parent, childCollection) =>
            {
                int count = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    if (child is T)
                    {
                        childCollection.Add((T)(object)child);
                    }
                    else if (child != null && child is UIElement)
                    {
                        getChildCollection.Invoke(child as UIElement, childCollection);
                    }
                }

            });

            getChildCollection.Invoke(self, visualCollection);
            return visualCollection;
        }

    }
}
