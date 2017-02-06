using System;

namespace EnjoyES.Stores
{
    public interface IStreamRecord
    {
        /// <summary>
        /// Unique Identifier.
        /// </summary>
        Guid Id { get; }
        
        /// <summary>
        /// Stream name.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Byte array of event content.
        /// </summary>
        byte[] Content { get; }

        /// <summary>
        /// Byte array of evet metadata.
        /// </summary>
        byte?[] Metadata { get; }
    }
}
