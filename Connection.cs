using Godot;
using System;
using Packets;
using System.Collections.Generic;
using System.Reflection;
using Godot.Collections;
public class Connection : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    StreamPeerTCP client;
    PacketPeerStream wrapped_client;
    bool connected = false;
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
        }
        if (connected) {
            getPackets();
        }
    }

    public void start()
    {
        var ip = "192.95.22.236";
        var port = 21321;
        GD.Print(String.Format("Connecting to {0}:{1}", ip, port));
        client.ConnectToHost(ip, port);
        if (client.IsConnectedToHost()) {
            client.SetNoDelay(true);
            client.SetBlockSignals(true);
            wrapped_client = new PacketPeerStream();
            wrapped_client.SetStreamPeer(client);
            var testPacket = new Packets.PingPacket("Hello There!");
            //sendPacket(testPacket);
            //wrapped_client.PutPacket(data);
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
            GD.Print(String.Format("Received packet, ID: {0} Length: {1}", packetId, packetLength));
            //GD.Print(BitConverter.ToString( (byte[]) packetData[1]));
            var data = (byte[]) packetData[1];
            var packet = Packets.Packets.decode(packetId, data);
            
            if (packet is ReadyPacket)
            {
                ReadyPacket parsed_packet = (ReadyPacket) packet;
                if (parsed_packet.code == 0) {
                    string token = (string) GetParent().GetNode("Discord Integration").Call("getToken");
                    var loginPacket = new LoginPacket(token);
                    sendPacket(loginPacket);
                }
                else if (parsed_packet.code == 1)
                {
                    var requestWorldPacket = new RequestWorldPacket();
                    sendPacket(requestWorldPacket);
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
                    var playerSpriteScene = (PackedScene) node.Call("getSprite", "rowan");
                    var playerSprite = (AnimatedSprite) playerSpriteScene.Instance();
                    playerSprite.SetName("PlayerSprite");
                    //playerSprite.SetAnimation("down");
                    node.GetNode("World/Player").AddChild(playerSprite);
                    //playerSprite.Position = ((KinematicBody2D) node.GetNode("Player")).Position;
                    //playerSprite.Visible = true;
                    GD.Print(playerSprite);
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
                GetNode("../WorldScene").Call("addEntity", parsed_packet.x, parsed_packet.y, parsed_packet.type, parsed_packet.facing, parsed_packet.interactable, parsed_packet.sprite, parsed_packet.uuid);
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
            client.PutData(data);
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
