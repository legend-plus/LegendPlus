using System.Collections.Generic;
using System;

namespace Packets
{
    public class Packets {
        static Dictionary<short, string> packet_names = new Dictionary<short, string>
        {
            [-7] = "send_message_packet",
            [-6] = "move_and_face_packet",
            [-5] = "move_packet",
            [-4] = "request_world_packet",
            [-3] = "join_game_packet",
            [-2] = "login_packet",
            [-1] = "ping_packet",
            [0] = "null_packet",
            [1] = "pong_packet",
            [2] = "login_result_packet",
            [3] = "world_packet",
            [4] = "ready_packet",
            [5] = "player_position_packet",
            [6] = "disconnect_packet",
            [7] = "chat_packet"
        };

        public static object[] decodeData(DataType[] schema, byte[] data)
        {
            var converter = new MiscUtil.Conversion.BigEndianBitConverter();
            var output = new object[schema.Length];
            var offset = 0;
            for (var i = 0; i < schema.Length; i++)
            {
                if (schema[i] is DataVarying)
                {
                    var scheme = (DataVarying) schema[i];
                    output[i] = scheme.decode(data, converter, offset);
                    offset += scheme.getLength(data, converter, offset);
                }
                else
                {
                    output[i] = schema[i].decode(data, converter, offset);
                    offset += schema[i].length;
                }
            }
            return output;
        }

        public static byte[] encodeData(DataType[] schema, object[] data) {
            var converter = new MiscUtil.Conversion.BigEndianBitConverter();
            int dataLength = 0;
            byte[][] dataParts = new byte[schema.Length][];
            for (var i = 0; i < schema.Length; i++)
            {
                if (schema[i] is DataVarying)
                {
                    var scheme = (DataVarying) schema[i];
                    var schemeData = scheme.encode(data[i], converter);
                    dataLength += schemeData.Length;
                    dataParts[i] = schemeData;
                }
                else
                {
                    dataLength += schema[i].length;
                    dataParts[i] = schema[i].encode(data[i], converter);
                }
            }
            var output = new byte[dataLength];
            var offset = 0;
            for (var i = 0; i < schema.Length; i++) {
                System.Buffer.BlockCopy(dataParts[i], 0, output, offset, dataParts[i].Length);
                offset += dataParts[i].Length;
            }
            return output;
        }

        public static Packet decode(short id, byte[] data)
        {
            string packet_name = packet_names[id];
            switch (packet_name)
            {
                case "send_message_packet":
                    return new SendMessagePacket(data);
                case "move_and_face_packet":
                    return new MoveAndFacePacket(data);
                case "move_packet":
                    return new MovePacket(data);
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
                case "player_position_packet":
                    return new PlayerPositionPacket(data);
                case "disconnect_packet":
                    return new DisconnectPacket(data);
                case "chat_packet":
                    return new ChatPacket(data);
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