using LegendItems;
using System;

namespace Packets
{
    public class ModifyItemPacket : Packet
    {
        public override short id { get { return 16; } }
        public override string name { get { return "Modify Item"; } }

        public static DataType[] schema = {
            new DataFixedString(32),
            new DataItem(),
            new DataInt()
        };

        public string uuid;

        public Guid guid;

        public Item item;

        public int index;

        public ModifyItemPacket(Guid guid, Item item, int index)
        {
            uuid = guid.ToString("N");
            this.guid = guid;
            this.item = item;
            this.index = index;
        }

        public ModifyItemPacket(byte[] received_data)
        {
            var decoded = Packets.decodeData(schema, received_data);
            uuid = (string)decoded[0];
            guid = Guid.Parse(uuid);
            item = (Item)decoded[1];
            index = (int)decoded[2];
        }

        public override byte[] encode()
        {
            var output = Packets.encodeData(schema, new object[] { uuid, item, index });
            return output;
        }
    }

}