using Godot;
using System;

public class legend : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public int state = 0;
    public long stateTime = 0;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      /*var children = GetChildren();
      GD.Print("My children");
      for (var i = 0; i < children.Count; i++) {
          var child = (Node) children[i];
          GD.Print(child.GetName());
      }*/
  }

    public long UnixTimeNow()
    {
        var timeSpan = (System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0));
        return (long)timeSpan.TotalSeconds;
    }
  public void setState(int newState)
  {
      state = newState;
      stateTime = UnixTimeNow();
  }

  public int getState()
  {
      return state;
  }

  public long getStateTime()
  {
      return stateTime;
  }
}
