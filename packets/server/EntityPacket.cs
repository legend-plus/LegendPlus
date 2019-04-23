namespace Packets
{
    public class EntityPacket : Packet {
        public override short id { get{ return 8;} }
        public override string name { get{ return "Entity";} }

        public static DataType[] schema = {
            new DataInt(),
            new DataInt(),
            new DataUShort(),
            new DataByte(),
            new DataByte(),
            new DataString(),
            new DataFixedString(32)
        };

        public int x;
        public int y;
        public int facing;
        public int type;
        public bool interactable;
        public string sprite;
        public string uuid;

        public EntityPacket(int pos_x, int pos_y, int entity_type, int entity_facing, bool entity_interactable, string entity_sprite, string entity_uuid )
        {
            x = pos_y;
            y = pos_y;
            facing = entity_facing;
            type = entity_type;
            interactable = entity_interactable;
            sprite = entity_sprite;
            uuid = entity_uuid;
        }
        public EntityPacket(byte[] received_data)
        {
            var decoded = Packets.decodeData(schema, received_data);
            x = (int) decoded[0];
            y = (int) decoded[1];
            type = (ushort) decoded[2];
            facing = (int) decoded[3];
            interactable = ((int) decoded[4]) == 1;
            sprite = (string) decoded[5];
            uuid = (string) decoded[6];
        }

        public override byte[] encode()
        {
            var output = Packets.encodeData(schema, new object[] {x, y, type, interactable ? 1 : 0, sprite, uuid});
            return output;
        }
    }

}