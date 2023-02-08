using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NetworkModule;
using PacketBase;

namespace GameServer
{
    class MainServer
    {
        public ServerNetwork ServerNet
        {
            get;
            private set;
        }

        public void Init(int port, int backlog)
        {
            ServerNet = new ServerNetwork();
            ServerNet.Init(IPAddress.Any, port, backlog);
        }

        public void PacketSendToClient(Socket socket, GamePacket packet)
        {
            ServerNet.PacketSend(socket, packet);
        }

        // 패킷 들어온게 있는지 확인 후 손실여부 체크
        public void ReceivePacket()
        {
            if (ServerNet.dataQueue.Count != 0)
            {
                ReceiveData rd = ServerNet.dataQueue.Dequeue();

                byte[] header = new byte[PacketDefine.HEADERSIZE];
                Buffer.BlockCopy(rd.clientData, 0, header, 0, PacketDefine.HEADERSIZE);

                int bodySize = BitConverter.ToInt32(header, 0);
                byte[] body = new byte[bodySize];

                Buffer.BlockCopy(rd.clientData, PacketDefine.HEADERSIZE, body, 0, bodySize);

                ServerNet.networkMessage.Enqueue(rd.clientSocket.RemoteEndPoint + "의 수신 패킷 분석");
                PacketAnalyze(rd.clientSocket, body);
            }
        }

        // 패킷 분석하여 DB에 접근하는 부분
        private void PacketAnalyze(Socket clientSocket, byte[] data)
        {
            GamePacket packet = (GamePacket)GamePacket.Deserialize(data);

            ServerPacket resultPacket = new ServerPacket();

            switch (packet.packetId)
            {
                case (short)PacketBase.PacketId.ReqLogin:
                    // 로그인
                    LoginPacket loginPacket = (LoginPacket)LoginPacket.Deserialize(data);
                    bool loginResult = DBManager.Instance.Login(loginPacket.UserId, loginPacket.UserPw);

                    if (loginResult)
                    {
                        resultPacket.packetError = (short)ErrorCode.None;
                        resultPacket.ServerResult = true;
                        ServerNet.networkMessage.Enqueue(clientSocket.RemoteEndPoint + "의 로그인 성공");
                    }
                    else
                    {
                        resultPacket.packetError = (short)ErrorCode.LoginFailure;
                        resultPacket.ServerResult = false;
                        ServerNet.networkMessage.Enqueue(clientSocket.RemoteEndPoint + "의 로그인 실패");
                    }

                    resultPacket.packetId = (short)PacketId.ResLogin;
                    break;

                case (short)PacketBase.PacketId.ReqIdCheck:
                    // 아이디 중복 확인
                    IdCheckPacket checkPacket = (IdCheckPacket)IdCheckPacket.Deserialize(data);
                    bool checkResult = DBManager.Instance.IdDuplicateCheck(checkPacket.UserId);

                    if (!checkResult)
                    {
                        resultPacket.packetError = (short)ErrorCode.None;
                        resultPacket.ServerResult = true;
                        ServerNet.networkMessage.Enqueue(clientSocket.RemoteEndPoint + "의 아이디중복검사 성공");
                    }
                    else
                    {
                        resultPacket.packetError = (short)ErrorCode.IdCheckFailure;
                        resultPacket.ServerResult = false;
                        ServerNet.networkMessage.Enqueue(clientSocket.RemoteEndPoint + "의 아이디중복검사 실패");
                    }

                    resultPacket.packetId = (short)PacketId.ResIdCheck;
                    break;

                case (short)PacketBase.PacketId.ReqSignUp:
                    // 회원가입
                    SignUpPacket signUpPacket = (SignUpPacket)SignUpPacket.Deserialize(data);

                    bool signUpIdResult = DBManager.Instance.IdDuplicateCheck(signUpPacket.UserId);
                    bool signUpResult = DBManager.Instance.SignUp(signUpPacket.UserId, signUpPacket.UserPw);

                    if (signUpResult && !signUpIdResult)
                    {
                        resultPacket.packetError = (short)ErrorCode.None;
                        resultPacket.ServerResult = true;
                        ServerNet.networkMessage.Enqueue(clientSocket.RemoteEndPoint + "의 회원가입 성공");
                    }
                    else
                    {
                        resultPacket.packetError = (short)ErrorCode.SignUpFailure;
                        resultPacket.ServerResult = false;
                        ServerNet.networkMessage.Enqueue(clientSocket.RemoteEndPoint + "의 회원가입 실패");
                    }

                    resultPacket.packetId = (short)PacketId.ResSignUp;
                    break;
            }

            PacketSendToClient(clientSocket, resultPacket);
        }
    }
}
