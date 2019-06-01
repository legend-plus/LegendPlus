using Godot;
using System;
using System.Collections.Generic;
using LegendItems;

public class InventoryPanel : PanelContainer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    PackedScene itemScene;

    public Dictionary<string, StreamTexture> sprites = new Dictionary<string, StreamTexture>();
    public override void _Ready()
    {
        itemScene = GD.Load<PackedScene>("res://ui/ItemNode.tscn");
        //TODO: Probably don't load every item texture at startup.
        sprites["unknown"] = GD.Load<StreamTexture>("res://items/sprites/unknown.tres");
        sprites["unknown_sword"] = GD.Load<StreamTexture>("res://items/sprites/unknown_sword.tres");
        sprites["dark_sword"] = GD.Load<StreamTexture>("res://items/sprites/dark_sword.tres");
        sprites["jeweled_sword"] = GD.Load<StreamTexture>("res://items/sprites/jeweled_sword.tres");
        sprites["sword"] = GD.Load<StreamTexture>("res://items/sprites/sword.tres");
        sprites["royal_sword"] = GD.Load<StreamTexture>("res://items/sprites/royal_sword.tres");
        sprites["red_potion"] = GD.Load<StreamTexture>("res://items/sprites/red_potion.tres");
    }

    public void SetInventory(Inventory inventory)
    {
        var itemNodes = GetNode("Side/Items/ItemScroller/ItemPanel/ItemContainer").GetChildren();
        foreach (Node node in itemNodes)
        {
            node.Free();
        }

        foreach (Item item in inventory.items)
        {
            AddItem(item);
        }
    }

    public void AddItem(Item item)
    {
        var itemContainer = GetNode("Side/Items/ItemScroller/ItemPanel/ItemContainer");
        ItemNode itemNode = (ItemNode) itemScene.Instance();
        itemNode.item = item;
        TextureRect itemSprite = (TextureRect) itemNode.GetNode("ItemBox/ItemLabel/Sprite");
        var spriteName = item.GetSprite();
        if (sprites.ContainsKey(spriteName))
        {
            itemSprite.SetTexture(sprites[spriteName]);
        }
        else
        {
            itemSprite.SetTexture(sprites["unknown"]);
        }
        Label itemName = (Label) itemNode.GetNode("ItemBox/ItemLabel/Name");
        itemName.SetText(item.GetName());
        itemContainer.AddChild(itemNode);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
