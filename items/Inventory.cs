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

        public Inventory(Guid guid)
        {
            this.guid = guid;
        }

        public Inventory()
        {
            guid = Guid.NewGuid();
        }
        
        public void AddItem(Item item, bool update = true)
        {
            if (item.GetMaxStack() > 1)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].Stackable(item) && items[i].GetQuantity() < items[i].GetMaxStack())
                    {
                        int addAmount = Math.Min(items[i].GetMaxStack() - items[i].GetQuantity(), item.GetQuantity());
                        items[i].AddQuantity(addAmount);
                        item.AddQuantity(-addAmount);
                        if (update)
                        {
                            ItemModified?.Invoke(this, items[i], i);
                        }
                        if (item.GetQuantity() <= 0)
                        {
                            break;
                        }
                    }
                }
            }
            if (item.GetQuantity() >= 1)
            {
                items.Add(item);
                if (update)
                {
                    ItemAddedToInventory?.Invoke(this, item, items.Count - 1);
                }
            }
        }

        public void UpdateItem(Item item, int index, bool update = false)
        {
            items[index] = item;
            if (update)
            {
                ItemModified?.Invoke(this, items[index], index);
            }
        }

        public delegate void ItemAddedToInventoryHandler(Inventory inventory, Item item, int index);

        public event ItemAddedToInventoryHandler ItemAddedToInventory;

        public delegate void ItemModifiedHandler(Inventory inventory, Item item, int index);

        public event ItemModifiedHandler ItemModified;
    }
}
