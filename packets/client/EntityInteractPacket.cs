using System;

namespace Packets
{
    public class EntityInteractPacket : Packet
    {
        public override short id { get { return -8; } }
        public override string name { get { return "Entity Interact"; } }

        public short interactType;
        public string uuid;
        public Guid guid;

        public static DataType[] schema = {
            new DataShort(),
            new DataFixedString(32)
        };

        public EntityInteractPacket(short interactType, Guid uuid)
        {
            this.guid = uuid;
            this.uuid = uuid.ToString("N");
            this.interactType = interactType;
        }
        public EntityInteractPacket(byte[] received_data)
        {
            var decoded = Packets.decodeData(schema, received_data);
            interactType = (short)decoded[0];
            uuid = (string)decoded[1];
            guid = Guid.Parse(uuid);
        }

        public override byte[] encode()
        {
            var output = Packets.encodeData(schema, new object[] { interactType, uuid });
            return output;
        }
    }

}