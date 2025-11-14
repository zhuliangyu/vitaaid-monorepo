using MySystem.Base;
using MySystem.Base.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MyToolkit.Base.Extensions
{
    public enum TreeTraversalMode
    {
        /// <summary>
        /// Start at root and explore all neightboring nodes, 
        /// then explore each neightbor node in turn folowing same rule (neightboring nodes first)
        /// </summary>
        BreadthFirst = 0,
        /// <summary>
        /// Start at root and explore as far as possible along each branch before backgracking.
        /// </summary>
        DepthFirst,
        /// <summary>
        /// Start at the leaf nodes and explore by moving up the tree
        /// </summary>
        BottomUp,
        /// <summary>
        /// Breadth First is the Default
        /// </summary>
        Default = BreadthFirst
    }
    public static class DependencyObjectExtension
    {
        public static void Refresh(this DependencyObject depobj)
        {
            if (depobj == null)
                return;

            // we are already on a dispatcher thread, dependency object should already be refreshed
            if (depobj.Dispatcher.Thread.ManagedThreadId == Thread.CurrentThread.ManagedThreadId)
                return;

            var are = new AutoResetEvent(false);

            depobj.Dispatcher.Invoke(() => are.Set(), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            are.WaitOne();
        }
        private static void UpdateBindingState(BindingExpression binding, bool bReadOnly)
        {
            if (binding == null) return;
            bool b = false;
            if ((b = binding.ParentBinding?.ValidationRules?.Any() ?? false))
                binding.ParentBinding.ValidationRules[0].ValidatesOnTargetUpdated = !bReadOnly;
            binding.UpdateTarget();
        }
        public static void IsReadOnlyForVisualChildren<T>(this DependencyObject depObj, bool bReadOnly) where T : DependencyObject
        {
            if (depObj != null)
            {
                IEnumerable<T> oChilds = FindVisualDescendants<T>(depObj);
                oChilds.Select(x => x as FrameworkElement).Where(x => x != null)
                       .Action(child =>
                       {
                           if (child.Tag.LogicalEqual("XUI"))
                               return;
                           child.IsInputtable(!bReadOnly);
                           if (child is TextBox)
                               UpdateBindingState(((TextBox)child).GetBindingExpression(TextBox.TextProperty), bReadOnly);
                       });
            }
        }

        public static IEnumerable<DependencyObject> VisualTreeTraversal(
            this DependencyObject me,
            bool includeChildItemsControls = true,
            TreeTraversalMode traversalMode = TreeTraversalMode.BreadthFirst)
        {
            if (includeChildItemsControls)
                return me.TreeTraversal(traversalMode, GetChildrenFuncIncludeChildItemsControls).Skip(1);
            else
                return me.TreeTraversal(traversalMode, GetChildrenFuncExcludeChildItemsControls).Skip(1);
        }

        static IEnumerable<DependencyObject> GetChildrenFuncIncludeChildItemsControls(DependencyObject parent)
        {
            return GetChildrenFunc(parent, includeChildItemsControls: true);
        }

        static IEnumerable<DependencyObject> GetChildrenFuncExcludeChildItemsControls(DependencyObject parent)
        {
            return GetChildrenFunc(parent, includeChildItemsControls: false);
        }

        static IEnumerable<DependencyObject> GetChildrenFunc(
            DependencyObject parent,
            bool includeChildItemsControls = true)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);

            // STEP 1: If no children and parent is a Content Control -> return content
            if (childrenCount == 0 && parent is ContentControl)
            {
                DependencyObject content = (parent as ContentControl).Content as DependencyObject;

                if (content != null)
                {
                    yield return content;
                }
            }

            // STEP 2: Return children
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is ItemsControl && !includeChildItemsControls)
                    continue;

                yield return child;
            }

            yield break;
        }

        public static DependencyObject FindVisualDescendantByType(this DependencyObject depObj, string descendantTypeFullName)
        {
            for (int childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount(depObj); childIndex++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, childIndex);

                if (child != null)
                {
                    if (child.GetType().FullName == descendantTypeFullName)
                    {
                        return child;
                    }

                    var child2 = child.FindVisualDescendantByType(descendantTypeFullName);

                    if (child2 != null)
                        return child2;
                }
            }

            return null;
        }

        public static TDescendant FindVisualDescendant<TDescendant>(
            this DependencyObject depObj,
            string descendantName = "",
            bool includeContentOfContentControls = true) where TDescendant : class
        {
            for (int childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount(depObj); childIndex++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, childIndex);

                if (child != null)
                {
                    if (child is TDescendant)
                    {
                        if (string.IsNullOrEmpty(descendantName)) // if name wasn't provided then we found what we need
                        {
                            return child as TDescendant;
                        }
                        else if (child is FrameworkElement) // if name was provided then check if element has the right name
                        {
                            if ((child as FrameworkElement).Name == descendantName)
                                return child as TDescendant;
                        }
                    }

                    if (includeContentOfContentControls)
                    {
                        var cc = child as ContentControl;
                        if (cc != null)
                        {
                            var contentFe = cc.Content as FrameworkElement;
                            if (contentFe != null)
                            {
                                if (contentFe is TDescendant && contentFe.Name == descendantName)
                                    return contentFe as TDescendant;

                                var contentChild = contentFe.FindVisualDescendant<TDescendant>(descendantName, includeContentOfContentControls);
                                if (contentChild != null)
                                    return (TDescendant)contentChild;
                            }
                        }
                    }

                    var child2 = child.FindVisualDescendant<TDescendant>(descendantName, includeContentOfContentControls);

                    if (child2 != null)
                        return child2 as TDescendant;
                }
            }

            return null;
        }

        public static DependencyObject FindVisualDescendant(this DependencyObject depObj, Func<DependencyObject, bool> isMatchFunc)
        {
            for (int childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount(depObj); childIndex++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, childIndex);

                if (child != null)
                {
                    if (isMatchFunc(child))
                        return child;

                    var child2 = child.FindVisualDescendant(isMatchFunc);

                    if (child2 != null)
                        return child2;
                }
            }

            return null;
        }

        public static TDescendant FindVisualDescendantByDataContext<TDescendant>(this DependencyObject depObj, object dataContext)
            where TDescendant : class
        {
            return depObj.FindVisualDescendant(
                _do =>
                {
                    var fe = _do as FrameworkElement;

                    if (fe == null)
                        return false;

                    if (!(fe is TDescendant))
                        return false;

                    return object.Equals(fe.DataContext, dataContext);
                }) as TDescendant;
        }

        public static DependencyObject FindVisualDescendantByDataContext(this DependencyObject depObj, object dataContext)
        {
            return depObj.FindVisualDescendant(
                _do =>
                {
                    var fe = _do as FrameworkElement;

                    if (fe == null)
                        return false;

                    return object.Equals(fe.DataContext, dataContext);
                });
        }

        public static IEnumerable<TDescendant> FindVisualDescendants<TDescendant>(
            this DependencyObject depObj) where TDescendant : class
        {
            return
                (from c in depObj.VisualTreeTraversal().OfType<TDescendant>()
                 select c);
        }

        /// <summary>
        /// If specified Dependency Object is not a Visual or Visual3D, then try to walk up logical tree until you find a Visual
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static DependencyObject FindNearestVisual(this DependencyObject dobj)
        {
            if (dobj is Visual || dobj is Visual3D)
                return dobj;

            var parent = dobj.GetLogicalParent();

            while (parent != null)
            {
                if (parent is Visual || parent is Visual3D)
                    return parent;

                parent = dobj.GetLogicalParent();
            }

            return null;
        }

        public static bool IsVisualChildOf(this DependencyObject child, DependencyObject potentialParent, DependencyObject stopSearchAt = null)
        {
            child = child.FindNearestVisual();

            DependencyObject parent = child.GetVisualParent();

            while (parent != null && parent != stopSearchAt)
            {
                if (parent == potentialParent)
                    return true;

                parent = parent.GetVisualParent();
            }

            return false;
        }

        public static TParent FindVisualParent<TParent>(this DependencyObject me, DependencyObject stopSearchAt = null)
            where TParent : DependencyObject
        {
            return (TParent)me.FindVisualParent(typeof(TParent), stopSearchAt);
        }

        public static DependencyObject FindVisualParent(this DependencyObject me, Type parentType, DependencyObject stopSearchAt = null)
        {
            me = me.FindNearestVisual();

            DependencyObject parent = me.GetVisualParent();

            while (parent != null && parent != stopSearchAt)
            {
                if (parent.GetType() == parentType || parent.GetType().IsSubclassOf(parentType))
                    return parent;

                parent = parent.GetVisualParent();
            }

            return null;
        }


        public static TParent FindLogicalParent<TParent>(this DependencyObject me)
         where TParent : DependencyObject
        {
            DependencyObject parent = LogicalTreeHelper.GetParent(me);

            while (parent != null)
            {
                TParent typedParent = parent as TParent;

                if (typedParent != null)
                {
                    return typedParent;
                }

                parent = LogicalTreeHelper.GetParent(parent);
            }

            return null;
        }

        public static DependencyObject GetVisualParent(this DependencyObject obj)
        {
            return VisualTreeHelper.GetParent(obj);
        }

        public static DependencyObject GetLogicalParent(this DependencyObject obj)
        {
            return LogicalTreeHelper.GetParent(obj);
        }

        public static DependencyObject GetVisualOrLogicalParent(this DependencyObject sourceElement)
        {
            if (sourceElement is Visual)
            {
                return (VisualTreeHelper.GetParent(sourceElement) ?? LogicalTreeHelper.GetParent(sourceElement));
            }

            return LogicalTreeHelper.GetParent(sourceElement);
        }
       
    }
}
