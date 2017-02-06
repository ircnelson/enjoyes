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
        /// <param name="stream"></param>
        /// <returns></returns>
        Task<IEnumerable<TStreamRecord>> GetStreamsAsync(string stream);
        
        /// <summary>
        /// Retrieve all forward streams of specific stream id and version.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        Task<IEnumerable<TStreamRecord>> GetForwardStreamsAsync(string stream, long version = 1);

        /// <summary>
        /// Append an item into specific stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="expectedVersion"></param>
        /// <param name="content"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        Task AppendStreamAsync(string stream, long expectedVersion, params TStreamRecord[] records);
    }
}
