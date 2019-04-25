namespace Packets
{
    public class EntityMovePacket : Packet {
        public override short id { get{ return 9;} }
        public override string name { get{ return "Entity Move";} }

        public static DataType[] schema = {
            new DataInt(),
            new DataInt(),
            new DataByte(),
            new DataFixedString(32)
        };

        public int x;
        public int y;

        public int facing;
        public string uuid;

        public EntityMovePacket(int pos_x, int pos_y, int entity_facing, string entity_uuid )
        {
            x = pos_y;
            y = pos_y;
            uuid = entity_uuid;
            facing = entity_facing;
        }
        public EntityMovePacket(byte[] received_data)
        {
            var decoded = Packets.decodeData(schema, received_data);
            x = (int) decoded[0];
            y = (int) decoded[1];
            facing = (int) decoded[2];
            uuid = (string) decoded[3];
        }

        public override byte[] encode()
        {
            var output = Packets.encodeData(schema, new object[] {x, y, facing, uuid});
            return output;
        }
    }

}