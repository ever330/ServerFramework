using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketBase
{
    public class PacketDefine
    {
        public static readonly int HEADERSIZE = 4;
    }

    public enum ErrorCode : short
    {
        None                = 0,

        ConnectFailure      = 1000,
        LoginFailure        = 1001,
        IdCheckFailure      = 1002,
        SignUpFailure       = 1003
    }

    public enum PacketId : short
    {
        ClientBegin         = 1000,

        ConnectResult       = 1001,

        ReqIdCheck          = 1100,
        ResIdCheck          = 1101,

        ReqSignUp           = 1200,
        ResSignUp           = 1201,

        ReqLogin            = 1300,
        ResLogin            = 1301,

        ClientEnd           = 1900
    }
}
