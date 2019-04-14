using Godot;
using System;

public class StartControl : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode("Menu").GetNode("StartButton").Connect("presssed", this, nameof(_OnStartButtonPressed));
    }

    public void _OnStartButtonPressed()
    {
        GD.Print("Boop.");
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
