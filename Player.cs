using Godot;
using System;
using Packets;

public class Player : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    public int facing;

    const float moveTime = 0.2f;
    bool moving = false;

    float timeMoving = 0.0f;

    Vector2 moveDelta = new Vector2(0, 0);

    public Vector2 pos = new Vector2(0, 0);

    Vector2 prevPos = new Vector2(0, 0);
    string prevAnim = "";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
    {
        var camera = (Camera2D) GetParent().GetNode("Camera2D");
        var tileMap = (TileMap) GetParent().GetNode("Tiles");
        var bumpMap = (TileMap) GetParent().GetNode("BumpMap");
        if (pos != prevPos) {
            var newPos = tileMap.MapToWorld(pos);
            SetPosition(newPos);
            camera.SetPosition(newPos);
            moving = false;
        }
        var target = new Vector2(pos);
        if (Input.IsActionPressed("ui_right") && !moving)
        {
            target.x += 1;
            facing = 3;
            moving = true;
        }
        if (Input.IsActionPressed("ui_left") && !moving)
        {
            target.x -= 1;
            facing = 0;
            moving = true;
        }
        if (Input.IsActionPressed("ui_down") && !moving)
        {
            target.y += 1;
            facing = 2;
            moving = true;
        }
        if (Input.IsActionPressed("ui_up") && !moving)
        {
            target.y -= 1;
            facing = 1;
            moving = true;
        }
        //GD.Print(new object[] {pos, " vs.  ", target});
        if (!pos.Equals(target) && moving)
        {
            var prevPos = tileMap.MapToWorld(pos);
            var newPos = tileMap.MapToWorld(target);
            if ( bumpMap.GetCell((int) target.x, (int) target.y) != 0 )
            {
                var deltaPos = newPos - prevPos;
                moveDelta = deltaPos / moveTime;
                timeMoving = 0.0f;
                GD.Print("Delta ", deltaPos);
                pos = target;
                var movePacket = new MoveAndFacePacket((int) pos.x, (int) pos.y, (int) facing);
                var conn = (StreamPeerTCP) GetParent().GetParent().GetNode("Connection").Call("getClient");
                var data = Packets.Packets.encode(movePacket);
                conn.PutData(data);
            } 
            else
            {
                var movePacket = new MoveAndFacePacket((int) pos.x, (int) pos.y, (int) facing);
                var conn = (StreamPeerTCP) GetParent().GetParent().GetNode("Connection").Call("getClient");
                var data = Packets.Packets.encode(movePacket);
                conn.PutData(data);
            }
        }
        var result = tileMap.MapToWorld(pos);
        //camera.SetPosition(result);
        var playerSprite = (AnimatedSprite) GetNode("PlayerSprite");
        string animation = "";
        switch (facing)
        {
            case 0:
                animation = "left";
                break;
            case 1:
                animation = "up";
                break;
            case 2:
                animation = "down";
                break;
            case 3:
                animation = "right";
                break;
        }
        if (moving)
        {
            animation += "_walk";
            playerSprite.Play();
            if ((timeMoving + delta) >= moveTime)
            {
                delta = moveTime - timeMoving;
                moving = false;
            }
            timeMoving += delta;
            MoveAndCollide(moveDelta * delta);
            camera.SetPosition(Position);
        }
        else
        {
            SetPosition(result);
            camera.SetPosition(result);
        }
        if (animation != prevAnim)
        {
            GD.Print("Setting anim to " + animation);
            playerSprite.SetAnimation(animation);
        }
        prevPos = pos;
        prevAnim = animation;
    }

    public void move(int x, int y)
    {
        pos.x = x;
        pos.y = y;
        var tileMap = (TileMap) GetParent().GetNode("Tiles");
        var result = tileMap.MapToWorld(pos);
        SetPosition(result);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
