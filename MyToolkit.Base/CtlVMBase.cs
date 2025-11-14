using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using MySystem.Base;

namespace MyToolkit.Base
{
    public class CtlVMBase<T> where T : new()
    {
        public ObservableCollection<T> Items { get; set; }
        public virtual T SelectedObj { get; set; }
        protected IList<T> _deletedObjs = new List<T>();
        public IList<T> deletedObjs { get { return _deletedObjs; } }
        protected IList<T> _dirtyObjs = new List<T>();
        public IList<T> dirtyObjs { get { return _dirtyObjs; } }

        public virtual bool bReadOnly { get; set; }

        T t = new T();

        private eOPSTATE _eState = eOPSTATE.INIT;
        public eOPSTATE eState
        {
            get
            {
                return _eState;
            }
            set
            {
                _eState = value;
                ObjStateMachine();
            }
        }

        public virtual void New_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                T newObj = new T();
                Items.Add(newObj);
                eState = eOPSTATE.NEW;
                OnAfterNew(sender, e);
            }
            catch (Exception ex) { throw ex; }
        }

        public virtual void OnAfterNew(object sender, RoutedEventArgs e)
        { try { } catch (Exception ex) { throw ex; } }

        public virtual void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex) { throw ex; }
        }

        public virtual void Saving_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex) { throw ex; }
        }

        public virtual void Undo_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex) { throw ex; }
        }

        public virtual void View_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex) { throw ex; }
        }

        public virtual void Deleting_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex) { throw ex; }
        }


        public virtual void ObjStateMachine()
        {
            try
            {
                bReadOnly = true;
                switch (eState)
                {
                    case eOPSTATE.NEW:
                        bReadOnly = false;
                        OnNewing();
                        break;
                    case eOPSTATE.DIRTY:
                        bReadOnly = false;
                        OnEditing();
                        break;
                    case eOPSTATE.INIT:
                        bReadOnly = true;
                        OnViewing();
                        break;
                    case eOPSTATE.DELETE:
                        bReadOnly = false;
                        OnDeleting();
                        break;
                }
                OnStateChange();
            }
            catch (Exception ex) { throw ex; }
        }

        public virtual void OnStateChange()
        {
            try
            {
            }
            catch (Exception ex) { throw ex; }
        }
        public virtual void OnNewing()
        {
            try
            {
            }
            catch (Exception ex) { throw ex; }
        }
        public virtual void OnEditing()
        {
            try
            {
            }
            catch (Exception ex) { throw ex; }
        }
        public virtual void OnViewing()
        {
            try
            {
            }
            catch (Exception ex) { throw ex; }
        }
        public virtual void OnDeleting()
        {
            try
            {
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
