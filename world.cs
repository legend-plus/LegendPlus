using Godot;
using System;
using System.Collections.Generic;
using System.Collections;
using Packets;

public class world : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    public int[,] tileWorld;

    public int[,] bumpWorld;

    public Dictionary<String, PackedScene> sprites = new Dictionary<string, PackedScene>();

    public List<Node2D> entityList = new List<Node2D>();

    public Dictionary<String, KinematicBody2D> entities = new Dictionary<string, KinematicBody2D>();

    PackedScene baseEntity;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("World create!");   
        sprites["rowan"] = GD.Load<PackedScene>("res://characters/rowan/rowan.tscn");
        sprites["orange_cat"] = GD.Load<PackedScene>("res://characters/orange_cat/orange_cat.tscn");
        sprites["antonio"] = GD.Load<PackedScene>("res://characters/antonio/antonio.tscn");
        baseEntity = GD.Load<PackedScene>("res://entities/Entity.tscn");
    }

    public override void _Process(float delta)
    {
        
    }

    public PackedScene getSprite(string spriteName) {
        if (sprites.ContainsKey(spriteName))
        {
            return sprites[spriteName];
        }
        else
        {
            return sprites["rowan"];
        }
    }

    public void loadWorld(int[] flatWorld, int[] flatBump, UInt32 height, UInt32 width)
    {
        // Godot seems to flatten my worlds...
        // Guess the earth is flat after all
        tileWorld = new int[height, width];
        bumpWorld = new int[height, width];
        var tileMap = (TileMap) GetNode("World/Tiles");
        var bumpMap = (TileMap) GetNode("World/BumpMap");
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
        var camera = (Camera2D) GetNode("World/Camera2D");
        var corner = tileMap.MapToWorld(new Vector2(height, width));
        camera.LimitRight = (int) corner.x;
        camera.LimitBottom = (int) corner.y;
        GD.Print("World Loaded");
    }

    public void addEntity(int pos_x, int pos_y, int entity_type, int entity_facing, bool entity_interactable, string entity_sprite, string entity_uuid)
    {
        if (sprites.ContainsKey(entity_sprite) && !entities.ContainsKey(entity_uuid))
        {
            var spriteScene = sprites[entity_sprite];
            var sprite = (AnimatedSprite) spriteScene.Instance();
            var entity = (KinematicBody2D) baseEntity.Instance();
            sprite.SetName("EntitySprite");
            entity.AddChild(sprite);
            entity.SetName(entity_uuid);
            var tileMap = (TileMap) GetNode("World/Tiles");
            var pos = tileMap.MapToWorld(new Vector2(pos_x, pos_y));
            entity.SetPosition(pos);
            entity.Call("SetTilePosition", new Vector2(pos_x, pos_y));
            entity.Call("SetFacing", entity_facing);
            entityList.Add(entity);
            entities.Add(entity_uuid, entity);
            GetNode("World").AddChild(entity);
        }
        else if (sprites.ContainsKey(entity_sprite) && entities.ContainsKey(entity_uuid))
        {
            var tileMap = (TileMap) GetNode("World/Tiles");
            entities[entity_uuid].SetVisible(true);
            entities[entity_uuid].Call("SetTilePosition", new Vector2(pos_x, pos_y));
            var pos = tileMap.MapToWorld(new Vector2(pos_x, pos_y));
            entities[entity_uuid].SetPosition(pos);
            entities[entity_uuid].Call("SetFacing", entity_facing);
            ((AnimatedSprite) entities[entity_uuid].GetNode("EntitySprite")).SetVisible(true);
        }
    }

    public void moveEntity(string entity_uuid, int pos_x, int pos_y, int facing)
    {
        entities[entity_uuid].Call("Move", new Vector2(pos_x, pos_y), facing);
    }

    public void hideEntity(string entity_uuid)
    {
        entities[entity_uuid].SetVisible(false);
        ((AnimatedSprite) entities[entity_uuid].GetNode("EntitySprite")).SetVisible(false);
    }

    public bool collidesWithEntity(Vector2 position)
    {
        for (var i = 0; i < entityList.Count; i++)
        {
            var entity = entityList[i];
            if (position.x == entity.Position.x && position.y == entity.Position.y)
            {
                return true;
            }
        }
        return false;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
