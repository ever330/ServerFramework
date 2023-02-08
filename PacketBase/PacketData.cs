using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace PacketBase
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GIPacket
    {
        [MarshalAs(UnmanagedType.I2)]
        public short packetId;

        [MarshalAs(UnmanagedType.I2)]
        public short packetError;

        [MarshalAs(UnmanagedType.I4)]
        public int packetSize;

        public byte[] Serialize()
        {
            var size = Marshal.SizeOf(typeof(GIPacket));
            var array = new byte[size];
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(this, ptr, true);
            Marshal.Copy(ptr, array, 0, size);
            Marshal.FreeHGlobal(ptr);
            return array;
        }

        public static GIPacket Deserialize(byte[] array)
        {
            var size = Marshal.SizeOf(typeof(GIPacket));
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(array, 0, ptr, size);
            var s = (GIPacket)Marshal.PtrToStructure(ptr, typeof(GIPacket));
            Marshal.FreeHGlobal(ptr);
            return s;
        }
    }

    // Marshal 클래스를 이용하여 전송할 데이터를 직렬화, 역직렬화 하는 부분 

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Marshaling<T> where T : class
    {
        [MarshalAs(UnmanagedType.I2)]
        public short packetId;

        [MarshalAs(UnmanagedType.I4)]
        public short packetSize;

        public byte[] Serialize()
        {
            var size = Marshal.SizeOf(typeof(T));
            var array = new byte[size];
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(this, ptr, true);
            Marshal.Copy(ptr, array, 0, size);
            Marshal.FreeHGlobal(ptr);
            return array;
        }

        public static T Deserialize(byte[] array)
        {
            var size = Marshal.SizeOf(typeof(T));
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(array, 0, ptr, size);
            var s = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);
            return s;
        }
    }
}
