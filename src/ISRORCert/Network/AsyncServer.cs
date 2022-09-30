using System;
using System.Net;
using System.Net.Sockets;

namespace ISRORCert.Network
{
    public class AsyncServer : AsyncBase
    {
        public void Accept(string host, int port, int outstanding, IAsyncInterface @interface)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            if (!IPAddress.TryParse(host, out var address))
            {
                IPHostEntry host_entry = Dns.GetHostEntry(host);
                address = host_entry.AddressList[0];
            }
            socket.Bind(new IPEndPoint(address, port));

            socket.Listen(outstanding);

            for (int x = 0; x < outstanding; ++x)
            {
                AsyncToken token = new AsyncToken();
                token.Socket = socket;
                token.Interface = @interface;

                SocketAsyncEventArgs acceptEvtArgs = new SocketAsyncEventArgs();
                acceptEvtArgs.UserToken = token;
                acceptEvtArgs.Completed += NetworkOnAccept;
                ProcessAccept(acceptEvtArgs);
            }
        }

        private void DispatchAccept(object param)
        {
            SocketAsyncEventArgs e = (SocketAsyncEventArgs)param;

            NetworkOnAccept(null, e);
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            AsyncToken token = (AsyncToken)e.UserToken;

            e.AcceptSocket = null;

            if (!token.Socket.AcceptAsync(e))
            {
                ThreadPool.QueueUserWorkItem(DispatchAccept, e);
            }
        }

        private void NetworkOnAccept(object sender, SocketAsyncEventArgs e)
        {
            AsyncToken token = (AsyncToken)e.UserToken;

            Socket socket = e.AcceptSocket;

            ProcessAccept(e); // Start the next accept asap.

            if (socket == null)
            {
                return; // Ignore errors because there is nothing to do
            }

            AsyncState state = new AsyncState(this, socket, AsyncOperation.Accept, token.Interface); // Now handle the current connection.

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

                state.Cleanup(); // Cleanup the socket

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