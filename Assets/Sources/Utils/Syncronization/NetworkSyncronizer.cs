using Utils.DataStructure;

namespace Utils.Syncronizer
{
    public class NetworkSyncronizer<T>
    {
        private readonly TwinReadWriteQueue<T> _queue;

        public NetworkSyncronizer()
        {
            _queue = new TwinReadWriteQueue<T>();
        }

        public void BeginRead()
        {
            _queue.Flip();
        }

        public void PutRawBytes(byte[] bytes)
        {
            //TODO
        }
    }
}
