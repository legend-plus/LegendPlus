using Godot;
using System;
using Packets;

public class OptionBackground : PanelContainer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    Theme normal;
    Theme hover;

    Theme pressed;

    bool over = false;
    bool isPressed = false;

    Guid uuid;
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
                DialoguePanel panel = (DialoguePanel) GetNode("../../../../");
                if (isPressed && over && panel.unlocked)
                {
                    var conn = (Connection) GetNode("../../../../../../../Connection");
                    var optionPacket = new GUIOptionPacket(uuid);
                    conn.sendPacket(optionPacket);
                    panel.unlocked = false;
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
            SetTheme(hover);
            over = true;
            //this.AddStyleboxOverride("Panel", hover);
        }
        else if (what == (int) NotificationMouseExit)
        {
            SetTheme(normal);
            over = false;
            //this.AddStyleboxOverride("Panel", normal);
        }
    }

    public void SetUuid(Guid uuid)
    {
        this.uuid = uuid;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
