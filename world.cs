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

    public int facing;

    public Vector2 pos = new Vector2(0, 0);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("World create!");   
    }

    public override void _Process(float delta)
    {
        var camera = (Camera2D) GetNode("Camera2D");
        bool posChange = false;
        if (Input.IsActionJustPressed("ui_right"))
        {
            pos.x += 1;
            posChange = true;
            facing = 3;
        }
        if (Input.IsActionJustPressed("ui_left"))
        {
            pos.x -= 1;
            posChange = true;
            facing = 0;
        }
        if (Input.IsActionJustPressed("ui_down"))
        {
            pos.y += 1;
            posChange = true;
            facing = 2;
        }
        if (Input.IsActionJustPressed("ui_up"))
        {
            pos.y -= 1;
            posChange = true;
            facing = 1;
        }
        if (posChange)
        {
            var movePacket = new MoveAndFacePacket((int) pos.x, (int) pos.y, (int) facing);
            var conn = (StreamPeerTCP) GetParent().GetNode("Connection").Call("getClient");
            var data = Packets.Packets.encode(movePacket);
            conn.PutData(data);
        }
        var tileMap = (TileMap) GetNode("Tiles");
        var result = tileMap.MapToWorld(pos);
        camera.SetPosition(result);
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
        var camera = (Camera2D) GetNode("Camera2D");
        var corner = tileMap.MapToWorld(new Vector2(height, width));
        camera.LimitRight = (int) corner.x;
        camera.LimitBottom = (int) corner.y;
        GD.Print("World Loaded");
    }

    public void move(int x, int y)
    {
        pos.x = x;
        pos.y = y;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
