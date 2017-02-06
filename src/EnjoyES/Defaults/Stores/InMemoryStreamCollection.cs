﻿using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace EnjoyES.Defaults.Stores
{
    internal class InMemoryStreamCollection : IEnumerable<InMemoryStreamRecord>
    {
        private readonly ConcurrentBag<InMemoryStreamRecord> _collection = new ConcurrentBag<InMemoryStreamRecord>();

        public long LatestVersion { get; private set; } = -1;

        public InMemoryStreamCollection(params InMemoryStreamRecord[] items)
        {
            foreach (var item in items)
            {
                _collection.Add(item);
            }
        }

        public void Add(InMemoryStreamRecord item)
        {
            _collection.Add(item);

            LatestVersion = item.Version;
        }

        public IEnumerator<InMemoryStreamRecord> GetEnumerator()
        {
            return _collection
                .OrderBy(o => o.Version)
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
