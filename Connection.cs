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
            var data = Packets.Packets.encode(testPacket);
            GD.Print(BitConverter.ToString(data));
            //wrapped_client.PutPacket(data);
            //GD.Print(client.PutData(data));
            connected = true;
            GD.Print("Connected.");
        } else {
            GD.Print("Failed to connect.");
        }
    }

    public void getPackets() {
        //GD.Print("Get Packets");
        //GD.Print(wrapped_client.GetAvailablePacketCount());
        if (connected && client.GetAvailableBytes() > 0) {
            //byte[] packet_data = wrapped_client.GetPacket();
            //UInt32 packetLength = BitConverter.ToUInt32(packet_data, 0);
            //short packetId = BitConverter.ToInt16(packet_data, 4);
            UInt32 packetLength = (UInt32) client.GetU32();
            short packetId = (short) client.Get16();
            var packetData = client.GetData((int) packetLength - 2);
            //var data = new List<byte>(packet_data).GetRange(6, packet_data.Length -6).ToArray();
            GD.Print(String.Format("Received packet, ID: {0} Length: {1}", packetId, packetLength));
            //GD.Print(BitConverter.ToString(data));
            var data = (byte[]) packetData[1];
            var packet = Packets.Packets.decode(packetId, data);
            
            if (packet is PongPacket) {
                PongPacket parsed_packet = (PongPacket) packet;
                GD.Print("Got pong of " + parsed_packet.message);
            }

            var testPacket = new Packets.PingPacket("Hello There!");
            var datas = Packets.Packets.encode(testPacket);
            client.PutData(datas);
        } else {
            var testPacket = new Packets.PingPacket("Hello There!");
            var data = Packets.Packets.encode(testPacket);
            //client.PutData(data);
            //GD.Print(BitConverter.ToString(data));
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}