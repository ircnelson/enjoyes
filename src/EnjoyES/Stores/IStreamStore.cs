using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnjoyES.Stores
{
    public interface IStreamStore<TStreamRecord> : IDisposable
        where TStreamRecord : IStreamRecord
    {
        /// <summary>
        /// Retrieve all streams.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TStreamRecord>> GetStreamsAsync();

        /// <summary>
        /// Retrieve all streams of an specific stream id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<TStreamRecord>> GetStreamsAsync(Guid id);

        /// <summary>
        /// Retrieve all streams of specific stream id and stream name.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<IEnumerable<TStreamRecord>> GetStreamsAsync(Guid id, string name);

        /// <summary>
        /// Retrieve all forward streams of specific stream id and version.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        Task<IEnumerable<TStreamRecord>> GetForwardStreamsAsync(Guid id, long version = 1);

        /// <summary>
        /// Append an item into specific stream.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="expectedVersion"></param>
        /// <param name="content"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        Task AppendStreamAsync(Guid id, string name, long expectedVersion, byte[] content, byte?[] metadata = null);
    }
}
