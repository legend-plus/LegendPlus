using LegendItems;
using System;

namespace Packets
{
    public class InventoryPacket : Packet
    {
        public override short id { get { return 14; } }
        public override string name { get { return "Inventory"; } }

        public static DataType[] schema = {
            new DataFixedString(32),
            new DataInventory()
        };

        public Inventory inventory;

        public InventoryPacket(Inventory inventory)
        {
            this.inventory = inventory;
        }

        public InventoryPacket(byte[] received_data)
        {
            var decoded = Packets.decodeData(schema, received_data);
            var uuid = (string)decoded[0];
            var guid = Guid.Parse(uuid);
            inventory = (Inventory)decoded[1];
            inventory.guid = guid;
        }

        public override byte[] encode()
        {
            var output = Packets.encodeData(schema, new object[] { inventory.guid.ToString("N"), inventory });
            return output;
        }
    }

}