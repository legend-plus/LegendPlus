using System;
using System.Collections.Generic;
using System.Text;

namespace LegendItems
{
    public class Inventory
    {
        //Pretty Simple, For Now.
        public List<Item> items = new List<Item>();
        
        public Guid guid;
        
        public InventoryPanel panel = null;

        public Inventory(Guid guid)
        {
            this.guid = guid;
        }

        public void SetPanel(InventoryPanel panel)
        {
            this.panel = panel;
        }

        public Inventory()
        {
            guid = Guid.NewGuid();
        }
        public void AddItem(Item item)
        {
            items.Add(item);
            if (panel != null)
            {
                panel.AddItem(item);
            }
        }
    }
}
