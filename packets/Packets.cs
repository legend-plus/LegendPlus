using System.Collections.Generic;
using System;

namespace Packets
{
    public class Packets {
        static Dictionary<short, string> packet_names = new Dictionary<short, string>
        {
            [-4] = "request_world_packet",
            [-3] = "join_game_packet",
            [-2] = "login_packet",
            [-1] = "ping_packet",
            [0] = "null_packet",
            [1] = "pong_packet",
            [2] = "login_result_packet",
            [3] = "world_packet",
            [4] = "ready_packet"
        };

        public static Packet decode(short id, byte[] data) {
            string packet_name = packet_names[id];
            switch (packet_name)
            {
                case "request_world_packet":
                    return new RequestWorldPacket(data);
                case "join_game_packet":
                    return new JoinGamePacket(data);
                case "login_packet":
                    return new LoginPacket(data);
                case "ping_packet":
                    return new PingPacket(data);
                case "null_packet":
                    return new NullPacket(data);
                case "pong_packet":
                    return new PongPacket(data);
                case "login_result_packet":
                    return new LoginResultPacket(data);
                case "world_packet":
                    return new WorldPacket(data);
                case "ready_packet":
                    return new ReadyPacket(data);
                default:
                    return new NullPacket(data);
            }
        }

        public static byte[] encode(Packet packet) {
            byte[] packetId = BitConverter.GetBytes(packet.id);
            byte[] encodedPacket = packet.encode();
            UInt32 packetLength = Convert.ToUInt32(packetId.Length + encodedPacket.Length);
            byte[] packetLengthData = BitConverter.GetBytes(packetLength);

            byte[] output = new byte[packetLengthData.Length + packetId.Length + encodedPacket.Length];
            //Endianess flipping
            Array.Reverse(packetId);
            Array.Reverse(packetLengthData);

            System.Buffer.BlockCopy(packetLengthData, 0, output, 0, packetLengthData.Length);
            System.Buffer.BlockCopy(packetId, 0, output, packetLengthData.Length, packetId.Length);
            System.Buffer.BlockCopy(encodedPacket, 0, output, packetLengthData.Length + packetId.Length, encodedPacket.Length);

            return output;
        }
    }

}