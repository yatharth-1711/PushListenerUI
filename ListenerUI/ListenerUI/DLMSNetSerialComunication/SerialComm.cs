using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gurux.DLMS.Secure;
using Gurux.DLMS;
using Gurux.Serial;
using log4net;
using System.Reflection;
using Gurux.Net;
using MeterReader.Converter;
using System.Diagnostics;
using Gurux.Common;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Objects.Enums;
using MeterComm.DLMS;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Objects;
using System.Threading;
using System.IO.Ports;
using AutoTest.FrameWork.Converts;
using MeterComm;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Utilities.Encoders;
using System.Data.Odbc;
using System.Xml.Linq;
using MeterReader.TestHelperClasses;

namespace MeterReader.DLMSNetSerialCommunication
{
    internal class SerialComm : IDisposable
    {
        #region Declaration
        //Logger
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //delegate for Error Text
        public delegate void LineTrafficControl(string updatedText, string status);
        //initial  Event
        public static event LineTrafficControl LineTrafficControlEventHandler = delegate { }; // add empty delegate!;
        //Delegate for Line traffic save
        public delegate void LineTrafficSave();
        public static event LineTrafficSave LineTrafficSaveEventHandler = delegate { };// add empty delegate!
        /// <summary>
        /// Wait time in ms.
        /// </summary>
        public int WaitTime = 5000;
        /// <summary>
        /// Retry count.
        /// </summary>
        public int RetryCount = 3;
        /// <summary>
        /// Notify caller from the notification event.
        /// </summary>
        ///  /// <summary>
        /// Notify caller from the notification event.
        /// </summary>
        public Action<object> OnNotification;
        TraceLevel Trace;
        private GXSerial connection;
        public GXDLMSSecureClient dlmsClient;
        GXReplyData reply = new GXReplyData();
        byte[] data;
        public static string recData = "";
        public static string partialRecData = "";
        WrapperParser parse;
        public static string LOG_DIRECTORY = "";
        public static Dictionary<string, object> staticDownloadedList = new Dictionary<string, object>();
        #endregion

        #region constructor
        public SerialComm(string Path)
        {
            LOG_DIRECTORY = Path;
            Trace = TraceLevel.Info;
            InitializeDLMSClient();
            parse = new WrapperParser();
        }
        private void InitializeDLMSClient()
        {
            try
            {
                string passsword = "";
                Authentication authentication = Authentication.None;
                Security security = Security.None;
                if (DLMSInfo.IsLNWithCipher)
                {
                    security = Security.AuthenticationEncryption;
                }
                else
                {
                    security = Security.None;
                    authentication = Authentication.None;
                }
                int clientAddress = 16;
                if (DLMSInfo.AccessMode == 0)
                {
                    clientAddress = 16;
                    passsword = DLMSInfo.MeterAuthPassword;
                    authentication = Authentication.None;
                }
                else if (DLMSInfo.AccessMode == 2)
                {
                    clientAddress = 48;
                    passsword = DLMSInfo.MeterAuthPasswordWrite;
                    authentication = Authentication.High;
                }
                else if (DLMSInfo.AccessMode == 1)
                {
                    clientAddress = 32;
                    passsword = DLMSInfo.MeterAuthPassword;
                    authentication = Authentication.Low;
                }
                else if (DLMSInfo.AccessMode == 4)
                {
                    clientAddress = 80;
                    passsword = DLMSInfo.MeterAuthPasswordWrite;
                    authentication = Authentication.High;
                }

                dlmsClient = new GXDLMSSecureClient(
                true,                                   // Logical Name referencing
                clientAddress,                          // dlmsClient Address
                1,                                      // Server Address
                authentication,                    // Change based on user input
                $"{passsword}",   // Meter password
                InterfaceType.HDLC
                );
                dlmsClient.UseLogicalNameReferencing = true;
                dlmsClient.ServerAddressSize = (byte)1;
                dlmsClient.Authentication = authentication;


                // Configure Ciphering
                dlmsClient.Ciphering.SystemTitle = GXCommon.GetAsByteArray(DLMSInfo.TxtSysT);
                dlmsClient.Ciphering.Security = security; // Adjust as needed
                dlmsClient.Ciphering.BlockCipherKey = GXCommon.GetAsByteArray(DLMSInfo.TxtEK); ;
                dlmsClient.Ciphering.AuthenticationKey = GXCommon.GetAsByteArray(DLMSInfo.TxtAK);
                dlmsClient.Ciphering.SecuritySuite = SecuritySuite.Suite0;
                dlmsClient.Ciphering.DedicatedKey = GXCommon.GetAsByteArray(DLMSInfo.TxtAK);
                dlmsClient.Ciphering.InvocationCounter = 0U;
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.Message.ToString()}");
            }
        }

        #endregion

