using Godot;
using System;
using System.Collections.Generic;

public class gui : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    float guiTimer = 0;

    const int SAMPLES = 100;

    struct tick {
        public float time;
        public int dataIn;
        public int packetsIn;
        public int dataOut;
        public int packetsOut;
    }

    List<tick> ticks = new List<tick>();
    int dataIn = 0;
    int packetsIn = 0;

    int dataOut = 0;
    int packetsOut = 0;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
    {
        guiTimer += delta;
        ticks.Add(new tick() {
            time = guiTimer,
            dataIn = dataIn,
            packetsIn = packetsIn,
            dataOut = dataOut,
            packetsOut = packetsOut
        });
        dataIn = 0;
        packetsIn = 0;
        dataOut = 0;
        packetsOut = 0;
        if (ticks.Count > SAMPLES)
        {
            ticks.RemoveAt(0);
        }
        float sampleDelta = (ticks[ticks.Count-1].time - ticks[0].time);
        double fps = Math.Round((ticks.Count-1) / sampleDelta);
        int totalDataIn = 0;
        int totalPacketsIn = 0;
        int totalDataOut = 0;
        int totalPacketsOut = 0;
        for (var i = 0; i < ticks.Count; i++)
        {
            totalDataIn += ticks[i].dataIn;
            totalPacketsIn += ticks[i].packetsIn;
            totalDataOut += ticks[i].dataOut;
            totalPacketsOut += ticks[i].packetsOut;
        }
        double pps = Math.Round(totalPacketsIn / sampleDelta);
        double dips = Math.Round(totalDataIn / sampleDelta);
        double pops = Math.Round(totalPacketsOut / sampleDelta);
        double dops = Math.Round(totalDataOut / sampleDelta);
        //int received = (int)  (rollingIn / (receivedPackets[receivedPackets.Count-1].time - receivedPackets[0].time));
        //int numReceived = (int) ( (receivedPackets.Count - 1) / (receivedPackets[receivedPackets.Count-1].time - receivedPackets[0].time));
        var debug = (Control) GetNode("Debug");
        if (debug.Visible)
        {
            var label = (Label) debug.GetNode("FPS");
            label.SetText("FPS=" + fps.ToString());

            label = (Label) debug.GetNode("NetworkIn");
            label.SetText("IN=" + pps.ToString() + "/" + dips.ToString()+ "bps");

            label = (Label) debug.GetNode("NetworkOut");
            label.SetText("OUT=" + pops.ToString() + "/" + dops.ToString()+ "bps");

            var playerPosNode = GetNodeOrNull("../WorldScene/World/Player");
            if (playerPosNode != null)
            {
                var playerPos = (Vector2) playerPosNode.Call("GetPos");
                Vector2 chunkPos = new Vector2((int) playerPos.x >> 3, (int) playerPos.y >> 3);
                label = (Label) debug.GetNode("Chunk");
                label.SetText("CHUNK=" + chunkPos.x.ToString() +  ", " + chunkPos.y.ToString());
                label = (Label) debug.GetNode("Pos");
                label.SetText("POS=" + playerPos.x.ToString() + "," + playerPos.y.ToString());
            }
        }
    }
    public void recordPacket(int size)
    {
        dataIn += size;
        packetsIn += 1;
    }

    public void recordSendPacket(int size)
    {
        dataOut += size;
        packetsOut += 1;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
