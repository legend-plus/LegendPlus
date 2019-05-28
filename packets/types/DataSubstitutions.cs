using LegendDialogue;
using MiscUtil.Conversion;
using System.Collections.Generic;
using System;

namespace Packets
{
    class DataSubstitutions : DataVarying
    {
        public override int getLength(byte[] data, MiscUtil.Conversion.BigEndianBitConverter converter, int offset = 0)
        {
            return ((int)converter.ToUInt32(data, offset)) + 4;
        }

        public override byte[] encode(object data, BigEndianBitConverter converter)
        {
            List<Substitution> substitutions = (List<Substitution>)data;
            //Yes... We're doing this differently from DataOptions
            //This code is an absolute horrid mess
            //And I can only hope that not everything I make in the future builds off this
            //Because I really, really, need to come back to this later once I know what I'm doing.
            //Alyssa 2019-05-25
            //P.S. you have to simplify your substitutions first
            List<byte[]> subData = new List<byte[]>();
            int dataLength = 0;
            foreach (Substitution sub in substitutions)
            {
                byte[] encoded = sub.Encode(converter);
                dataLength += encoded.Length + 4;
                subData.Add(converter.GetBytes(encoded.Length));
                subData.Add(encoded);
            }
            byte[] output = new byte[dataLength+4];

            System.Buffer.BlockCopy(converter.GetBytes(dataLength), 0, output, 0, 4);
            System.Buffer.BlockCopy(subData.ToArray(), 0, output, 4, dataLength);

            return output;
        }

        public override object decode(byte[] data, BigEndianBitConverter converter, int offset = 0)
        {
            List<Substitution> substitutions = new List<Substitution>();
            int substitutionsLength = (int)converter.ToUInt32(data, offset);
            //Godot.GD.Print("subs Length: ", substitutionsLength, " data: ", BitConverter.ToString(data), " offset: ", offset);
            int pos = offset+4;

            while (pos < offset + substitutionsLength + 4)
            {
                int subLength = (int) converter.ToUInt32(data, pos);
                pos += 4;
                substitutions.Add(Substitution.DecodeSubstitution(data, converter, pos));
                pos += subLength;
            }
            return substitutions;
        }

    }
}