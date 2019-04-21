using Godot;
using System;

public class GameFocus : Container
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _Notification(int what)
    {
        if (what == NotificationFocusEnter)
        {
            //GetNode("../../WorldScene/World/Player").Call("setFocus", true);
        }
        else if (what == NotificationFocusExit)
        {
            //GetNode("../../WorldScene/World/Player").Call("setFocus", false);
        }
    }

    public override void _Process(float delta)
    {
        Control focusOwner = GetFocusOwner();
        if (focusOwner != null) {
            GetNode("../../WorldScene/World/Player").Call("setFocus", focusOwner.GetName());
        } else {
            GetNode("../../WorldScene/World/Player").Call("setFocus", "GameFocus");
        }
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
