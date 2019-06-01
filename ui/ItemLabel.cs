using Godot;
using System;

public class ItemLabel : HSplitContainer
{
    Theme normal;
    Theme hover;

    Theme pressed;

    bool over = false;
    bool isPressed = false;

    ItemNode itemNode;
    public override void _Ready()
    {
        itemNode = (ItemNode) GetNode("../../");
        normal = GD.Load<Theme>("res://ui/optiontheme.tres");
        hover = GD.Load<Theme>("res://ui/optionthemehover.tres");
        pressed = GD.Load<Theme>("res://ui/optionthemepressed.tres");
    }

    public override void _GuiInput(InputEvent @event)
    {
        var smh = @event;
        if (smh is InputEventMouseButton)
        {
            InputEventMouseButton iemb = (InputEventMouseButton) smh;
            if (iemb.Pressed)
            {
                itemNode.SetTheme(pressed);
                itemNode.isPressed = true;
            }
            else
            {
                if (itemNode.isPressed && itemNode.over)
                {
                    // Get Item Details 'n Stuff
                }
                itemNode.SetTheme(itemNode.over ? itemNode.hover : itemNode.normal);
                itemNode.isPressed = false;
            }
        }
    }

    public override void _Notification(int what)
    {
        if (what == (int) NotificationMouseEnter)
        {
            itemNode.SetTheme(hover);
            itemNode.over = true;
            //this.AddStyleboxOverride("Panel", hover);
        }
        else if (what == (int) NotificationMouseExit)
        {
            itemNode.SetTheme(normal);
            itemNode.over = false;
            //this.AddStyleboxOverride("Panel", normal);
        }
    }
}