        #region Connect Disconnect Methods
        public bool SignONDLMS(out string signONErrorMessage)
        {
            bool IsSuccessfull = false;
            signONErrorMessage = "";
            IsSuccessfull = Connect();
            if (IsSuccessfull)
            {
                IsSuccessfull = SNRMRequest();
                if (IsSuccessfull)
                {
                    IsSuccessfull = AARQRequest();
                    if (!IsSuccessfull)
                    {
                        signONErrorMessage = "AARQ Error";
                    }
                }
                else
                {
                    signONErrorMessage = "SNRM Error";
                }
            }
            else
            {
                signONErrorMessage = "Port Connect Error";
            }
            return IsSuccessfull;
        }
        /// <summary>
        /// This Initialize the GXNet connection object
        /// </summary>
        /// <returns>Return True if It get connected.</returns>
        public bool Connect()
        {
            //LineTrafficControlEventHandler($"\r\n     Server Connect Request", "Send");
            bool IsConnected = false;
            try
            {
                connection = new GXSerial();
                connection.PortName = DLMSInfo.comPort;
                connection.BaudRate = DLMSInfo.BaudRate;
                connection.StopBits = StopBits.One;
                connection.DataBits = 8;
                connection.Parity = Parity.None;
                connection.DtrEnable = connection.RtsEnable = true;
                connection.Trace = TraceLevel.Info;
                connection.Open();
                IsConnected = true;
                //LineTrafficControlEventHandler($"\r\n     Server Connected Successfully to {ip}  {port}\r\n", "Send");
                //if (IsloggerON)
                //_logService.LogMessage(_logBox, $"Connected at IP: {ip} at Port: {port}",Color.Green);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.Message.ToString()}");
                //LineTrafficControlEventHandler($"\r\n     Server Connect Error: {ex.Message.ToString()}\r\n", "Receive");
                //Log($"Error: {ex.Message}", Color.Red);
            }
            return IsConnected;
        }
        /// <summary>
        /// Directly Close the GXNet connection
        /// </summary>
        public void Disconnect()
        {
            LineTrafficControlEventHandler($"\r\n     DISCONNECT SERVER\r\n", "Send");
            try
            {
                connection?.Close();
                LineTrafficControlEventHandler($"     DISCONNECT SERVER Successfully\r\n", "Send");
                //MessageBox.Show("Disconnected successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message.ToString()}");
                LineTrafficControlEventHandler($"     DISCONNECT SERVER Error: {ex.Message.ToString()}\r\n", "Receive");
                //MessageBox.Show($"Disconnection error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region DLMS Methods
        /// <summary>
        /// Send SNRM Request to the meter.
        /// </summary>
        public bool SNRMRequest()
        {
            LineTrafficControlEventHandler($"\r\n     SNRM Request", "Send");
            bool IsSNRMPassed = false;
            try
            {
                ReadDataBlock(dlmsClient.SNRMRequest(), reply);
                //Log("SNRM Response " + reply.ToString(), Color.Green);
                try
                {
                    dlmsClient.ParseUAResponse(reply.Data);
                    IsSNRMPassed = true;
                    LineTrafficControlEventHandler($"     SNRM Success\r\n", "Send");
                    reply.Clear();
                }
                catch (Exception ex)
                {
                    reply.Clear();
                    log.Error($"{ex.Message.ToString()}");
                    IsSNRMPassed = false;
                    //Log(ex.Message.ToString(), Color.Red);
                }
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message.ToString()}");
                IsSNRMPassed = false;
                LineTrafficControlEventHandler($"     SNRM Error: {ex.Message.ToString()}\r\n", "Receive");
            }
            return IsSNRMPassed;
        }
        /// <summary>
        /// Send AARQ Request to the meter.
        /// </summary>
        public bool AARQRequest()
        {
            LineTrafficControlEventHandler($"\r\n     AARQ Request", "Send");
            bool IsAARQPassed = false;
            try
            {
                ReadDataBlock(dlmsClient.AARQRequest(), reply);
                //Log("AARQ Response " + reply.ToString(), Color.Green);
                try
                {
                    dlmsClient.ParseAAREResponse(reply.Data);
                    IsAARQPassed = true;
                    reply.Clear();
                }
                catch (Exception ex)
                {
                    reply.Clear();
                    log.Error($"{ex.Message.ToString()}");
                    IsAARQPassed = false;
                    //Log(ex.Message.ToString(), Color.Red);
                }
                reply.Clear();
                if (dlmsClient.Authentication != Authentication.None && dlmsClient.Authentication != Authentication.Low)
                {
                    ReadDataBlock(dlmsClient.GetApplicationAssociationRequest(), reply);
                    dlmsClient.ParseApplicationAssociationResponse(reply.Data);
                    IsAARQPassed = true;
                }
                if (IsAARQPassed)
                    LineTrafficControlEventHandler($"     AARQ Success\r\n", "Send");
                else
                    LineTrafficControlEventHandler($"     AARQ Error\r\n", "Receive");
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message.ToString()}");
                IsAARQPassed = false;
                LineTrafficControlEventHandler($"     AARQ Error: {ex.Message.ToString()}\r\n", "Receive");
            }
            return IsAARQPassed;
        }

