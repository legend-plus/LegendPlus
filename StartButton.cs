using Godot;
using System;

public class StartButton : Button
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    public override void _Pressed()
    {
        GD.Print("Beep.");
        var loadingRes = GD.Load<PackedScene>("res://gameload.tscn");
        var node = loadingRes.Instance();
        node.SetName("GameLoader");
        GetParent().GetParent().Call("setState", 1);
        GetParent().GetParent().AddChild(node);
        GetParent().GetParent().GetNode("Menu").Free();
    }
}
