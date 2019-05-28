using MiscUtil.Conversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegendDialogue
{
    public class FlagSubstitution : Substitution
    {
        const short SubId = 1;
        string flagKey;
        public FlagSubstitution(string flagKey)
        {
            this.flagKey = flagKey;
        }

        public override byte[] Encode(BigEndianBitConverter converter)
        {
            // This is why you should always simplify first
            byte[] idData = converter.GetBytes(SubId);
            byte[] textData = System.Text.Encoding.UTF8.GetBytes("{" + flagKey + "}");

            byte[] output = new byte[textData.Length + 2];

            System.Buffer.BlockCopy(idData, 0, output, 0, 2);
            System.Buffer.BlockCopy(textData, 0, output, 0, textData.Length);

            return output;
        }

        public override string ToString()
        {
            return this.flagKey;
        }
    }
}
