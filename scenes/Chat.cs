using Godot;
using System;

public class Chat : ScrollContainer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public bool sticky = true;

    public bool stickNext = false;
    Control toResize = null;

    PackedScene messageContainer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        messageContainer = GD.Load<PackedScene>("res://scenes/ChatMessage.tscn");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Draw()
    {
        var curScroll = GetVScrollbar().GetValue();
        var maxScroll = GetVScrollbar().GetMax() - GetVScrollbar().GetSize().y;
        if (toResize != null)
        {
            var label = (Label) toResize.GetNode("Message");
            toResize.SetCustomMinimumSize(new Vector2(750, label.GetSize().y));
        }
        if (curScroll >= maxScroll)
        {
            sticky = true;
        }
        else if (stickNext)
        {
            GetVScrollbar().SetValue(GetVScrollbar().GetMax());
            stickNext = false;
        }
        else
        {
            sticky = false;
        }
        
    }

    public void AddMessage(string message)
    {
        var chatMessageNode = (Control) messageContainer.Instance();
        var messageLabel = (Label) chatMessageNode.GetNode("Message");
        messageLabel.SetText(message);
        chatMessageNode.SetCustomMinimumSize(new Vector2(750, messageLabel.GetLineCount() * messageLabel.GetLineHeight()));
        toResize = chatMessageNode;
        GetNode("ChatContainer").AddChild(chatMessageNode);
        Update();
        GD.Print("[CHAT] " + message);
        if (sticky)
        {
            SetVScroll((int) GetVScrollbar().GetMax());
            GetVScrollbar().SetValue(GetVScrollbar().GetMax());
            stickNext = true;
        }
    }
}
