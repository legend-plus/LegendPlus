using Godot;
using System;

public class gameload : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    bool freedMenu = false;
    public override void _Ready()
    {
        //GetParent().GetNode("Menu").Free();
        GetParent().GetNode("Connection").Call("start");
    }

    public override void _Process(float delta)
    {
        if (!freedMenu)
        {
            GetParent().GetNode("Menu").Free();
            freedMenu = true;
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
