using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PacketBase;

/// <summary>
/// DB Server, Game Server에 붙이게 될 부분.
/// </summary>

namespace NetworkModule
{
    public struct ReceiveData
    {
        public Socket clientSocket;
        public byte[] clientData;
    }

    public class ServerNetwork
    {
        private List<Socket> m_clientSocketList;
        private byte[] m_data;

        // Network에서 전송결과를 담을 큐
        public Queue<string> networkMessage;

        // 클라이언트에서 온 데이터를 담을 큐
        public Queue<ReceiveData> dataQueue;

        public Socket ServerSocket
        {
            get;
            private set;
        }

        // 서버 소켓 생성 및 클라이언트 소켓 바인드 비동기 대기
        public void Init(IPAddress hostAddress, int port, int backlog)
        {
            m_clientSocketList = new List<Socket>();
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ipEp = new IPEndPoint(hostAddress, port);

            ServerSocket.Bind(ipEp);
            ServerSocket.Listen(backlog);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptCompleted);
            ServerSocket.AcceptAsync(args);

            dataQueue = new Queue<ReceiveData>();
            networkMessage = new Queue<string>();
        }

        // 클라이언트 접속 수락 callback 함수
        private void AcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            bool isDuplication = false;

            foreach (var client in m_clientSocketList)
            {
                // 중복 접속 관련 처리 부분
                IPEndPoint ipEp1 = (IPEndPoint)client.RemoteEndPoint;
                IPEndPoint ipEp2 = (IPEndPoint)e.AcceptSocket.RemoteEndPoint;

                if (Equals(ipEp1.Address, ipEp2.Address))
                {
                    client.Disconnect(false);
                    isDuplication = true;
                }
            }

            Socket clientSocket = e.AcceptSocket;
            ServerPacket packet = new ServerPacket
            {
                packetId = (short)PacketId.ConnectResult
            };

            if (isDuplication)
            {
                networkMessage.Enqueue(clientSocket.RemoteEndPoint.ToString() + " IP 중복 접속");
                packet.packetError = (short)ErrorCode.ConnectFailure;
                packet.ServerResult = false;
            }
            else
            {
                m_clientSocketList.Add(clientSocket);
                networkMessage.Enqueue(clientSocket.RemoteEndPoint.ToString() + " 접속");
                packet.packetError = (short)ErrorCode.None;
                packet.ServerResult = true;
            }

            PacketSend(clientSocket, packet);


            if (m_clientSocketList != null)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                m_data = new byte[1024];
                args.SetBuffer(m_data, 0, 1024);
                args.UserToken = m_clientSocketList;
                args.Completed += new EventHandler<SocketAsyncEventArgs>(ReceiveCompleted);
                clientSocket.ReceiveAsync(args);
            }

            e.AcceptSocket = null;
            ServerSocket.AcceptAsync(e);
        }

        // 데이터 수신 callback 함수
        public void ReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            Socket clientSocket = (Socket)sender;

            if (clientSocket.Connected && e.BytesTransferred > 0)
            {
                m_data = e.Buffer;

                ReceiveData rd = new ReceiveData
                {
                    clientSocket = clientSocket,
                    clientData = new byte[e.BytesTransferred]
                };

                Buffer.BlockCopy(m_data, 0, rd.clientData, 0, e.BytesTransferred);

                dataQueue.Enqueue(rd);

                // 데이터 수신 byte배열 초기화
                for (int i = 0; i < m_data.Length; i++)
                {
                    m_data[i] = 0;
                }

                e.SetBuffer(m_data, 0, 1024);
                clientSocket.ReceiveAsync(e);
            }
            else
            {
                clientSocket.Disconnect(false);
                m_clientSocketList.Remove(clientSocket);

                networkMessage.Enqueue(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss\n") + clientSocket.RemoteEndPoint.ToString() + "의 연결이 끊어졌습니다.");
            }
        }

        public void PacketSend(Socket clientSocket, GamePacket packet)
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

            bool pending = clientSocket.SendAsync(sendEventArgs);

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
