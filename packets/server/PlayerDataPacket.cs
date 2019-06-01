using System;

namespace Packets
{
    public class PlayerDataPacket : Packet
    {
        public override short id { get { return 15; } }
        public override string name { get { return "Player Data"; } }

        public string sprite;
        public Guid guid;
        public string uuid;

        public static DataType[] schema = {
            new DataString(),
            new DataFixedString(32)
        };

        public PlayerDataPacket(string sprite, Guid guid)
        {
            this.sprite = sprite;
            this.guid = guid;
            this.uuid = guid.ToString("N");
        }
        public PlayerDataPacket(byte[] received_data)
        {
            var decoded = Packets.decodeData(schema, received_data);
            sprite = (string)decoded[0];
            uuid = (string)decoded[1];
            guid = Guid.Parse(uuid);
        }

        public override byte[] encode()
        {
            var output = Packets.encodeData(schema, new object[] { sprite, uuid });
            return output;
        }
    }

}