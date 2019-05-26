using System;
using System.Collections.Generic;
using System.Text;

namespace LegendItems
{
    public class BaseItem
    {
        public String type;
        public String sprite;
        public String name;
        public String description;
        public String itemId;

        public BaseItem(String sprite, String name, String description, String itemId, String type = "item")
        {
            this.type = type;
            this.sprite = sprite;
            this.name = name;
            this.description = description;
            this.itemId = itemId;
        }
    }
}
