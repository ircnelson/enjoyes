using EnjoyES.Stores;

namespace EnjoyES.Defaults.Stores
{
    public class InMemoryStreamRecord : IStreamRecord
    {
        public string Name { get; }
        public long Version { get; }
        public byte[] Content { get; }
        public byte?[] Metadata { get; }
        
        public InMemoryStreamRecord(string name, long version, byte[] content, byte?[] metadata = null)
        {
            Name = name;
            Version = version;
            Content = content;
            Metadata = metadata;
        }
    }
}
