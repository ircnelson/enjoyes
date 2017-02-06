using EnjoyES.Stores;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using EnjoyES.Exceptions;

namespace EnjoyES.Defaults.Stores
{
    public class InMemoryStreamStore : IStreamStore<InMemoryStreamRecord>
    {
        private static ConcurrentDictionary<Guid, InMemoryStreamCollection> _items = new ConcurrentDictionary<Guid, InMemoryStreamCollection>();

        public Task AppendStreamAsync(Guid id, string name, long expectedVersion, byte[] content, byte?[] metadata = null)
        {
            CheckExpectedVersion(id, expectedVersion);

            var newStreamRecord = new InMemoryStreamRecord(name, 0, content, metadata);

            _items.AddOrUpdate(id, new InMemoryStreamCollection { newStreamRecord }, (streamId, streamItems) =>
            {
                var version = streamItems.LatestVersion + 1;

                newStreamRecord = new InMemoryStreamRecord(name, version, content, metadata);

                streamItems.Add(newStreamRecord);
                
                return streamItems;
            });

            return Task.CompletedTask;
        }
        
        public Task<IEnumerable<InMemoryStreamRecord>> GetForwardStreamsAsync(Guid id, long version = 1)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<InMemoryStreamRecord>> GetStreamsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<InMemoryStreamRecord>> GetStreamsAsync(Guid id)
        {
            var records = Enumerable.Empty<InMemoryStreamRecord>();

            if (_items.ContainsKey(id))
                records = _items[id].AsEnumerable();
            
            return Task.FromResult(records);
        }

        public Task<IEnumerable<InMemoryStreamRecord>> GetStreamsAsync(Guid id, string name)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
        }

        private void CheckExpectedVersion(Guid id, long expectedVersion)
        {
            if (expectedVersion == ExpectedVersion.Any) return;

            if (!_items.ContainsKey(id) && expectedVersion != ExpectedVersion.NoStream)
            {
                throw new WrongExpectedVersionException(ExpectedVersion.NoStream, expectedVersion);
            }

            else if (_items.ContainsKey(id) && _items[id].LatestVersion != expectedVersion)
            {
                throw new WrongExpectedVersionException(_items[id].LatestVersion, expectedVersion);
            }
        }
    }
}
