namespace EnjoyES.Stores
{
    public interface IStreamRecord
    {
        /// <summary>
        /// Stream Version.
        /// </summary>
        long Version { get; }

        /// <summary>
        /// Stream name.
        /// </summary>
        string Name { get; }

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
