using Godot;
using System;

public class world : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    public int[,] tileWorld;

    public int[,] bumpWorld;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("World create!");   
    }

    public override void _Process(float delta)
    {
        var camera = (Camera2D) GetNode("Camera2D");
        var pos = camera.GetPosition();
        GD.Print("At " + pos.x + ", " + pos.y);
        if (Input.IsActionJustPressed("ui_right"))
        {
            pos.x += 32;
            pos.x = (float) Math.Round((double) pos.x / 32) * 32;
            camera.SetPosition(pos);
        }
        if (Input.IsActionJustPressed("ui_left"))
        {
            pos.x -= 32;
            pos.x = (float) Math.Round((double) pos.x / 32) * 32;
            camera.SetPosition(pos);
        }
        if (Input.IsActionJustPressed("ui_down"))
        {
            pos.y += 32;
            pos.y = (float) Math.Round((double) pos.y / 32) * 32;
            camera.SetPosition(pos);
        }
        GD.Print("At " + pos.x + ", " + pos.y);
        if (Input.IsActionJustPressed("ui_up"))
        {
            pos.y -= 32;
            pos.y = (float) Math.Round((double) pos.y / 32) * 32;
        
        }
        GD.Print("At " + pos.x + ", " + pos.y);
        if (pos.y < 182) {
            pos.y = 182;
        }
        if (pos.x < 288) {
            pos.x = 288;
        }
        GD.Print("At4 " + pos.x + ", " + pos.y);
        camera.SetPosition(pos);
    }

    public void loadWorld(int[] flatWorld, int[] flatBump, UInt32 height, UInt32 width)
    {
        // Godot seems to flatten my worlds...
        // Guess the earth is flat after all
        tileWorld = new int[height, width];
        bumpWorld = new int[height, width];
        var tileMap = (TileMap) GetNode("Tiles");
        for (var y = 0; y < height; y++) {
            for (var x = 0; x < width; x++) {
                var cell = flatWorld[(y * width) + x];
                tileWorld[y, x] = cell;
                tileMap.SetCell(x, y, cell);
                var bumpCell = flatBump[(y * width) + x];
                bumpWorld[y, x] = bumpCell;
            }
        }
        GD.Print("World Loaded");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
