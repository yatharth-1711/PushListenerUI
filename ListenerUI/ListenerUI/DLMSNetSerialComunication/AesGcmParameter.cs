

using Gurux.DLMS.Enums;
using System.Text;
using Gurux.DLMS;
using Indali.Security.Enum;
using Indali.Common;

namespace meterReader.AesGcmParameter
{
    public class AesGcmParameter
    {
        public byte Tag { get; set; }

        public Security Security { get; set; }

        public ulong InvocationCounter { get; set; }

        public byte[] SystemTitle { get; set; }

        public byte[] BlockCipherKey { get; set; }

        public byte[] AuthenticationKey { get; set; }

        public CountType Type { get; set; }

        public byte[] CountTag { get; set; }

        public byte[] RecipientSystemTitle { get; set; }

        public byte[] DateTime { get; set; }

        public byte[] OtherInformation { get; set; }

        public int KeyParameters { get; set; }

        public byte[] KeyCipheredData { get; set; }

        public byte[] CipheredContent { get; set; }

        public byte[] SharedSecret { get; set; }

        public SecuritySuite SecuritySuite { get; set; }

        internal GXDLMSTranslatorStructure Xml { get; set; }

        public bool IgnoreSystemTitle { get; set; }

        public AesGcmParameter(
          byte tag,
          Security security,
          uint invocationCounter,
          byte[] systemTitle,
          byte[] blockCipherKey,
          byte[] authenticationKey)
        {
            Tag = tag;
            Security = security;
            InvocationCounter = (ulong)invocationCounter;
            SystemTitle = systemTitle;
            BlockCipherKey = blockCipherKey;
            AuthenticationKey = authenticationKey;
            Type = CountType.Packet;
            SecuritySuite = SecuritySuite.AesGcm128;
        }

        public AesGcmParameter(byte[] systemTitle, byte[] blockCipherKey, byte[] authenticationKey)
        {
            SystemTitle = systemTitle;
            BlockCipherKey = blockCipherKey;
            AuthenticationKey = authenticationKey;
            Type = CountType.Packet;
            SecuritySuite = SecuritySuite.AesGcm128;

        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Security: ");
            stringBuilder.Append((object)Security);
            stringBuilder.Append(" Invocation Counter: ");
            stringBuilder.Append(InvocationCounter);
            stringBuilder.Append(" SystemTitle: ");
            stringBuilder.Append(TSTCommon.ToHex(SystemTitle, true));
            stringBuilder.Append(" AuthenticationKey: ");
            stringBuilder.Append(TSTCommon.ToHex(AuthenticationKey, true));
            stringBuilder.Append(" BlockCipherKey: ");
            stringBuilder.Append(TSTCommon.ToHex(BlockCipherKey, true));
            return stringBuilder.ToString();
        }

    }
}
