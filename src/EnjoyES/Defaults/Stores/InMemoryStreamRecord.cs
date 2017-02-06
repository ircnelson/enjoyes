using System;
using EnjoyES.Stores;

namespace EnjoyES.Defaults.Stores
{
    public class InMemoryStreamRecord : IStreamRecord
    {
        public Guid Id { get; }
        public string Type { get; }
        public byte[] Content { get; }
        public byte?[] Metadata { get; }

        public InMemoryStreamRecord(Guid id, string type, byte[] content, byte?[] metadata = null)
        {
            Id = id;
            Type = type;
            Content = content;
            Metadata = metadata;
        }
    }
}
