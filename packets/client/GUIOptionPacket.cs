using System;

namespace Packets
{
    public class GUIOptionPacket : Packet
    {
        public override short id { get { return -9; } }
        public override string name { get { return "GUI Option"; } }

        public string uuid;
        public string data;
        public Guid guid;

        public static DataType[] schema = {
            new DataFixedString(32),
            new DataEndingString()
        };

        public GUIOptionPacket(Guid uuid, string data = "")
        {
            this.guid = uuid;
            this.uuid = uuid.ToString("N");
            this.data = data;
        }
        public GUIOptionPacket(byte[] received_data)
        {
            var decoded = Packets.decodeData(schema, received_data);
            uuid = (string)decoded[0];
            guid = Guid.Parse(uuid);
            data = (string)decoded[1];
        }

        public override byte[] encode()
        {
            var output = Packets.encodeData(schema, new object[] { uuid, data });
            return output;
        }
    }

}