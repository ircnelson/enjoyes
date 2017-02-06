using EnjoyES.Stores;

namespace EnjoyES.Defaults.Stores
{
    public class InMemoryStreamStoreFactory : IStreamStoreFactory<InMemoryStreamStore, InMemoryStreamRecord>
    {
        public InMemoryStreamStore Create()
        {
            return new InMemoryStreamStore();
        }
    }
}
