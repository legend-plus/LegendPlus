using Godot;
using System;
using Packets;

public class LineEdit : Godot.LineEdit
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    bool doReleaseFocus = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public override void _Notification(int what)
    {
        if (what == NotificationFocusEnter)
        {
            //GetNode("../../WorldScene/World/Player").Call("setFocus", false);
        }
    }

    public void _TextEnterred(string text)
    {
        if (text != "")
        {
            SetText("");
            var chatPacket = new SendMessagePacket(text);
            var conn = (StreamPeerTCP) GetNode("../../Connection").Call("getClient");
            var data = Packets.Packets.encode(chatPacket);
            conn.PutData(data);
        }
        doReleaseFocus = true;
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_chat") && !doReleaseFocus)
        {
            GrabFocus();
        }
        if (doReleaseFocus)
        {
            ReleaseFocus();
            doReleaseFocus = false;
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
