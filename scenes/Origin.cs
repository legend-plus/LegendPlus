using Godot;
using System;

public class Origin : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
    {
        Update();
    }

    public override void _Draw()
    {
        var debug = (Control) GetNode("../../../GUI/Debug");
        if (debug.Visible) {
            var playerPos = (Vector2) GetParent().GetNode("Player").Call("GetPos");
            Vector2 chunkPos = new Vector2((int) playerPos.x >> 3, (int) playerPos.y >> 3);
            var tileMap = (TileMap) GetParent().GetNode("Tiles");
            var topLeft = tileMap.MapToWorld(new Vector2(chunkPos.x * 8, chunkPos.y * 8));
            var topRight = tileMap.MapToWorld(new Vector2((chunkPos.x * 8) + 8, chunkPos.y * 8));
            var bottomLeft = tileMap.MapToWorld(new Vector2(chunkPos.x * 8, (chunkPos.y * 8) + 8));
            var bottomRight = tileMap.MapToWorld(new Vector2((chunkPos.x * 8) + 8, (chunkPos.y * 8) + 8));
            var col = new Color(0, 255, 0);
            DrawLine(topLeft, topRight, col, 2);
            DrawLine(topRight, bottomRight, col, 2);
            DrawLine(bottomRight, bottomLeft, col, 2);
            DrawLine(bottomLeft, topLeft, col, 2);
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
