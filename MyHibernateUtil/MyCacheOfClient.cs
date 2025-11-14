using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyHibernateUtil
{
    public class MyCacheOfClient : IFlushEventListener
    {
        private IList<IMyCache> CacheOfClient = new List<IMyCache>();
        public void Register(IMyCache myCache) => CacheOfClient.Add(myCache);
        public void Clear()
        {
            foreach (var cache in CacheOfClient)
                cache.Clear();
        }

        public Task OnFlushAsync(FlushEvent @event, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void OnFlush(FlushEvent @event)
        {
            Clear();
        }
    }
}
