using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PacketBase
{
    /// <summary>
    /// 마샬링을 사용하지 않고 MemoryStream을 사용한 패킷
    /// </summary>
    [Serializable]
    public class GamePacket
    {
        public short packetId;
        public short packetError;

        private const int PacketMaxSize = 1024;

        public GamePacket()
        {
            this.packetId = 0;
            this.packetError = 0;
        }

        public static byte[] Serialize(Object data)
        {
            try
            {
                MemoryStream ms = new MemoryStream(PacketMaxSize);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, data);
                return ms.ToArray();
            }
            catch
            {
                return null;
            }
        }

        public static Object Deserialize(byte[] data)
        {
            try
            {
                MemoryStream ms = new MemoryStream(PacketMaxSize);
                ms.Write(data, 0, data.Length);

                ms.Position = 0;
                BinaryFormatter bf = new BinaryFormatter();
                Object obj = bf.Deserialize(ms);
                ms.Close();

                return obj;
            }
            catch
            {
                return null;
            }
        }
    }

    [Serializable]
    public class LoginPacket : GamePacket
    {
        public string UserId { get; set; }
        public string UserPw { get; set; }
    }

    [Serializable]
    public class IdCheckPacket : GamePacket
    {
        public string UserId { get; set; }
    }

    [Serializable]
    public class SignUpPacket : GamePacket
    {
        public string UserId { get; set; }
        public string UserPw { get; set; }
    }

    // 클라이언트의 요청에 따른 서버의 결과를 담아줄 패킷
    [Serializable]
    public class ServerPacket : GamePacket
    {
        public bool ServerResult { get; set; }
    }
}
