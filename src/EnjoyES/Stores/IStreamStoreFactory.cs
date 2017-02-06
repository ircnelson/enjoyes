namespace EnjoyES.Stores
{
    public interface IStreamStoreFactory<out TStreamStore, TStreamRecord>
        where TStreamStore : IStreamStore<TStreamRecord>
        where TStreamRecord : IStreamRecord
    {
        TStreamStore Create();
    }
}
