using Godot;
using System;
using Packets;

public class world : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    public int[,] tileWorld;

    public int[,] bumpWorld;

    public bool debugMode = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("World create!");   
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("debug_mode"))
        {
            debugMode = !debugMode;
            var tileMap = (TileMap) GetNode("Tiles");
            var bumpMap = (TileMap) GetNode("BumpMap");
            tileMap.SetVisible(!debugMode);
            bumpMap.SetVisible(debugMode);
        }
    }

    public void loadWorld(int[] flatWorld, int[] flatBump, UInt32 height, UInt32 width)
    {
        // Godot seems to flatten my worlds...
        // Guess the earth is flat after all
        tileWorld = new int[height, width];
        bumpWorld = new int[height, width];
        var tileMap = (TileMap) GetNode("Tiles");
        var bumpMap = (TileMap) GetNode("BumpMap");
        for (var y = 0; y < height; y++) {
            for (var x = 0; x < width; x++) {
                var cell = flatWorld[(y * width) + x];
                tileWorld[y, x] = cell;
                tileMap.SetCell(x, y, cell);
                var bumpCell = flatBump[(y * width) + x];
                bumpWorld[y, x] = bumpCell;
                bumpMap.SetCell(x, y, bumpCell);
            }
        }
        var camera = (Camera2D) GetNode("Camera2D");
        var corner = tileMap.MapToWorld(new Vector2(height, width));
        camera.LimitRight = (int) corner.x;
        camera.LimitBottom = (int) corner.y;
        GD.Print("World Loaded");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
