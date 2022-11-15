using System;
using System.Net;
using System.Net.Sockets;

namespace ISRORCert.Network
{
    public class AsyncState
    {
        private Socket m_socket;
        private AsyncBase m_server;

        private SocketAsyncEventArgs m_read_event_args;
        private SocketAsyncEventArgs m_write_event_args;

        private byte[] m_read_buffer;

        private Queue<AsyncBuffer> m_write_buffers;
        private AsyncBuffer? m_current_write_buffer;

        public AsyncContext Context { get; set; }
        public AsyncOperation Operation { get; }

        public EndPoint? EndPoint => m_socket?.RemoteEndPoint;

        public AsyncState(AsyncBase server, Socket socket, AsyncOperation operation, IAsyncInterface @interface)
        {
            m_server = server;
            m_socket = socket;
            Operation = operation;

            m_current_write_buffer = null;

            m_read_buffer = new byte[8192];

            m_read_event_args = new SocketAsyncEventArgs();
            m_read_event_args.Completed += OnIO;
            m_read_event_args.UserToken = this;
            m_read_event_args.SetBuffer(m_read_buffer, 0, m_read_buffer.Length);

            m_write_event_args = new SocketAsyncEventArgs();
            m_write_event_args.Completed += OnIO;
            m_write_event_args.UserToken = this;

            m_write_buffers = new Queue<AsyncBuffer>();

            Context = new AsyncContext
            {
                State = this,
                Interface = @interface,
            };
        }

        internal void Disconnect()
        {
            try
            {
                m_socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception) { }
        }

        internal void Cleanup()
        {
            try
            {
                m_socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception) { }

            try
            {
                m_socket.Close();
            }
            catch (Exception) { }
        }

        private void OnIO(object? sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessRecv(e);
                    break;

                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;

                default:
                    throw new NotImplementedException("The code will only handle send and receive operations.");
            }
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            AsyncState state = e.UserToken as AsyncState;

            if (state.ProcessWrite(e))
            {
                return;
            }

            m_server.RemoveState(this);
        }

        private void ProcessRecv(SocketAsyncEventArgs e)
        {
            AsyncState state = e.UserToken as AsyncState;

            if (state.ProcessRead(e))
            {
                return;
            }

            m_server.RemoveState(this);
        }

        internal void Read()
        {
            if (!m_socket.ReceiveAsync(m_read_event_args))
            {
                ProcessRecv(m_read_event_args);
            }
        }

        private bool ProcessRead(SocketAsyncEventArgs e)
        {
            try
            {
                if (e.BytesTransferred <= 0 || e.SocketError != SocketError.Success)
                {
                    try
                    {
                        Context.Interface.OnDisconnect(Context);
                    }
                    catch (Exception) { }
                    Cleanup();
                    return false;
                }

                bool result = false;
                try
                {
                    result = Context.Interface.OnReceive(Context, m_read_buffer, e.BytesTransferred);
                }
                catch (Exception) { }

                if (!result)
                {
                    try
                    {
                        Context.Interface.OnDisconnect(Context);
                    }
                    catch (Exception) { }
                    Cleanup();
                    return false;
                }

                Read();
            }
            catch (Exception)
            {
                Cleanup();
                return false;
            }
            return true;
        }

        private void DispatchWrite()
        {
            if (!m_socket.SendAsync(m_write_event_args)) // Attempt the write
            {
                ProcessSend(m_write_event_args); // It completed immediately, we must manually invoke the handler
            }
        }

        private void CheckWrite()
        {
            lock (m_write_buffers)
            {
                if (m_current_write_buffer == null) // If no write is in progress
                {
                    if (m_write_buffers.Count > 0) // There is pending data
                    {
                        m_current_write_buffer = m_write_buffers.Dequeue(); // Get the next buffer

                        m_write_event_args.SetBuffer(m_current_write_buffer.Buffer, m_current_write_buffer.Offset, m_current_write_buffer.Count); // Setup the async write

                        DispatchWrite(); // Begin the write
                    }
                }
            }
        }

        private bool ProcessWrite(SocketAsyncEventArgs e)
        {
            try
            {
                if (e.BytesTransferred <= 0 || e.SocketError != SocketError.Success) // Check for errors
                {
                    try
                    {
                        Context.Interface.OnDisconnect(Context);
                    }
                    catch (Exception) { }
                    Cleanup();
                    return false;
                }

                lock (m_write_buffers)
                {
                    m_current_write_buffer.Offset += e.BytesTransferred; // Update index
                    m_current_write_buffer.Count -= e.BytesTransferred; // Update count

                    if (m_current_write_buffer.Count > 0) // If there is data left to be sent
                    {
                        m_write_event_args.SetBuffer(m_current_write_buffer.Offset, m_current_write_buffer.Count); // Setup the next async write
                        DispatchWrite(); // Begin the next write
                        return true; // Everything is fine
                    }

                    m_current_write_buffer = null; // Clear out the last write object
                    CheckWrite(); // Perform the logic to check for the next write
                }
            }
            catch (Exception)
            {
                Cleanup();
                return false;
            }
            return true; // Everything is fine
        }

        internal void Write(AsyncBuffer buffer)
        {
            lock (m_write_buffers)
            {
                m_write_buffers.Enqueue(buffer); // Save the buffer

                CheckWrite(); // Perform the logic to check for the next write. NOTE: This stays inside the lock to keep FIFO order.
            }
        }
    }
}