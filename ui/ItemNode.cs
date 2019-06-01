using Godot;
using System;
using LegendItems;

public class ItemNode : PanelContainer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    public Theme normal;
    public Theme hover;

    public Theme pressed;

    public bool over = false;
    public bool isPressed = false;
    public Item item;
    public override void _Ready()
    {
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
                SetTheme(pressed);
                isPressed = true;
            }
            else
            {
                if (isPressed && over)
                {
                    // Get Item Details 'n Stuff
                }
                SetTheme(over ? hover : normal);
                isPressed = false;
            }
        }
    }

    public override void _Notification(int what)
    {
        if (what == (int) NotificationMouseEnter)
        {
            GD.Print("ENTER");
            SetTheme(hover);
            over = true;
            //this.AddStyleboxOverride("Panel", hover);
        }
        else if (what == (int) NotificationMouseExit)
        {
            GD.Print("EXIT");
            SetTheme(normal);
            over = false;
            //this.AddStyleboxOverride("Panel", normal);
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
