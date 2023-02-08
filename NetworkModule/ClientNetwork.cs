using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PacketBase;

/// <summary>
/// Client에 붙이게 될 부분.
/// Game Server는 DB Server와도 연동이 되어야 하기 때문에 Server와 Client 양쪽 역할을 동시에 해야한다.
/// </summary>

namespace NetworkModule
{
    public class ClientNetwork
    {
        private Socket m_cbSocket;
        private byte[] m_recvBuffer;
        private const int MAXSIZE = 4096;
        private string m_host;
        private int m_port;

        // Network에서 전송결과를 담을 큐
        public Queue<string> networkMessage;

        // 서버에서 전송받은 데이터를 담을 큐
        public Queue<byte[]> dataQueue;

        public Socket ClientSocket
        {
            get;
            private set;
        }

        public void Init(string host, int port)
        {
            m_recvBuffer = new byte[MAXSIZE];
            m_host = host;
            m_port = port;

            networkMessage = new Queue<string>();
            dataQueue = new Queue<byte[]>();

            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Connect();
        }

        private void Connect()
        {
            networkMessage.Enqueue("서버 접속 대기중 ...");

            try
            {
                ClientSocket.BeginConnect(m_host, m_port, new AsyncCallback(ConnectCallBack), ClientSocket);
            }
            catch (SocketException e)
            {
                networkMessage.Enqueue("접속실패");
                Connect();
                networkMessage.Enqueue(e.Message);
            }
        }

        private void ConnectCallBack(IAsyncResult IAR)
        {
            try
            {
                Socket tempSocket = (Socket)IAR.AsyncState;
                IPEndPoint ipEp = (IPEndPoint)tempSocket.RemoteEndPoint;

                networkMessage.Enqueue("접속성공");

                tempSocket.EndConnect(IAR);
                m_cbSocket = tempSocket;
                m_cbSocket.BeginReceive(m_recvBuffer, 0, m_recvBuffer.Length, SocketFlags.None,
                    new AsyncCallback(OnReceiveCallBack), m_cbSocket);
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.NotConnected)
                {
                    networkMessage.Enqueue("접속실패");
                    Connect();
                }
            }
        }

        public void Receive()
        {
            m_cbSocket.BeginReceive(m_recvBuffer, 0, m_recvBuffer.Length, SocketFlags.None,
                new AsyncCallback(OnReceiveCallBack), m_cbSocket);
        }

        private void OnReceiveCallBack(IAsyncResult IAR)
        {
            try
            {
                Socket tempSock = (Socket)IAR.AsyncState;
                int readSize = tempSock.EndReceive(IAR);
                
                // 서버로부터 전송받은 데이터가 있을경우 데이터큐에 넣어준다.
                if (readSize != 0)
                {
                    networkMessage.Enqueue("데이터 수신");

                    dataQueue.Enqueue(m_recvBuffer);
                }
                Receive();
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.ConnectionReset)
                {
                    Connect();
                }
            }
        }

        public void PacketSend(GamePacket packet)
        {
            SocketAsyncEventArgs sendEventArgs = new SocketAsyncEventArgs();

            sendEventArgs.Completed += SendCompleted;
            sendEventArgs.UserToken = this;

            byte[] body = GamePacket.Serialize(packet);
            byte[] header = BitConverter.GetBytes(body.Length);

            byte[] sendData = new byte[body.Length + header.Length];

            Array.Copy(header, 0, sendData, 0, header.Length);
            Array.Copy(body, 0, sendData, header.Length, body.Length);

            sendEventArgs.SetBuffer(sendData, 0, sendData.Length);

            bool pending = ClientSocket.SendAsync(sendEventArgs);

            if (!pending)
            {
                SendCompleted(null, sendEventArgs);
            }
        }

        private void SendCompleted(object sender, SocketAsyncEventArgs e)
        {
            networkMessage.Enqueue("패킷 전송에 성공하였습니다.");
        }
    }
}
