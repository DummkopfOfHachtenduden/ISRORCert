using System;
using System.Net;
using System.Net.Sockets;

namespace ISRORCert.Network
{
    public class AsyncClient : AsyncBase
    {

        public void Connect(string host, int port, IAsyncInterface @interface)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress address = null;
            if (!IPAddress.TryParse(host, out address))
            {
                IPHostEntry host_entry = Dns.GetHostEntry(host);
                address = host_entry.AddressList[0];
            }

            AsyncToken token = new AsyncToken();
            token.Socket = socket;
            token.Interface = @interface;

            SocketAsyncEventArgs connectEvtArgs = new SocketAsyncEventArgs();
            connectEvtArgs.UserToken = token;
            connectEvtArgs.Completed += NetworkOnConnect;
            connectEvtArgs.RemoteEndPoint = new IPEndPoint(address, port);
            ProcessConnect(connectEvtArgs);
        }

        private void ProcessConnect(SocketAsyncEventArgs e)
        {
            AsyncToken token = (AsyncToken)e.UserToken;

            if (!token.Socket.ConnectAsync(e))
            {
                NetworkOnConnect(null, e);
            }
        }

        private void NetworkOnConnect(object sender, SocketAsyncEventArgs e)
        {
            AsyncToken token = (AsyncToken)e.UserToken;

            Socket socket = e.ConnectSocket;

            if (socket == null) // The connection failed
            {
                token.Interface.OnError(null); // There is no state object yet, so pass the user object
                return;
            }

            AsyncState state = new AsyncState(this, socket, AsyncOperation.Connect, token.Interface); // Now handle the current connection.

            bool result = false;
            try
            {
                result = state.Context.Interface.OnConnect(state.Context);
            }
            catch (Exception) { }

            if (!result)
            {
                try
                {
                    state.Context.Interface.OnError(state.Context); // Ensure the user can cleanup anything before the object dies
                }
                catch (Exception) { }

                state.Cleanup(); // Cleanup the object
                return;
            }

            try
            {
                state.Read(); // Begin receiving data on the socket
            }
            catch (Exception)
            {
                state.Cleanup(); // Cleanup the object
                return;
            }

            AddState(state); // Store the state to keep it alive
        }
    }
}