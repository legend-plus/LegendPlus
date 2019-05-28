using Godot;
using System;
using System.Collections.Generic;
using LegendDialogue;

public class DialoguePanel : PanelContainer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    Dictionary<string, StreamTexture> avatars = new Dictionary<string, StreamTexture>();
    PackedScene optionContainer;

    public bool unlocked = false;
    public override void _Ready()
    {
        avatars["antonio_avatar"] = GD.Load<StreamTexture>("res://characters/antonio/avatar.tres");
        optionContainer = GD.Load<PackedScene>("res://ui/OptionContainer.tscn");
    }

    public void CloseDialogue(Guid uuid)
    {
        if (Visible)
        {
            ((Control) GetParent()).SetVisible(false);
            SetVisible(false);
            Godot.Collections.Array children = GetNode("Dialogue").GetChildren();
            foreach (Node obj in children)
            {
                if (obj.GetFilename() == "res://ui/OptionContainer.tscn")
                {
                    obj.Free();
                }
            }
        }
    }
    public void SetDialogue(string text, string author, string sprite, List<Substitution> substitutions, List<OptionView> options)
    {
        Label name = (Label) GetNode("Dialogue/MainDialogue/MainDialogueBox/Speaker/NameControl/Name");
        name.SetText(author);
        Label labelText = (Label) GetNode("Dialogue/MainDialogue/MainDialogueBox/Text");
        List<object> subs = new List<object>();
        foreach (Substitution sub in substitutions)
        {
            subs.Add(sub.ToString());
        }
        text = String.Format(text, subs.ToArray());
        labelText.SetText(text);
        TextureRect avatar = (TextureRect) GetNode("Dialogue/MainDialogue/MainDialogueBox/Speaker/Avatar");
        if (avatars.ContainsKey(author))
        {
            avatar.SetTexture(avatars[author]);
        }

        Godot.Collections.Array children = GetNode("Dialogue").GetChildren();

        foreach (Node obj in children)
        {
            if (obj.GetFilename() == "res://ui/OptionContainer.tscn")
            {
                obj.Free();
            }
        }

        foreach (OptionView view in options)
        {
            Node instance = optionContainer.Instance();
            Label optionText = (Label) instance.GetNode("Container/OptionBackground/Option");
            OptionBackground bg = (OptionBackground) instance.GetNode("Container/OptionBackground");
            bg.SetUuid(view.uuid);
            optionText.SetText(view.text);
            GetNode("Dialogue").AddChild(instance);
        }

        ((Control) GetParent()).SetVisible(true);
        SetVisible(true);
        unlocked = true;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
