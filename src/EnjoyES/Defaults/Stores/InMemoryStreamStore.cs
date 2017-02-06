using EnjoyES.Stores;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnjoyES.Exceptions;

namespace EnjoyES.Defaults.Stores
{
    public class InMemoryStreamStore : IStreamStore<InMemoryStreamRecord>
    {
        private static ConcurrentDictionary<string, InMemoryStreamCollection> _items = new ConcurrentDictionary<string, InMemoryStreamCollection>();

        public Task AppendStreamAsync(string stream, long expectedVersion, params InMemoryStreamRecord[] records)
        {
            CheckExpectedVersion(stream, expectedVersion);
            
            _items.AddOrUpdate(stream, new InMemoryStreamCollection(records.ToArray()), (streamId, streamItems) =>
            {
                streamItems.AddRange(records);

                return streamItems;
            });
            
            return Task.CompletedTask;
        }
        
        public Task<IEnumerable<InMemoryStreamRecord>> GetForwardStreamsAsync(string stream, long version = 1)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<InMemoryStreamRecord>> GetStreamsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<InMemoryStreamRecord>> GetStreamsAsync(string stream)
        {
            var records = Enumerable.Empty<InMemoryStreamRecord>();

            if (_items.ContainsKey(stream))
                records = _items[stream].AsEnumerable();
            
            return Task.FromResult(records);
        }
        
        public void Dispose()
        {
        }

        private void CheckExpectedVersion(string stream, long expectedVersion)
        {
            if (expectedVersion == ExpectedVersion.Any) return;

            if (!_items.ContainsKey(stream) && expectedVersion != ExpectedVersion.NoStream)
            {
                throw new ConcurrencyException(ExpectedVersion.NoStream, expectedVersion);
            }

            else if (_items.ContainsKey(stream) && _items[stream].LatestVersion != expectedVersion)
            {
                throw new ConcurrencyException(_items[stream].LatestVersion, expectedVersion);
            }
        }
    }
}
