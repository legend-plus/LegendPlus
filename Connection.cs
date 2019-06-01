using Godot;
using System;
using Packets;
using System.Collections.Generic;
using System.Reflection;
using Godot.Collections;
using LegendItems;
public class Connection : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    StreamPeerTCP client;
    PacketPeerStream wrapped_client;
    bool connected = false;

    bool disconnected = false;

    //string ip = "192.95.22.236";
    //string ip = "127.0.0.1";
    string ip = "10.1.0.152";
    int port = 21321;

    bool joined = false;

    // Called when the node enters the scene tree for the first time.

    public override void _Ready()
    {
        client = new StreamPeerTCP();
        client.SetBigEndian(true);
    }

    public override void _Process(float delta)
    {
        if (connected && !client.IsConnectedToHost()) {
            connected = false;
            disconnected = true;
        }
        if (connected) {
            getPackets();
        }
        if (disconnected)
        {
            GD.Print(String.Format("Reconnecting to {0}:{1}", ip, port));
            client.ConnectToHost(ip, port);
            if (client.IsConnectedToHost()) {
                client.SetNoDelay(true);
                client.SetBlockSignals(true);
                wrapped_client = new PacketPeerStream();
                wrapped_client.SetStreamPeer(client);
                connected = true;
                disconnected = false;
                GD.Print("Reconnected.");
            } else {
                GD.Print("Failed to connect.");
            }
        }
    }

    public void start()
    {
        GD.Print(String.Format("Connecting to {0}:{1}", ip, port));
        client.ConnectToHost(ip, port);
        if (client.IsConnectedToHost()) {
            client.SetNoDelay(true);
            client.SetBlockSignals(true);
            wrapped_client = new PacketPeerStream();
            wrapped_client.SetStreamPeer(client);
            var testPacket = new Packets.PingPacket("Hello There!");
            //sendPacket(testPacket);
            //GD.Print(client.PutData(data));
            connected = true;
            GD.Print("Connected.");
        } else {
            GD.Print("Failed to connect.");
        }
    }

    public void getPackets()
    {
        //GD.Print("Get Packets");
        //GD.Print(wrapped_client.GetAvailablePacketCount());
        if (connected && client.GetAvailableBytes() > 0)
        {
            //byte[] packet_data = wrapped_client.GetPacket();
            //UInt32 packetLength = BitConverter.ToUInt32(packet_data, 0);
            //short packetId = BitConverter.ToInt16(packet_data, 4);
            UInt32 packetLength = (UInt32) client.GetU32();
            short packetId = (short) client.Get16();
            var packetData = client.GetData((int) packetLength - 2);
            //var data = new List<byte>(packet_data).GetRange(6, packet_data.Length -6).ToArray();
            //GD.Print(BitConverter.ToString( (byte[]) packetData[1]));
            var data = (byte[]) packetData[1];
            var packet = Packets.Packets.decode(packetId, data);

            if (GetParent().GetNodeOrNull("GUI") != null) {
                var gui = (Control) GetParent().GetNodeOrNull("GUI");
                gui.Call("recordPacket", packetLength + 4);
            }

            GD.Print(String.Format("Received packet {0}, ID: {1} Length: {2}", packet.name, packetId, packetLength));
            
            if (packet is ReadyPacket)
            {
                ReadyPacket parsed_packet = (ReadyPacket) packet;
                if (parsed_packet.code == 0) {
                    string token = (string) GetParent().GetNode("Discord Integration").Call("getToken");
                    var loginPacket = new LoginPacket(token);
                    GD.Print("Sending login");
                    sendPacket(loginPacket);
                }
                else if (parsed_packet.code == 1)
                {
                    var requestWorldPacket = new RequestWorldPacket();
                    sendPacket(requestWorldPacket);
                    if (!joined)
                    {
                        var loadingRes = GD.Load<PackedScene>("res://scenes/world.tscn");
                        var node = loadingRes.Instance();
                        node.SetName("WorldScene");
                        var loadingGuiRes = GD.Load<PackedScene>("res://scenes/gui.tscn");
                        var gui = (Control) loadingGuiRes.Instance();
                        gui.SetName("GUI");
                        GetParent().Call("setState", 2);
                        GetParent().AddChild(node);
                        //GetParent().AddChild(gui);
                        GetParent().AddChild(gui);
                        //node.AddChild(gui);
                        GetParent().GetNode("GameLoader").Free();
                        //var playerSpriteScene = (PackedScene) node.Call("getSprite", "rowan");
                        //var playerSprite = (AnimatedSprite) playerSpriteScene.Instance();
                        //playerSprite.SetName("PlayerSprite");
                        //node.GetNode("World/Player").AddChild(playerSprite);
                        //playerSprite.Position = ((KinematicBody2D) node.GetNode("Player")).Position;
                        //playerSprite.Visible = true;
                        //GD.Print(playerSprite);
                        joined = true;
                    }
                }
            }
            else if (packet is PongPacket)
            {
                PongPacket parsed_packet = (PongPacket) packet;
                GD.Print("Got pong of " + parsed_packet.message);
            }
            else if (packet is LoginResultPacket)
            {
                LoginResultPacket parsed_packet = (LoginResultPacket) packet;
                GD.Print("Login Result: " + parsed_packet.responseCode.ToString() + " Name: " + parsed_packet.userId);
                var joinGamePacket = new JoinGamePacket();
                sendPacket(joinGamePacket);
            }
            else if (packet is WorldPacket)
            {
                WorldPacket parsed_packet = (WorldPacket) packet;
                //GD.Print(parsed_packet.debug);
                GetParent().GetNode("WorldScene").Call("loadWorld", new object[] {parsed_packet.worldData, parsed_packet.bumpData, parsed_packet.height, parsed_packet.width});
            }
            else if (packet is PlayerPositionPacket)
            {
                PlayerPositionPacket parsed_packet = (PlayerPositionPacket) packet;
                GetParent().GetNode("WorldScene/World/Player").Call("move", new object[] {parsed_packet.x, parsed_packet.y});
            }
            else if (packet is ChatPacket)
            {
                ChatPacket parsed_packet = (ChatPacket) packet;
                GetNode("../GUI/Chat").Call("AddMessage", parsed_packet.author + ": " + parsed_packet.msg);
            }
            else if (packet is EntityPacket)
            {
                EntityPacket parsed_packet = (EntityPacket) packet;
                GD.Print("Got entity '", parsed_packet.sprite, "' at ", parsed_packet.x, ",", parsed_packet.y, " ID: ", parsed_packet.uuid);
                GetNode("../WorldScene").Call("addEntity", parsed_packet.x, parsed_packet.y, parsed_packet.type, parsed_packet.facing, parsed_packet.interactable, parsed_packet.sprite, parsed_packet.uuid, parsed_packet.type != 2);
            }
            else if (packet is EntityMovePacket)
            {
                EntityMovePacket parsed_packet = (EntityMovePacket) packet;
                GD.Print("Got entity moving to ", parsed_packet.x, ",", parsed_packet.y, " ID: ", parsed_packet.uuid);
                GetNode("../WorldScene").Call("moveEntity", parsed_packet.uuid, parsed_packet.x, parsed_packet.y, parsed_packet.facing);
            }
            else if (packet is InvalidateCachePacket)
            {
                InvalidateCachePacket parsed_packet = (InvalidateCachePacket) packet;
                GD.Print(parsed_packet.uuid, " Invalidated.");
                GetNode("../WorldScene").Call("hideEntity", parsed_packet.uuid);
            }
            else if (packet is DialoguePacket)
            {
                DialoguePacket parsed_packet = (DialoguePacket) packet;
                GD.Print("Got dialogue \"", parsed_packet.text, "\"");
                Window window = (Window) GetNode("../GUI/Window");
                window.OpenDialoguePanel();
                DialoguePanel dialoguePanel = window.OpenDialoguePanel(); //(DialoguePanel) GetNode("../GUI/Window/DialoguePanel");
                dialoguePanel.SetDialogue(parsed_packet.text, parsed_packet.author, parsed_packet.sprite, parsed_packet.substitutions, parsed_packet.optionViews);
            }
            else if (packet is CloseDialoguePacket)
            {
                CloseDialoguePacket parsed_packet = (CloseDialoguePacket) packet;
                DialoguePanel dialoguePanel = (DialoguePanel) GetNode("../GUI/Window/DialoguePanel");
                dialoguePanel.CloseDialogue(parsed_packet.guid);
                //dialoguePanel.SetDialogue(parsed_packet.text, parsed_packet.author, parsed_packet.sprite, parsed_packet.substitutions, parsed_packet.optionViews);
            }
            else if (packet is PlayerDataPacket)
            {
                PlayerDataPacket parsed_packet = (PlayerDataPacket) packet;
                var player = (Player) GetNode("../WorldScene/World/Player");
                player.SetUuid(parsed_packet.guid);
                if (player.GetNodeOrNull("PlayerSprite") != null)
                {
                    player.GetNodeOrNull("PlayerSprite").Free();
                }
                var playerSpriteScene = (PackedScene) GetNode("../WorldScene").Call("getSprite", parsed_packet.sprite);
                var playerSprite = (AnimatedSprite) playerSpriteScene.Instance();
                playerSprite.SetName("PlayerSprite");
                player.AddChild(playerSprite);
            }
            else if (packet is InventoryPacket)
            {
                InventoryPacket parsed_packet = (InventoryPacket) packet;
                var player = (Player) GetNode("../WorldScene/World/Player");
                if (player.guid == parsed_packet.inventory.guid)
                {
                    player.inventory = parsed_packet.inventory;
                    foreach (Item item in player.inventory.items)
                    {
                        GD.Print("Item: ", item.GetName(), " \"", item.GetDescription(), "\"");
                    }
                }
            }
        } else {
            var testPacket = new Packets.PingPacket("Hello There!");
            //sendPacket(testPacket);
        }
    }

    public void sendPacket(Packet packet)
    {
        if (client.IsConnectedToHost())
        {
            var data = Packets.Packets.encode(packet);
            if (GetParent().GetNodeOrNull("GUI") != null) {
                var gui = (Control) GetParent().GetNodeOrNull("GUI");
                gui.Call("recordSendPacket", data.Length);
            }
            client.PutData(data);
            GD.Print("Sent packet ID ", packet.id, " \"", packet.name, "\"");
        }
    }

    public StreamPeerTCP getClient() {
        return client;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
