using Godot;
using System;

public class Entity : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    const float moveTime = 0.2f;
    bool moving = false;
    public int facing;
    float timeMoving = 0.0f;

    Vector2 moveDelta = new Vector2(0, 0);

    public Vector2 pos = new Vector2(-1, -1);
    string prevAnim = "";

    public bool solid = true;

    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
    {
        var tileMap = (TileMap) GetParent().GetNode("Tiles");
        var bumpMap = (TileMap) GetParent().GetNode("BumpMap");
        var sprite = (AnimatedSprite) GetNode("EntitySprite");
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
            sprite.Play();
            if ((timeMoving + delta) >= moveTime)
            {
                delta = moveTime - timeMoving;
                moving = false;
            }
            timeMoving += delta;
            MoveAndCollide(moveDelta * delta);
        }

        if (animation != prevAnim)
        {
            sprite.SetAnimation(animation);
        }
        prevAnim = animation;
    }

    public void Move(Vector2 target, int entity_facing)
    {
        facing = entity_facing;
        if (Math.Abs(target.x - pos.x) <= 1 && Math.Abs(target.y - pos.y) <= 1)
        {
            moving = true;
            var tileMap = (TileMap) GetParent().GetNode("Tiles");
            var bumpMap = (TileMap) GetParent().GetNode("BumpMap");
            var prevPos = tileMap.MapToWorld(pos);
            var newPos = tileMap.MapToWorld(target);
            var deltaPos = newPos - prevPos;
            moveDelta = deltaPos / moveTime;
            timeMoving = 0.0f;
            GD.Print("Delta ", deltaPos);
            pos = target;
        }
        else
        {
            var tileMap = (TileMap) GetParent().GetNode("Tiles");
            var bumpMap = (TileMap) GetParent().GetNode("BumpMap");
            var newPos = tileMap.MapToWorld(target);
            pos = target;
            SetPosition(newPos);
        }
    }

    public void SetTilePosition(Vector2 newPos)
    {
        pos = newPos;
    }

    public void SetFacing(int new_facing)
    {
        facing = new_facing;
    }

    public void SetSolid(bool newSolidity)
    {
        solid = newSolidity;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
