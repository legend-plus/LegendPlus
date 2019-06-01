using Godot;
using System;

public class Window : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    PackedScene dialoguePanel;
    PackedScene inventoryPanel;
    public override void _Ready()
    {
        dialoguePanel = GD.Load<PackedScene>("res://ui/DialoguePanel.tscn");
        inventoryPanel = GD.Load<PackedScene>("res://ui/InventoryPanel.tscn");
    }

    public DialoguePanel OpenDialoguePanel()
    {
        var children = GetChildren();
        foreach (Node obj in children)
        {
            if (obj.GetFilename() == "res://ui/DialoguePanel.tscn")
            {
                obj.Free();
            }
        }
        var panel = (DialoguePanel) dialoguePanel.Instance();
        AddChild(panel);
        panel.SetName("DialoguePanel");
        panel.SetVisible(true);
        SetVisible(true);
        return panel;
    }

    public InventoryPanel OpenInventoryPanel()
    {
        var children = GetChildren();
        foreach (Node obj in children)
        {
            if (obj.GetFilename() == "res://ui/InventoryPanel.tscn")
            {
                obj.Free();
            }
        }
        var panel = (InventoryPanel) inventoryPanel.Instance();
        AddChild(panel);
        panel.SetName("InventoryPanel");
        panel.SetVisible(true);
        SetVisible(true);
        return panel;
    }

    public void ClosePanels()
    {
        var children = GetChildren();
        foreach (Node obj in children)
        {
            if (
                obj.GetFilename() == "res://ui/InventoryPanel.tscn" ||
                obj.GetFilename() == "res://ui/DialoguePanel.tscn"
                )
            {
                obj.Free();
            }
        }
        SetVisible(false);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
