using System;

namespace Packets
{
    public class CloseDialoguePacket : Packet
    {
        public override short id { get { return 12; } }
        public override string name { get { return "Close Dialogue"; } }

        public string uuid;
        public Guid guid;

        public static DataType[] schema = {
            new DataFixedString(32)
        };

        public CloseDialoguePacket(Guid guid)
        {
            this.guid = guid;
            uuid = guid.ToString("N");
        }
        public CloseDialoguePacket(byte[] received_data)
        {
            var decoded = Packets.decodeData(schema, received_data);
            uuid = (string)decoded[0];
            this.guid = Guid.Parse(uuid);
        }

        public override byte[] encode()
        {
            var output = Packets.encodeData(schema, new object[] { uuid });
            return output;
        }
    }

}