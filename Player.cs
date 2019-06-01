using Godot;
using System;
using Packets;
using LegendItems;

public class Player : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";


    public bool debugMode = false;
    public int facing;
    public int prevFacing;

    const float moveTime = 0.2f;
    bool moving = false;

    bool focused = true;

    string focus = "GameFocus";

    float timeMoving = 0.0f;

    Vector2 moveDelta = new Vector2(0, 0);

    public Vector2 pos = new Vector2(-1, -1);

    Vector2 prevPos = new Vector2(0, 0);
    string prevAnim = "";

    string uuid;
    public Guid guid;

    public Inventory inventory;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
    {
        var camera = (Camera2D) GetParent().GetNode("Camera2D");
        var tileMap = (TileMap) GetParent().GetNode("Tiles");
        var bumpMap = (TileMap) GetParent().GetNode("BumpMap");
        //bool doMove = focused && !moving;
        bool doMove = (focus == "GameFocus" && !moving);
        if (pos != prevPos) {
            var newPos = tileMap.MapToWorld(pos);
            SetPosition(newPos);
            camera.SetPosition(newPos);
            moving = false;
        }
        var target = new Vector2(pos);
        if (Input.IsActionPressed("ui_right") && doMove)
        {
            target.x += 1;
            facing = 3;
            moving = true;
        }
        if (Input.IsActionPressed("ui_left") && doMove)
        {
            target.x -= 1;
            facing = 0;
            moving = true;
        }
        if (Input.IsActionPressed("ui_down") && doMove)
        {
            target.y += 1;
            facing = 2;
            moving = true;
        }
        if (Input.IsActionPressed("ui_up") && doMove)
        {
            target.y -= 1;
            facing = 1;
            moving = true;
        }
        if (Input.IsActionJustPressed("debug_mode"))
        {
            debugMode = !debugMode;
            tileMap.SetVisible(!debugMode);
            bumpMap.SetVisible(debugMode);
            var debugNode = (Control) GetParent().GetParent().GetParent().GetNode("GUI").GetNode("Debug");
            debugNode.SetVisible(debugMode);

        }
        if (Input.IsActionJustPressed("ui_interact"))
        {
            var interactTarget = new Vector2(pos);
            switch (facing)
            {
                case 0:
                    interactTarget.x -= 1;
                    break;
                case 1:
                    interactTarget.y -= 1;
                    break;
                case 2:
                    interactTarget.y += 1;
                    break;
                case 3:
                    interactTarget.x += 1;
                    break;
            }
            Entity interactEntity = (Entity) GetParent().GetParent().Call("getCollidedEntity", tileMap.MapToWorld(interactTarget));
            if (interactEntity != null)
            {
                var interactEntityPacket = new EntityInteractPacket(0, Guid.Parse(interactEntity.uuid));
                sendPacket(interactEntityPacket);
            }
            else
            {
                GD.Print("NO ENTITY AT ", interactTarget);
            }
        }
        if (Input.IsActionJustPressed("ui_open_inventory"))
        {
            Window window = (Window) GetNode("../../../GUI/Window");
            InventoryPanel inventoryPanel = window.OpenInventoryPanel();
            inventoryPanel.SetInventory(inventory);
        }
        if (Input.IsActionJustPressed("ui_close"))
        {
            Window window = (Window) GetNode("../../../GUI/Window");
            window.ClosePanels();
        }
        //GD.Print(new object[] {pos, " vs.  ", target});
        if (!pos.Equals(target) && moving)
        {
            var prevPos = tileMap.MapToWorld(pos);
            var newPos = tileMap.MapToWorld(target);
            if ( bumpMap.GetCell((int) target.x, (int) target.y) != 0 && !(bool) GetParent().GetParent().Call("collidesWithEntity", newPos))
            {
                var deltaPos = newPos - prevPos;
                moveDelta = deltaPos / moveTime;
                timeMoving = 0.0f;
                //GD.Print("Delta ", deltaPos);
                pos = target;
                var movePacket = new MoveAndFacePacket((int) pos.x, (int) pos.y, (int) facing);
                sendPacket(movePacket);
            } 
            else
            {
                if (facing != prevFacing)
                {
                    var movePacket = new MoveAndFacePacket((int) pos.x, (int) pos.y, (int) facing);
                    sendPacket(movePacket);
                }
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
            //GD.Print("Setting anim to " + animation);
            playerSprite.SetAnimation(animation);
        }
        prevPos = pos;
        prevAnim = animation;
        camera.SetPosition(Position);
        prevFacing = facing;
    }

    public void move(int x, int y)
    {
        pos.x = x;
        pos.y = y;
        var tileMap = (TileMap) GetParent().GetNode("Tiles");
        var result = tileMap.MapToWorld(pos);
        SetPosition(result);
    }

    public void setFocus(string newFocus)
    {
        //focused = newFocus;
        focus = newFocus;
    }

    public void sendPacket(Packet packet)
    {
        var client = (Connection) GetParent().GetNode("../../Connection");//.Call("getClient");
        client.sendPacket(packet);
        /*
        if (client.IsConnectedToHost())
        {
            var data = Packets.Packets.encode(packet);
            if (GetNodeOrNull("../../../GUI") != null) {
                var gui = (Control) GetNodeOrNull("../../../GUI");
                gui.Call("recordSendPacket", data.Length);
            }
            client.PutData(data);
        }*/
    }

    public Vector2 GetPos()
    {
        return pos;
    }

    public void SetUuid(Guid guid)
    {
        uuid = guid.ToString("N");
        this.guid = guid;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
