namespace ISRORCert.Network
{
    public interface IAsyncInterface
    {
        bool OnConnect(AsyncContext context);

        bool OnReceive(AsyncContext context, byte[] buffer, int count);

        void OnDisconnect(AsyncContext context);

        void OnError(AsyncContext context);

        void OnTick(AsyncContext context);
    }
}