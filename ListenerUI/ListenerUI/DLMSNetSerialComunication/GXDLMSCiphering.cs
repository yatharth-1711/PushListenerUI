
using Gurux.DLMS;
using Gurux.DLMS.Enums;
using Indali.Common;
using Indali.Security.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace meterReader.AesGcmParameter
{
    public class GXDLMSCiphering
    {
        private static byte[] GetNonse(uint invocationCounter, byte[] systemTitle)
        {
            byte[] nonse = new byte[12];
            systemTitle.CopyTo((Array)nonse, 0);
            ((IEnumerable<byte>)BitConverter.GetBytes(invocationCounter)).Reverse<byte>().ToArray<byte>().CopyTo((Array)nonse, 8);
            return nonse;
        }

        public static byte[] DecryptAesGcm(AesGcmParameter p, GXByteBuffer data)
        {
            Command command = data != null && data.Size >= 2 ? (Command)data.GetUInt8() : throw new ArgumentOutOfRangeException("cryptedData");
            switch (command)
            {
                case Command.GloInitiateRequest:
                case Command.GloReadRequest:
                case Command.GloWriteRequest:
                case Command.GloInitiateResponse:
                case Command.GloReadResponse:
                case Command.GloWriteResponse:
                case Command.DedInitiateRequest:
                case Command.DedReadRequest:
                case Command.DedWriteRequest:
                case Command.DedInitiateResponse:
                case Command.DedReadResponse:
                case Command.DedWriteResponse:
                case Command.GloGetRequest:
                case Command.GloSetRequest:
                //case Command.GloEventNotificationRequest:
                case Command.GloMethodRequest:
                case Command.GloGetResponse:
                case Command.GloSetResponse:
                case Command.GloMethodResponse:
                case Command.DedGetRequest:
                case Command.DedSetRequest:
                //case Command.DedEventNotificationRequest:
                case Command.DedMethodRequest:
                case Command.DedGetResponse:
                case Command.DedSetResponse:
                case Command.DedMethodResponse:
                case Command.GeneralCiphering:
                    ulong num1 = 0;
                    int num2;
                    if (command == Command.GeneralCiphering)
                    {
                        byte[] target1 = new byte[TSTCommon.GetObjectCount(data)];
                        data.Get(target1);
                        num1 = new GXByteBuffer(target1).GetUInt64();
                        byte[] target2 = new byte[TSTCommon.GetObjectCount(data)];
                        data.Get(target2);
                        p.SystemTitle = target2;
                        byte[] target3 = new byte[TSTCommon.GetObjectCount(data)];
                        data.Get(target3);
                        p.RecipientSystemTitle = target3;
                        int objectCount = TSTCommon.GetObjectCount(data);
                        if (objectCount != 0)
                        {
                            byte[] target4 = new byte[objectCount];
                            data.Get(target4);
                            p.DateTime = target4;
                        }
                        int uint8_1 = (int)data.GetUInt8();
                        if (uint8_1 != 0)
                        {
                            byte[] target5 = new byte[uint8_1];
                            data.Get(target5);
                            p.OtherInformation = target5;
                        }
                        num2 = (int)data.GetUInt8();
                        int uint8_2 = (int)data.GetUInt8();
                        num2 = (int)data.GetUInt8();
                        int uint8_3 = (int)data.GetUInt8();
                        p.KeyParameters = uint8_3;
                        if (uint8_3 == 1)
                        {
                            byte[] target6 = new byte[TSTCommon.GetObjectCount(data)];
                            data.Get(target6);
                            p.KeyCipheredData = target6;
                        }
                        else
                        {
                            if (uint8_3 != 2)
                                throw new ArgumentException("key-parameters");
                            if (TSTCommon.GetObjectCount(data) != 0)
                                throw new ArgumentException("Invalid key parameters");
                        }
                    }
                    num2 = TSTCommon.GetObjectCount(data);
                    p.CipheredContent = data.Remaining();
                    byte uint8 = data.GetUInt8();
                    Security security = (Security)((int)uint8 & 48);
                    if (((uint)uint8 & 128U) > 0U)
                        Debug.WriteLine("Compression is used.");
                    if (((uint)uint8 & 64U) > 0U)
                        Debug.WriteLine("Error: Key_Set is used.");
                    if (((uint)uint8 & 32U) > 0U)
                        Debug.WriteLine("Encryption is applied.");
                    SecuritySuite securitySuite = (SecuritySuite)((int)uint8 & 3);
                    p.Security = security;
                    uint uint32 = data.GetUInt32();
                    p.InvocationCounter = (ulong)uint32;
                    if (securitySuite != 0)
                        throw new NotImplementedException("Security Suite 1 is not implemented.");
                    //Debug.WriteLine("Decrypt settings: " + p.ToString());
                    //Debug.WriteLine("Encrypted: " + TSTCommon.ToHex(data.Data, false, data.Position, data.Size - data.Position));
                    byte[] numArray1 = new byte[12];
                    if (security == Security.Authentication)
                    {
                        byte[] numArray2 = new byte[data.Size - data.Position - 12];
                        data.Get(numArray2);
                        data.Get(numArray1);
                        GXDLMSCiphering.EncryptAesGcm(p, numArray2);
                        if (!GXDLMSCipheringStream.TagsEquals(numArray1, p.CountTag))
                        {
                            if (num1 > 0UL)
                                p.InvocationCounter = num1;
                            if (p.Xml == null)
                                throw new GXDLMSException("Decrypt failed. Invalid tag.");
                            p.Xml.AppendComment("Decrypt failed. Invalid tag.");
                        }
                        return numArray2;
                    }
                    byte[] numArray3 = (byte[])null;
                    if (security == Security.Encryption)
                    {
                        numArray3 = new byte[data.Size - data.Position];
                        data.Get(numArray3);
                    }
                    else if (security == Security.AuthenticationEncryption)
                    {
                        numArray3 = new byte[data.Size - data.Position - 12];
                        data.Get(numArray3);
                        data.Get(numArray1);
                    }
                    byte[] authenticatedData = GXDLMSCiphering.GetAuthenticatedData(p, numArray3);
                    byte[] nonse = GXDLMSCiphering.GetNonse(uint32, p.SystemTitle);
                    GXDLMSCipheringStream cipheringStream = new GXDLMSCipheringStream(security, true, p.BlockCipherKey, authenticatedData, nonse, numArray1);
                    cipheringStream.Write(numArray3);
                    if (num1 > 0UL)
                        p.InvocationCounter = num1;
                    return cipheringStream.FlushFinalBlock();
                case Command.GeneralGloCiphering:
                case Command.GeneralDedCiphering:
                    int objectCount1 = TSTCommon.GetObjectCount(data);
                    if (objectCount1 != 0)
                    {
                        p.SystemTitle = new byte[objectCount1];
                        data.Get(p.SystemTitle);
                        if (p.Xml != null && p.Xml.Comments)
                            p.Xml.AppendComment(TSTCommon.SystemTitleToString("DLMS", p.SystemTitle));
                        goto case Command.GloInitiateRequest;
                    }
                    else
                        goto case Command.GloInitiateRequest;
                default:
                    throw new ArgumentOutOfRangeException("cryptedData");
            }
        }
        internal static byte[] EncryptAesGcm(AesGcmParameter param, byte[] plainText)
        {
            Debug.WriteLine("Encrypt settings: " + param.ToString());
            param.CountTag = (byte[])null;
            GXByteBuffer gxByteBuffer = new GXByteBuffer();
            if (param.Type == CountType.Packet)
                gxByteBuffer.SetUInt8((byte)param.Security);
            byte[] array = ((IEnumerable<byte>)BitConverter.GetBytes((uint)param.InvocationCounter)).Reverse<byte>().ToArray<byte>();
            byte[] authenticatedData = GXDLMSCiphering.GetAuthenticatedData(param, plainText);
            GXDLMSCipheringStream cipheringStream = new GXDLMSCipheringStream(param.Security, true, param.BlockCipherKey, authenticatedData, GXDLMSCiphering.GetNonse((uint)param.InvocationCounter, param.SystemTitle), (byte[])null);
            if (param.Security != Security.Authentication)
                cipheringStream.Write(plainText);
            byte[] numArray = cipheringStream.FlushFinalBlock();
            if (param.Security == Security.Authentication)
            {
                if (param.Type == CountType.Packet)
                    gxByteBuffer.Set(array);
                if ((param.Type & CountType.Data) != 0)
                    gxByteBuffer.Set(plainText);
                if ((param.Type & CountType.Tag) != 0)
                {
                    param.CountTag = cipheringStream.GetTag();
                    gxByteBuffer.Set(param.CountTag);
                }
            }
            else if (param.Security == Security.Encryption)
            {
                if (param.Type == CountType.Packet)
                    gxByteBuffer.Set(array);
                gxByteBuffer.Set(numArray);
            }
            else
            {
                if (param.Security != Security.AuthenticationEncryption)
                    throw new ArgumentOutOfRangeException("security");
                if (param.Type == CountType.Packet)
                    gxByteBuffer.Set(array);
                if ((param.Type & CountType.Data) != 0)
                    gxByteBuffer.Set(numArray);
                if ((param.Type & CountType.Tag) != 0)
                {
                    param.CountTag = cipheringStream.GetTag();
                    gxByteBuffer.Set(param.CountTag);
                }
            }
            if (param.Type == CountType.Packet)
            {
                GXByteBuffer buff = new GXByteBuffer((ushort)(10 + gxByteBuffer.Size));
                buff.SetUInt8(param.Tag);
                if (param.Tag == (byte)219 || param.Tag == (byte)220 || param.Tag == (byte)15)
                {
                    if (!param.IgnoreSystemTitle)
                    {
                        TSTCommon.SetObjectCount(param.SystemTitle.Length, buff);
                        buff.Set(param.SystemTitle);
                    }
                    else
                        buff.SetUInt8((byte)0);
                }
                TSTCommon.SetObjectCount(gxByteBuffer.Size, buff);
                buff.Set(gxByteBuffer.Array());
                return buff.Array();
            }
            byte[] bytes = gxByteBuffer.Array();
            Debug.WriteLine("Crypted: " + TSTCommon.ToHex(bytes, true));
            return bytes;
        }

        private static byte[] GetAuthenticatedData(AesGcmParameter p, byte[] plainText)
        {
            if (p.Security == Security.Authentication)
            {
                GXByteBuffer gxByteBuffer = new GXByteBuffer();
                gxByteBuffer.SetUInt8((byte)p.Security);
                gxByteBuffer.Set(p.AuthenticationKey);
                gxByteBuffer.Set(plainText);
                return gxByteBuffer.Array();
            }
            if (p.Security == Security.Encryption)
                return p.AuthenticationKey;
            if (p.Security != Security.AuthenticationEncryption)
                return (byte[])null;
            GXByteBuffer gxByteBuffer1 = new GXByteBuffer();
            gxByteBuffer1.SetUInt8((byte)p.Security);
            gxByteBuffer1.Set(p.AuthenticationKey);
            return gxByteBuffer1.Array();
        }
    }
}