        /// <summary>
        /// Send SNRM Request to the meter.
        /// </summary>
        public bool DisconnectRequest()
        {
            LineTrafficControlEventHandler($"\r\n     DISCONNECT COMMAND", "Send");
            bool IsDisconnectPassed = false;
            reply.Clear();
            try
            {
                ReadDataBlock(dlmsClient.DisconnectRequest(), reply);
                //Log("SNRM Response " + reply.ToString(), Color.Green);
                try
                {
                    dlmsClient.ParseUAResponse(reply.Data);
                    IsDisconnectPassed = true;
                    LineTrafficControlEventHandler($"     SUCCESSFULLY DISCONNECTED\r\n", "Response");
                    reply.Clear();
                }
                catch (Exception ex)
                {
                    reply.Clear();
                    log.Error($"{ex.Message.ToString()}");
                    IsDisconnectPassed = false;
                    //Log(ex.Message.ToString(), Color.Red);
                }
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message.ToString()}");
                IsDisconnectPassed = false;
                LineTrafficControlEventHandler($"     ERROR IN DISCONNECTION: {ex.Message.ToString()}\r\n", "Response");
            }
            return IsDisconnectPassed;
        }

        /// <summary>
        /// Send data block(s) to the meter.
        /// </summary>
        /// <param name="data">Send data block(s).</param>
        /// <param name="reply">Received reply from the meter.</param>
        /// <returns>Return false if frame is rejected.</returns>
        public bool ReadDataBlock(byte[][] data, GXReplyData reply)
        {
            if (data == null)
            {
                return true;
            }
            foreach (byte[] it in data)
            {
                //Log($"\r\nSend Command:\r\n{GXCommon.ToHex(it, true)}", Color.Green);
                //LineTrafficControlEventHandler($"\r\nSend Command:\r\n{GXCommon.ToHex(it, true)}", "Send");
                reply.Clear();
                ReadDataBlock(it, reply);
                //Log($"Decoded Message:\r\n{GXCommon.ToHex(reply.Data.Data, true)}", Color.Red);
                //LineTrafficControlEventHandler($"\r\nDecoded Message:\r\n{GXCommon.ToHex(reply.Data.Data, true)}\r\n", "Send");
            }
            return reply.Error == 0;
        }

        /// <summary>
        /// Read data block from the device.
        /// </summary>
        /// <param name="data">data to send</param>
        /// <param name="text">Progress text.</param>
        /// <param name="multiplier"></param>
        /// <returns>Received data.</returns>
        public void ReadDataBlock(byte[] data, GXReplyData reply)
        {
            ReadDLMSPacket(data, reply);
            partialRecData = "";
            lock (connection.Synchronous)
            {

                while (reply.IsMoreData &&
                    (dlmsClient.ConnectionState != Gurux.DLMS.Enums.ConnectionState.None ||
                    dlmsClient.PreEstablishedConnection))
                {
                    if (reply.IsStreaming())
                    {
                        data = null;
                    }
                    else
                    {
                        data = dlmsClient.ReceiverReady(reply);
                    }
                    ReadDLMSPacket(data, reply);
                    partialRecData = reply.ToString().Replace(" ", "");
                }

            }
            //Log($"{GXCommon.ToHex(reply.Data.Data, true)}\n", Color.Black);
        }

        /// <summary>
        /// Read DLMS Data from the device.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <returns>Received data.</returns>
        public void ReadDLMSPacket(byte[] data, GXReplyData reply)
        {
            string decodedXml = "";
            if (data == null && !reply.IsStreaming())
            {
                return;
            }
            GXReplyData notify = new GXReplyData();
            reply.Error = 0;
            object eop = (byte)0x7E;
            //In network connection terminator is not used.
            if (dlmsClient.InterfaceType != InterfaceType.HDLC &&
                dlmsClient.InterfaceType != InterfaceType.HdlcWithModeE)
            {
                eop = null;
            }
            int pos = 0;
            bool succeeded = false;
            GXByteBuffer rd = new GXByteBuffer();
            ReceiveParameters<byte[]> p = new ReceiveParameters<byte[]>()
            {
                Eop = eop,
                Count = dlmsClient.GetFrameSize(rd),
                AllData = true,
                WaitTime = WaitTime,
            };
            lock (connection.Synchronous)
            {
                while (!succeeded && pos != 3)
                {
                    if (!reply.IsStreaming())
                    {
                        //WriteTrace("TX:\t" + GXCommon.ToHex(data, true));
                        LineTrafficControlEventHandler($"\r\n(S)  {GXCommon.ToHex(data, true)} {GetTimeStamp()}", "Send");
                        //if (DLMSInfo.IsLNWithCipher)
                        //    LineTrafficControlEventHandler($"\r\n(XML)\r\n{DecodePushDataToXML(data)} {GetTimeStamp()}", "Send");
                        p.Reply = null;
                        connection.Send(data);
                        SetGetFromMeter.Wait(100);
                    }
                    succeeded = connection.Receive(p);
                    if (!succeeded)
                    {
                        if (++pos >= RetryCount)
                        {
                            LineTrafficControlEventHandler($"\r\nFailed to receive reply from the device in given time.\r\n", "Receive");
                            throw new Exception("Failed to receive reply from the device in given time.");
                        }
                        //If Eop is not set read one byte at time.
                        if (p.Eop == null)
                        {
                            p.Count = 1;
                        }
                        //Try to read again...
                        System.Diagnostics.Debug.WriteLine("Data send failed. Try to resend " + pos.ToString() + "/3");
                        LineTrafficControlEventHandler("\r\n" + "Data send failed. Try to resend " + pos.ToString() + "/3" + "\r\n", "Receive");
                    }
                }
                rd = new GXByteBuffer(p.Reply);
                try
                {
                    pos = 0;
                    //Loop until whole COSEM packet is received.
                    while (!dlmsClient.GetData(rd, reply, notify))
                    {
                        p.Reply = null;
                        if (notify.IsComplete && notify.Data.Data != null)
                        {
                            //Handle notify.
                            if (!notify.IsMoreData)
                            {
                                if (notify.PrimeDc != null)
                                {
                                    OnNotification?.Invoke(notify.PrimeDc);
                                    Console.WriteLine(notify.PrimeDc);
                                }
                                else
                                {
                                    //Show received push message as XML.
                                    string xml;
                                    GXDLMSTranslator t = new GXDLMSTranslator(TranslatorOutputType.SimpleXml);
                                    t.DataToXml(notify.Data, out xml);
                                    OnNotification?.Invoke(xml);
                                    Console.WriteLine(xml);
                                }
                                notify.Clear();
                                continue;
                            }
                        }
                        if (p.Eop == null)
                        {
                            p.Count = dlmsClient.GetFrameSize(rd);
                        }
                        while (!connection.Receive(p))
                        {
                            if (++pos >= RetryCount)
                            {
                                LineTrafficControlEventHandler("\r\nFailed to receive reply from the device in given time.\r\n", "Receive");
                                throw new Exception("Failed to receive reply from the device in given time.");
                            }
                            p.Reply = null;
                            connection.Send(data);
                            //Try to read again...
                            LineTrafficControlEventHandler("\r\nData send failed. Try to resend" + pos.ToString() + "/3" + "\r\n", "Receive");
                            System.Diagnostics.Debug.WriteLine("Data send failed. Try to resend " + pos.ToString() + "/3");
                        }
                        rd.Set(p.Reply);
                    }
                    //Log($"Reply: {GXCommon.ToHex(reply.Data.Data, true)}", Color.Magenta);
                }
                catch (Exception ex)
                {
                    //WriteTrace("RX:\t" + rd);
                    LineTrafficControlEventHandler($"\r\n(R)  {rd} {GetTimeStamp()}\r\n", "Receive");
                    throw ex;
                }
            }
            //Log($"Encrypted Response: {GXCommon.ToHex(rd.Data, true)}", Color.Black);
            //string decodedXml = DecodePushDataToXML(rd.Data);
            //Log($"XML:\r\n{decodedXml}", Color.DarkViolet);
            //string decryptData = GetDecryptData(decodedXml);
            //Log($"DECRYPT DATA:\r\n{decryptData}", Color.DarkViolet);
            //WriteTrace("RX:\t" + rd);
            LineTrafficControlEventHandler($"\r\n(R)  {rd} {GetTimeStamp()}\r\n", "Receive");
            //if (DLMSInfo.IsLNWithCipher)
            //    LineTrafficControlEventHandler($"\r\n(XML)\r\n{DecodePushDataToXML(rd.Data)} {GetTimeStamp()}\r\n", "Receive");
            if (reply.Error != 0)
            {
                if (reply.Error == (short)ErrorCode.Rejected)
                {
                    Thread.Sleep(1000);
                    ReadDLMSPacket(data, reply);
                }
                else
                {
                    throw new GXDLMSException(reply.Error);
                }
            }
        }
        public void Write(GXDLMSObject obj, int index, object writeValue = null)
        {
            object val;
            //Read(obj, index);
            GXReplyData reply = new GXReplyData();
            for (int it = 1; it != (obj as IGXDLMSBase).GetAttributeCount() + 1; ++it)
            {
                reply.Clear();
                if (it == index || (index == 0 && obj.GetDirty(it, out val)))
                {
                    bool forced = false;
                    GXDLMSAttributeSettings att = obj.Attributes.Find(it);
                    //Read DLMS data type if not known.
                    DataType type = obj.GetDataType(it); //int,uint8,string,etc.....
                    if (type == DataType.None)
                    {
                        byte[] data = dlmsClient.Read(obj, it)[0];
                        ReadDataBlock(data, reply); //Data, multiplier, trycount, reply
                        type = reply.DataType;
                        if (type != DataType.None)
                        {
                            obj.SetDataType(it, type);
                        }
                        reply.Clear();
                    }
                    try
                    {
                        if (att != null && att.ForceToBlocks)
                        {
                            forced = dlmsClient.ForceToBlocks = true;
                        }
                        try
                        {
                            dlmsClient.UpdateValue(obj, index, writeValue);
                            byte[][] response = dlmsClient.Write(obj, it);
                            byte[] testbb = new byte[1024];
                            foreach (byte[] buffer in response)
                            {
                                testbb = buffer;
                                //Log("Write Send Encrypted:\r\n" + GXCommon.ToHex(buffer), Color.DarkOrange);
                            }
                            //string decodedXml = DecodePushDataToXML(testbb);
                            //Log($"XML:\r\n{decodedXml}", Color.DarkViolet);
                            //string decryptData = GetDecryptData(decodedXml);
                            //Log($"DECRYPT DATA:\r\n{decryptData}", Color.DarkViolet);
                            //Log($"Write Response Encrypted:\r\n {GXCommon.ToHex(reply.Data.Data, true)}", Color.DarkOrange);
                            reply.Clear();
                            ReadDataBlock(response, reply);
                            ValueEventArgs e1 = new ValueEventArgs(obj, it, 0, null);
                            string xml = GXDLMSTranslator.ValueToXml(((IGXDLMSBase)obj).GetValue(dlmsClient.Settings, e1));
                        }
                        catch (GXDLMSException ex)
                        {
                            throw ex;
                        }
                        //Read data once again to make sure it is updated.
                        //reply.Clear();
                        //byte[] data = dlmsClient.Read(obj, it)[0];
                        //ReadDataBlock(data, reply);
                        //val = reply.Value;
                        //if (val is byte[] && (type = obj.GetUIDataType(it)) != DataType.None)
                        //{
                        //    val = GXDLMSClient.ChangeType((byte[])val, type, dlmsClient.UseUtc2NormalTime);
                        //}
                        //dlmsClient.UpdateValue(obj, it, val);
                    }
                    finally
                    {
                        if (forced)
                        {
                            dlmsClient.ForceToBlocks = false;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Read attribute value.
        /// </summary>
        /// <param name="it">COSEM object to read.</param>
        /// <param name="attributeIndex">Attribute index.</param>
        /// <returns>Read value.</returns>
        public object Read(GXDLMSObject it, int attributeIndex)
        {
            if (dlmsClient.CanRead(it, attributeIndex))
            {
                GXReplyData reply = new GXReplyData();
                if (!ReadDataBlock(dlmsClient.Read(it, attributeIndex), reply))
                {
                    if (reply.Error != (short)ErrorCode.Rejected)
                    {
                        throw new GXDLMSException(reply.Error);
                    }
                    reply.Clear();
                    Thread.Sleep(1000);
                    if (!ReadDataBlock(dlmsClient.Read(it, attributeIndex), reply))
                    {
                        throw new GXDLMSException(reply.Error);
                    }
                }
                //Log($"(R)    {reply.ToString()}", Color.Green);
                //Update data type.
                if (it.GetDataType(attributeIndex) == DataType.None)
                {
                    it.SetDataType(attributeIndex, reply.DataType);
                }
                return dlmsClient.UpdateValue(it, attributeIndex, reply.Value);
            }
            else
            {
                Console.WriteLine("Can't read " + it.ToString() + ". Not enought acccess rights.");
            }
            return null;
        }

        /// <summary>
        /// Method attribute value.
        /// </summary>
        public void Method(GXDLMSObject it, int attributeIndex, object value)
        {
            GXReplyData reply = new GXReplyData();
            ReadDataBlock(dlmsClient.Method(it, attributeIndex, value, GXDLMSConverter.GetDLMSDataType(value)), reply);
        }

        /// <summary>
        /// Updates one or more global keys.
        /// </summary>
        /// <param name="client">DLMS client that is used to generate action.</param>
        /// <param name="kek">Master key, also known as Key Encrypting Key.</param>
        /// <param name="list">List of Global key types and keys.</param>
        /// <returns>Generated action.</returns>
        public bool GlobalKeyTransfer(string globalKey, GlobalKeyType keyType = GlobalKeyType.UnicastEncryption)
        {
            bool result = false;
            List<KeyValuePair<GlobalKeyType, byte[]>> list = new List<KeyValuePair<GlobalKeyType, byte[]>>();//List of Global key types and keys.
            if (keyType == GlobalKeyType.UnicastEncryption)
                list.Add(new KeyValuePair<GlobalKeyType, byte[]>(GlobalKeyType.UnicastEncryption, GXCommon.GetAsByteArray(globalKey)));
            else
                list.Add(new KeyValuePair<GlobalKeyType, byte[]>(GlobalKeyType.Authentication, GXCommon.GetAsByteArray(globalKey)));
            byte[] kek = GXCommon.GetAsByteArray(DLMSInfo.MasterKey);//Master key, also known as Key Encrypting Key
            if (list == null || list.Count == 0)
            {
                throw new ArgumentException("Invalid list. It is empty.");
            }
            GXByteBuffer bb = new GXByteBuffer();
            bb.SetUInt8((byte)DataType.Array);
            bb.SetUInt8((byte)list.Count);
            byte[] tmp;
            foreach (KeyValuePair<GlobalKeyType, byte[]> it in list)
            {
                bb.SetUInt8((byte)DataType.Structure);
                bb.SetUInt8(2);
                GXCommonNotDefinedMethods.SetData(dlmsClient.Settings, bb, DataType.Enum, it.Key);
                tmp = GXDLMSSecureClient.Encrypt(kek, it.Value);
                GXCommonNotDefinedMethods.SetData(dlmsClient.Settings, bb, DataType.OctetString, tmp);
            }
            GXReplyData reply = new GXReplyData();
            ReadDataBlock(dlmsClient.Method(CreateObjectForWrite(ObjectType.SecuritySetup, "0.0.43.0.3.255", 2, DataType.Array, AccessMode.Read), 2, bb.Array(), DataType.Array), reply);
            if (reply.Error == 0 && reply.ErrorMessage == "")
            {
                result = true;
            }
            return result;
        }

        #endregion

        #region User Methods
        /// <summary>
        /// This Takes Object Type (Class), lnName (OBIS) and attribute index of object 
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="lnName"></param>
        /// <param name="attributeIndex"></param>
        /// <returns>return read in object form</returns>
        /// <exception cref="GXDLMSException"></exception>
        public object ReadCOSEMObject(ObjectType objType, string lnName, int attributeIndex)
        {
            LineTrafficControlEventHandler($"\r\n     GET CLASS-{(int)objType} | OBIS-{lnName} [{DLMSParser.GetObisName(((int)objType).ToString(), lnName, ((int)attributeIndex).ToString())}] | Attribute-{attributeIndex}", "Send");
            GXDLMSObject createObjToRead = CreateObjectForRead(objType, lnName, attributeIndex, AccessMode.ReadWrite);

            if (dlmsClient.CanRead(createObjToRead, attributeIndex))
            {
                GXReplyData reply = new GXReplyData();
                if (!ReadDataBlock(dlmsClient.Read(createObjToRead, attributeIndex), reply))
                {
                    if (reply.Error != (short)ErrorCode.Rejected)
                    {
                        throw new GXDLMSException(reply.Error);
                    }
                    reply.Clear();
                    Thread.Sleep(1000);
                    if (!ReadDataBlock(dlmsClient.Read(createObjToRead, attributeIndex), reply))
                    {
                        throw new GXDLMSException(reply.Error);
                    }
                }
                if (createObjToRead.GetDataType(attributeIndex) == DataType.None)
                {
                    createObjToRead.SetDataType(attributeIndex, reply.DataType);
                }
                recData = reply.ToString().Replace(" ", "");
                LineTrafficControlEventHandler($"     Received Data:\r\n     {reply.ToString()}\r\n", "Response");
                return dlmsClient.UpdateValue(createObjToRead, attributeIndex, reply.Value);
            }
            return null;
        }
        /// <summary>
        /// This Takes Object Type (Class), lnName (OBIS) and attribute index of object. DataType of the write Data and sData is Data to write
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="lnName"></param>
        /// <param name="attributeIndex"></param>
        /// <param name="dataType"></param>
        /// <param name="sData"></param>
        public void WriteData(ObjectType objType, string lnName, int attributeIndex, DataType dataType, object sData)
        {
            LineTrafficControlEventHandler($"\r\n     SET CLASS-{(int)objType} | OBIS-{lnName} [{DLMSParser.GetObisName(((int)objType).ToString(), lnName, ((int)attributeIndex).ToString())}] | Attribute-{attributeIndex}\r\n", "Send");
            Write(CreateObjectForWrite(objType, lnName, attributeIndex, dataType, AccessMode.ReadWrite), attributeIndex, sData);
        }

        /// <summary>
        /// Takes the HEX data string which needs to be set.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string WriteDayProfileData(string Value)
        {
            LineTrafficControlEventHandler($"\r\n     SET CLASS-{(int)ObjectType.ActivityCalendar} | OBIS-{"0.0.13.0.0.255"} [{DLMSParser.GetObisName(((int)ObjectType.ActivityCalendar).ToString(), "0.0.13.0.0.255", ((int)9).ToString())}] | Attribute-{9}", "Send");
            string resultMessage = "";
            try
            {
                byte[] rawData = GXCommon.HexToBytes(Value);
                GXByteBuffer bb = new GXByteBuffer(rawData);

                // Expect Array Tag
                if (bb.GetUInt8() != (byte)DataType.Array)
                    throw new Exception("Expected Array tag at the beginning.");
                int profileCount = bb.GetUInt8();
                List<GXDLMSDayProfile> dayProfiles = new List<GXDLMSDayProfile>();

                for (int i = 0; i < profileCount; i++)
                {
                    if (bb.GetUInt8() != (byte)DataType.Structure)
                        throw new Exception("Expected Structure");

                    int structLen = bb.GetUInt8(); // Typically 2: DayId and array of actions

                    // DayId
                    if (bb.GetUInt8() != (byte)DataType.UInt8)
                        throw new Exception("Expected UInt8 for DayId");
                    byte dayId = bb.GetUInt8();

                    // Array of actions
                    if (bb.GetUInt8() != (byte)DataType.Array)
                        throw new Exception("Expected Array for DayProfileActions");

                    int actionCount = bb.GetUInt8();
                    List<GXDLMSDayProfileAction> actions = new List<GXDLMSDayProfileAction>();

                    for (int j = 0; j < actionCount; j++)
                    {
                        if (bb.GetUInt8() != (byte)DataType.Structure)
                            throw new Exception("Expected Structure for Action");

                        int actionStructLen = bb.GetUInt8(); // should be 3

                        // StartTime (OctetString)
                        if (bb.GetUInt8() != (byte)DataType.OctetString)
                            throw new Exception("Expected OctetString for StartTime");
                        int timeLen = bb.GetUInt8();
                        byte[] timeBytes = new byte[timeLen];
                        bb.Get(timeBytes);
                        GXTime startTime = (GXTime)GXDLMSClient.ChangeType(timeBytes, DataType.Time);

                        // Script Logical Name (OctetString)
                        if (bb.GetUInt8() != (byte)DataType.OctetString)
                            throw new Exception("Expected OctetString for LogicalName");
                        int lnLen = bb.GetUInt8();
                        byte[] lnBytes = new byte[lnLen];
                        bb.Get(lnBytes);
                        string logicalName = string.Join(".", Array.ConvertAll(lnBytes, b => b.ToString()));

                        // ScriptSelector (UInt16)
                        if (bb.GetUInt8() != (byte)DataType.UInt16)
                            throw new Exception("Expected UInt16 for ScriptSelector");
                        ushort selector = bb.GetUInt16();

                        actions.Add(new GXDLMSDayProfileAction
                        {
                            StartTime = startTime,
                            ScriptLogicalName = logicalName,
                            ScriptSelector = selector
                        });
                    }

                    dayProfiles.Add(new GXDLMSDayProfile
                    {
                        DayId = dayId,
                        DaySchedules = actions.ToArray()
                    });
                }

                // Create GXDLMSActivityCalendar and apply parsed DayProfiles
                GXDLMSActivityCalendar cal = new GXDLMSActivityCalendar
                {
                    LogicalName = "0.0.13.0.0.255",
                    DayProfileTablePassive = dayProfiles.ToArray()
                };
                // Add to object list if not already added
                if (!dlmsClient.Objects.Contains(cal))
                    dlmsClient.Objects.Add(cal);

                GXReplyData reply = new GXReplyData();
                reply.Clear();
                // Write DayProfileTablePassive (attribute 9)
                ReadDataBlock(dlmsClient.Write(cal, 9), reply);
                recData = reply.ToString().Replace(" ", "");
                if (reply.ErrorMessage != "")
                {
                    reply.Clear();
                }
                else
                {
                    ReadCOSEMObject(ObjectType.ActivityCalendar, "0.0.13.0.0.255", 9);
                    if (Value.Length == recData.Trim().Length)
                        resultMessage = "Set Successfully";
                    else
                        resultMessage = "Error in Setting";
                }
                reply.Clear();
            }
            catch (Exception ex)
            {
                reply.Clear();
                resultMessage = $"Error: {ex.Message.ToString()}";
                log.Error($"Error: {ex.Message.ToString()}");
            }
            return resultMessage;
        }

        public string ShowValue(object val, int pos)
        {
            //If trace is info.
            if (Trace > TraceLevel.Warning)
            {
                //If data is array.
                if (val is byte[])
                {
                    val = GXCommon.ToHex((byte[])val, true);
                }
                else if (val is Array)
                {
                    string str = "";
                    for (int pos2 = 0; pos2 != (val as Array).Length; ++pos2)
                    {
                        if (str != "")
                        {
                            str += ", ";
                        }
                        if ((val as Array).GetValue(pos2) is byte[])
                        {
                            str += GXCommon.ToHex((byte[])(val as Array).GetValue(pos2), true);
                        }
                        else
                        {
                            str += (val as Array).GetValue(pos2).ToString();
                        }
                    }
                    val = str;
                }
                else if (val is System.Collections.IList)
                {
                    string str = "[";
                    bool empty = true;
                    foreach (object it2 in val as System.Collections.IList)
                    {
                        if (!empty)
                        {
                            str += ", ";
                        }
                        empty = false;
                        if (it2 is byte[])
                        {
                            str += GXCommon.ToHex((byte[])it2, true);
                        }
                        else
                        {
                            str += it2.ToString();
                        }
                    }
                    str += "]";
                    val = str;
                }
                //Console.WriteLine("Index: " + pos + " Value: " + val);
            }
            //Log("Attribute: " + pos + " Value: " + val, Color.Blue);
            return val.ToString();
        }

        #endregion

        #region Helper Methods
        static void RenameParameterWithUnit(DataTable dataTable)
        {
            DLMSParser parse = new DLMSParser();
            // Get the index of the "Scaler" column
            int scalerColumnIndex = dataTable.Columns.IndexOf("Scaler");
            // Get the index of the "Parameter" column
            int parameterColumnIndex = dataTable.Columns.IndexOf("Parameter Name");

            foreach (DataRow row in dataTable.Rows)
            {
                // Get the value of the "Scaler" column for the current row
                string unitString = row.Field<string>(scalerColumnIndex);
                // Check if the scaler value is not null or empty
                if (!string.IsNullOrEmpty(unitString))
                {
                    unitString = (string)parse.UnithshTable[unitString.Substring(6, 2)];
                    if (!string.IsNullOrEmpty(unitString))
                    {
                        // Iterate through each column in the DataTable
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            // Check if the column name contains "Data" and it's not the "Scaler" column
                            if (dataTable.Columns[i].ColumnName.Contains("Parameter"))
                            {
                                // Get the value of the current column for the current row
                                string parameterName = row.Field<string>(i);

                                // Multiply the value in the "Scaler" column with the value in the "Data" column
                                if (!string.IsNullOrEmpty(parameterName))
                                {
                                    row[i] = $"{parameterName} ({unitString})";
                                }
                            }
                        }
                    }
                }
            }
        }

        private string GetTimeStamp()
        {
            return DateTime.Now.ToString(Constants.timeStampFormate);
        }

        private void WriteTrace(string message)
        {
            string logFile = Path.Combine(LOG_DIRECTORY, $"Log_{DateTime.Now:ddMMyyyy}.txt");
            if (!Directory.Exists(LOG_DIRECTORY))
            {
                Directory.CreateDirectory(LOG_DIRECTORY);
            }
            string logMessage = $"[{DateTime.Now:dd-MM-yyyy hh:mm:ss:fff tt}] {message}";
            File.AppendAllText(logFile, logMessage + Environment.NewLine);
        }

        private string GetDecryptData(string XmlData)
        {
            string decryptData = "";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<root>" + $@"{XmlData}" + "</root>"); // Wrapping in a root node for valid XML
            foreach (XmlNode node in xmlDoc.DocumentElement.SelectNodes("//comment()"))
            {
                if (node.NodeType == XmlNodeType.Comment)
                {
                    string commentText = node.Value;
                    Match match = Regex.Match(commentText, @"Decrypt data:\s*([\dA-F ]+)", RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        decryptData = match.Groups[1].Value.Trim();
                        break;
                    }
                }
            }
            return decryptData;

        }

        private string DecodePushDataToXML(byte[] data)
        {
            string dataXML = "";
            try
            {
                GXReplyData reply = new GXReplyData();
                // Decrypt and parse push message
                //dlmsClient.GetData(data, reply);
                GXDLMSTranslator translator = new GXDLMSTranslator(TranslatorOutputType.SimpleXml);
                translator.Comments = true;
                translator.Security = Security.AuthenticationEncryption;
                translator.SecuritySuite = SecuritySuite.Suite0;
                translator.SystemTitle = GXCommon.GetAsByteArray("GOE00000");
                translator.AuthenticationKey = GXCommon.GetAsByteArray("NbPdClEkakNb04ab");
                translator.BlockCipherKey = GXCommon.GetAsByteArray("NbPdClEkakNb04ab");
                translator.InvocationCounter = 0;
                translator.DedicatedKey = GXCommon.GetAsByteArray("NbPdClEkakNb04ab");
                //translator.PduOnly = true;

                GXByteBuffer bb = new GXByteBuffer();
                bb.Set(data);
                GXDLMSTranslatorMessage msg = new GXDLMSTranslatorMessage();
                msg.Message = bb;
                msg.InterfaceType = InterfaceType.WRAPPER;

                dataXML = translator.MessageToXml(bb.Data);
                translator.Clear();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                //_logService.LogMessage(_logBox, $"Push Data Decode Error: {ex.Message}", Color.Red);
            }
            return dataXML;
        }

        public GXDLMSObject CreateObjectForWrite(ObjectType objType, string lnName, int attributeIndex, DataType dataType, AccessMode accessMode)
        {
            GXDLMSActivityCalendar gXDLMSActivityCalendar = new GXDLMSActivityCalendar();
            GXDLMSObject createdObject = GXDLMSClient.CreateObject(objType);
            createdObject.LogicalName = lnName;
            createdObject.SetDataType(attributeIndex, dataType);
            createdObject.SetAccess(attributeIndex, accessMode);
            return createdObject;
        }

        public GXDLMSObject CreateObjectForRead(ObjectType objType, string lnName, int attributeIndex, AccessMode accessMode)
        {
            GXDLMSObject createdObject = GXDLMSClient.CreateObject(objType);
            createdObject.LogicalName = lnName;
            createdObject.SetAccess(attributeIndex, accessMode);
            return createdObject;
        }

        #endregion

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
            connection = null;
            LineTrafficSaveEventHandler();
        }
    }
}
