using LegendDialogue;
using System;
using System.Collections.Generic;

namespace Packets
{
    public class DialoguePacket : Packet
    {
        public override short id { get { return 11; } }
        public override string name { get { return "Dialogue"; } }

        public static DataType[] schema = {
            new DataString(),
            new DataString(),
            new DataString(),
            new DataOptions(),
            new DataSubstitutions()
        };

        public string text;
        public string author;
        public string sprite;
        public List<OptionView> optionViews;
        public List<Substitution> substitutions;

        public DialoguePacket()
        {
            //Server only WHOOOO KNOOOOWS howw it's made
            this.text = "I";
            this.author = "SURE";
            this.sprite = "DON'T";
            this.optionViews = new List<OptionView>();
            this.substitutions = new List<Substitution>();
        }

        public DialoguePacket(byte[] received_data)
        {
            var decoded = Packets.decodeData(schema, received_data);
            text = (string)decoded[0];
            author = (string)decoded[1];
            sprite = (string)decoded[2];
            optionViews = (List<OptionView>)decoded[3];
            substitutions = (List<Substitution>)decoded[4];
            
            Godot.GD.Print("TXT '", text, "' -", author, " _", sprite, "_");
            foreach (OptionView view in optionViews)
            {
                Godot.GD.Print(view.uuid, " - ", view.text);
            }
            foreach (Substitution sub in substitutions)
            {
                Godot.GD.Print(sub.ToString());
            }
        }

        public override byte[] encode()
        {
            var output = Packets.encodeData(schema, new object[] { text, author, sprite, optionViews, substitutions });
            return output;
        }
    }

}