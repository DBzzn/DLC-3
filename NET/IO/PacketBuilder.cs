using System;
using System.IO;
using System.Text;

namespace DLC_3.NET.IO
{
    class PacketBuilder
    {
        MemoryStream _ms;

        public PacketBuilder()
        {
            _ms = new MemoryStream();
        }

        public void WriteOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }

        public void WriteMessage(string msg)
        {
            var msgLength = msg.Length;
            _ms.Write(BitConverter.GetBytes(msgLength), 0, sizeof(int));
            _ms.Write(Encoding.Unicode.GetBytes(msg), 0, msgLength);
        }

        public byte[] GetPacketBytes()
        {
            return _ms.ToArray();
        }
    }
}
