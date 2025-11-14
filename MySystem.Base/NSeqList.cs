using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MySystem.Base
{
    [Serializable]
    public class NSeqList<T> : ICollection<T>
    {
        protected IList<T> _innerList;
        public virtual IList<T> innerList
        {
            get
            {
                if (_innerList == null)
                    _innerList = new List<T>();
                return _innerList;
            }
            set { _innerList = value; }
        }

        public void ReassignSeq()
        {
            try
            {
                ReassignSeq(innerList);
            }
            catch (Exception) { throw; }
        }

        public static void ReassignSeq(IList<T> itemList)
        {
            try
            {
                if (typeof(IMESSequence).IsAssignableFrom(typeof(T)))
                {
                    for (int i = 1; i <= itemList.Count; i++)
                    {
                        IMESSequence si = (IMESSequence)(Object)(itemList[i - 1]);
                        si.Sequence = i;
                    }
                }
            }
            catch (Exception) { throw; }
        }

        public void Swap(int idx1, int idx2)
        {
            try
            {
                if (idx1 == idx2) return;
                if (idx1 < 0 || idx1 >= _innerList.Count()) return;
                if (idx2 < 0 || idx2 >= _innerList.Count()) return;

                if (typeof(IMESSequence).IsAssignableFrom(typeof(T)))
                {
                    T tmp = _innerList[idx1];
                    _innerList[idx1] = _innerList[idx2];
                    _innerList[idx2] = tmp;
                    ReassignSeq();
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public void Add(T item)
        {
            try
            {
                if (typeof(IMESSequence).IsAssignableFrom(typeof(T)))
                {
                    IMESSequence si = (IMESSequence)(Object)item;
                    si.Sequence = innerList.Count + 1;
                    innerList.Add(item);
                }
                else
                    innerList.Add(item);
            }
            catch (Exception ex) { throw ex; }
        }
        public void AddRange(IList<T> items)
        {
            try
            {
                foreach (var item in items) innerList.Add(item);
                ReassignSeq();
            }
            catch (Exception ex) { throw ex; }
        }

        public void Insert(int index, T item)
        {
            try
            {
                innerList.Insert(index, item);
                ReassignSeq();
            }
            catch (Exception ex) { throw ex; }
        }

        public void Clear()
        {
            innerList.Clear();
        }

        public bool Contains(T item)
        {
            return innerList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            innerList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return innerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return innerList.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            try
            {
                bool bRtn = false;
                bRtn = innerList.Remove(item);
                if (bRtn)
                    ReassignSeq();
                return bRtn;
            }
            catch (Exception ex) { throw ex; }
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return innerList.GetEnumerator();
        }
        public T this[int index]
        {
            get { return innerList[index]; }
            set { innerList[index] = value; }
        }
        public virtual NSeqList<T> ShallowCopy()
        {
            return (NSeqList<T>)this.MemberwiseClone();
        }
    }
}
