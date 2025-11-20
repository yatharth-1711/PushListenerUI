using Gurux.DLMS.Secure;
using Gurux.DLMS;
using Gurux.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Gurux.Common;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Objects.Enums;
using MeterReader.TestHelperClasses;
using System.IO;
using MeterComm;
using System.Xml;
using Gurux.DLMS.Objects;
using Gurux.DLMS.ManufacturerSettings;
using AutoTest.FrameWork.Converts;
using MeterComm.DLMS;
using System.Data;
using MeterReader.Converter;
using System.Text;
using System.Text.RegularExpressions;
using AutoTest.FrameWork;
using MeterReader.DLMSInterfaceClasses;

namespace MeterReader.DLMSNetSerialCommunication
{
    public class WrapperComm : IDisposable
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
        public Action<object> OnNotification;
        TraceLevel Trace;
        private GXNet connection;
        public GXDLMSSecureClient dlmsClient;
        GXReplyData reply = new GXReplyData();
        byte[] data;
        public static string recData = "";
        public static string partialRecData = "";
        WrapperParser parse;
        public static string LOG_DIRECTORY = "";
        public static Dictionary<string, object> staticDownloadedList = new Dictionary<string, object>();
        public static bool stopReadingAllData = false;
        #endregion

        #region constructor
        public WrapperComm(string Path)
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
                int clientAddress = 16;
                if (DLMSInfo.AccessMode == 2)
                {
                    clientAddress = 48;
                    passsword = DLMSInfo.MeterAuthPasswordWrite;
                }
                else if (DLMSInfo.AccessMode == 1)
                {
                    clientAddress = 32;
                    passsword = DLMSInfo.MeterAuthPassword;
                }
                else if (DLMSInfo.AccessMode == 4)
                {
                    clientAddress = 80;
                    passsword = DLMSInfo.MeterAuthPasswordWrite;
                }

                dlmsClient = new GXDLMSSecureClient(
                true,                                   // Logical Name referencing
                clientAddress,                          // dlmsClient Address
                1,                                      // Server Address
                Authentication.High,                    // Change based on user input
                $"{DLMSInfo.MeterAuthPasswordWrite}",   // Meter password
                InterfaceType.WRAPPER
                );
                dlmsClient.UseLogicalNameReferencing = true;
                // Configure Ciphering
                dlmsClient.Ciphering.SystemTitle = GXCommon.GetAsByteArray(DLMSInfo.TxtSysT);
                dlmsClient.Ciphering.Security = Security.AuthenticationEncryption; // Adjust as needed
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
                SetGetFromMeter.Wait(5000);
                IsSuccessfull = AARQRequest();
                if (!IsSuccessfull)
                {
                    signONErrorMessage = "AARQ Error";
                    Release();
                    Disconnect();
                }
            }
            else
            {
                signONErrorMessage = "Server Connect Error";
                Disconnect();
            }
            return IsSuccessfull;
        }

        public async Task<bool> SignONDLMSAsync()
        {
            bool isSuccessful = false;
            string signONErrorMessage = "";
            isSuccessful = await ConnectAsync();
            if (isSuccessful)
            {
                SetGetFromMeter.Wait(5000);
                isSuccessful = await AARQRequestAsync();
                if (!isSuccessful)
                {
                    signONErrorMessage = "AARQ Error";
                    await ReleaseAsync();
                    Disconnect();
                }
            }
            else
            {
                signONErrorMessage = "Server Connect Error";
            }
            CommonHelper.signOnErrors = signONErrorMessage;
            return isSuccessful;
        }

        /// <summary>
        /// This Initialize the GXNet connection object
        /// </summary>
        /// <returns>Return True if It get connected.</returns>
        public bool Connect()
        {
            LineTrafficControlEventHandler($"\r\n     Server Connect Request {GetTimeStamp()}", "Send");
            bool IsConnected = false;
            try
            {
                string ip = WrapperInfo.hostName;
                int port = WrapperInfo.port;
                connection = new GXNet(NetworkType.Tcp, ip, port);
                connection.UseIPv6 = true;
                connection.Open();
                SetGetFromMeter.Wait(1000);
                if (connection.IsOpen)
                {
                    IsConnected = true;
                    LineTrafficControlEventHandler($"\r\n     Server Connected Successfully to {ip}  {port} {GetTimeStamp()}\r\n", "Send");
                }
                else
                {
                    IsConnected = false;
                    LineTrafficControlEventHandler($"\r\n     Server Connect Error {GetTimeStamp()}\r\n", "Receive");
                }
                //if (IsloggerON)
                //_logService.LogMessage(_logBox, $"Connected at IP: {ip} at Port: {port}",Color.Green);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.Message.ToString()}");
                LineTrafficControlEventHandler($"\r\n     Server Connect Error: {ex.Message.ToString()} {GetTimeStamp()}\r\n", "Receive");
                //Log($"Error: {ex.Message}", Color.Red);
            }
            return IsConnected;
        }

        /// <summary>
        /// This Initialize the GXNet connection object in Async Mode
        /// </summary>
        /// <returns>Return True if It get connected.</returns>
        public async Task<bool> ConnectAsync()
        {
            LineTrafficControlEventHandler($"\r\n     Server Connect Request", "Send");
            bool IsConnected = false;
            try
            {
                string ip = WrapperInfo.hostName;
                int port = WrapperInfo.port;
                connection = new GXNet(NetworkType.Tcp, ip, port);
                connection.UseIPv6 = true;
                connection.Open();
                SetGetFromMeter.Wait(1000);
                if (connection.IsOpen)
                {
                    IsConnected = true;
                    LineTrafficControlEventHandler($"\r\n     Server Connected Successfully to {ip}  {port} {GetTimeStamp()}\r\n", "Send");
                }
                else
                {
                    IsConnected = false;
                    LineTrafficControlEventHandler($"\r\n     Server Connect Error {GetTimeStamp()}\r\n", "Receive");
                }
                //if (IsloggerON)
                //_logService.LogMessage(_logBox, $"Connected at IP: {ip} at Port: {port}",Color.Green);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.Message.ToString()}");
                LineTrafficControlEventHandler($"\r\n     Server Connect Error: {ex.Message.ToString()}\r\n", "Receive");
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
                connection.Dispose();
                //connection.DisconnectClient(WrapperInfo.hostName);
                //connection?.Close();
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
        /// <summary>
        /// Close connection to the meter by sending Release and Disconnect Request.
        /// </summary>
        public void Close()
        {
            LineTrafficControlEventHandler($"\r\n     DISCONNECT Request\r\n", "Send");
            if (connection != null && dlmsClient != null)
            {
                try
                {
                    if (Trace > TraceLevel.Info)
                    {
                        log.Info("Disconnecting from the meter.");
                    }
                    try
                    {
                        if (connection.IsOpen)
                            Release();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message.ToString());
                        //LineTrafficControlEventHandler($"\r\n     DISCONNECT FAILED Error: {ex.Message.ToString()}\r\n", "Receive");
                    }
                    GXReplyData reply = new GXReplyData();
                    if (connection.IsOpen)
                    {
                        ReadDLMSPacket(dlmsClient.DisconnectRequest(), reply);
                        LineTrafficControlEventHandler($"     DISCONNECT Success\r\n", "Send");
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message.ToString());
                    LineTrafficControlEventHandler($"     DISCONNECT FAILED Error: {ex.Message.ToString()}\r\n", "Receive");
                }
                //if (connection.IsOpen)
                //    connection.Close();
                //connection = null;
                //dlmsClient = null;
            }
        }
        /// <summary>
        /// Release.
        /// </summary>
        /// 
        public void Release()
        {
            LineTrafficControlEventHandler($"\r\n     RESELASE Request", "Send");
            if (connection != null && dlmsClient != null)
            {
                try
                {
                    if (Trace > TraceLevel.Info)
                    {
                        Console.WriteLine("Release from the meter.");
                    }
                    //Release is call only for secured connections.
                    //All meters are not supporting Release and it's causing problems.
                    if (dlmsClient.InterfaceType == InterfaceType.WRAPPER ||
                        (dlmsClient.Ciphering.Security != (byte)Security.None &&
                        !dlmsClient.PreEstablishedConnection))
                    {
                        GXReplyData reply = new GXReplyData();
                        ReadDataBlock(dlmsClient.ReleaseRequest(), reply);
                    }
                    LineTrafficControlEventHandler($"     RESELASE Success\r\n", "Send");
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message.ToString());
                    //All meters don't support Release.
                    Console.WriteLine("Release failed. " + ex.Message.ToString());
                    LineTrafficControlEventHandler($"     RESELASE FAILED Error: {ex.Message.ToString()}\r\n", "Receive");
                }
            }
        }
        /// <summary>
        /// Release Async.
        /// </summary>
        /// 
        public async Task<bool> ReleaseAsync()
        {
            LineTrafficControlEventHandler($"\r\n     RESELASE Request", "Send");
            bool IsRRPassed = false;
            if (connection != null && dlmsClient != null)
            {
                try
                {
                    if (Trace > TraceLevel.Info)
                    {
                        Console.WriteLine("Release from the meter.");
                    }
                    //Release is call only for secured connections.
                    //All meters are not supporting Release and it's causing problems.
                    if (dlmsClient.InterfaceType == InterfaceType.WRAPPER ||
                        (dlmsClient.Ciphering.Security != (byte)Security.None &&
                        !dlmsClient.PreEstablishedConnection))
                    {
                        GXReplyData reply = new GXReplyData();
                        ReadDataBlock(dlmsClient.ReleaseRequest(), reply);
                    }
                    IsRRPassed = true;
                    LineTrafficControlEventHandler($"     RESELASE Success\r\n", "Send");
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message.ToString());
                    //All meters don't support Release.
                    Console.WriteLine("Release failed. " + ex.Message.ToString());
                    LineTrafficControlEventHandler($"     RESELASE FAILED Error: {ex.Message.ToString()}\r\n", "Receive");
                }
            }
            return IsRRPassed;
        }
        #endregion

        #region DLMS Methods

        /// <summary>
        /// Send AARQ Request to the meter.
        /// </summary>
        public bool AARQRequest()
        {
            LineTrafficControlEventHandler($"\r\n     AARQ Request {GetTimeStamp()}", "Send");
            bool IsAARQPassed = false;
            try
            {
                ReadDataBlock(dlmsClient.AARQRequest(), reply);
                //Log("AARQ Response " + reply.ToString(), Color.Green);
                try
                {
                    dlmsClient.ParseAAREResponse(reply.Data);
                    reply.Clear();
                }
                catch (Exception ex)
                {
                    reply.Clear();
                    log.Error($"{ex.Message.ToString()}");
                    IsAARQPassed = false;
                    LineTrafficControlEventHandler($"\r\n     AARQ Error: {ex.Message.ToString()} {GetTimeStamp()}\r\n", "Receive");
                    //Log(ex.Message.ToString(), Color.Red);
                }
                SetGetFromMeter.Wait(500);
                ReadDataBlock(dlmsClient.GetApplicationAssociationRequest(), reply);
                dlmsClient.ParseApplicationAssociationResponse(reply.Data);
                if (reply.Error == 0)
                {
                    IsAARQPassed = true;
                    LineTrafficControlEventHandler($"\r\n     AARQ Success {GetTimeStamp()}\r\n", "Send");
                }
                else
                {
                    IsAARQPassed = false;
                    LineTrafficControlEventHandler($"\r\n     AARQ Error: {reply.ErrorMessage} {GetTimeStamp()}\r\n", "Receive");
                }
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message.ToString()}");
                IsAARQPassed = false;
                LineTrafficControlEventHandler($"\r\n     AARQ Error: {ex.Message.ToString()} {GetTimeStamp()}\r\n", "Receive");
            }
            return IsAARQPassed;
        }

        /// <summary>
        /// Send AARQ Request to the meter in Async Mode.
        /// </summary>
        public async Task<bool> AARQRequestAsync()
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
                    reply.Clear();
                }
                catch (Exception ex)
                {
                    reply.Clear();
                    log.Error($"{ex.Message.ToString()}");
                    IsAARQPassed = false;
                    LineTrafficControlEventHandler($"\r\n     AARQ Error: {ex.Message.ToString()} {GetTimeStamp()}\r\n", "Receive");
                    //Log(ex.Message.ToString(), Color.Red);
                }
                SetGetFromMeter.Wait(500);
                ReadDataBlock(dlmsClient.GetApplicationAssociationRequest(), reply);
                dlmsClient.ParseApplicationAssociationResponse(reply.Data);
                if (reply.Error == 0)
                {
                    IsAARQPassed = true;
                    LineTrafficControlEventHandler($"\r\n     AARQ Success {GetTimeStamp()}\r\n", "Send");
                }
                else
                {
                    IsAARQPassed = false;
                    LineTrafficControlEventHandler($"\r\n     AARQ Error: {reply.ErrorMessage} {GetTimeStamp()}\r\n", "Receive");
                }
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message.ToString()}");
                IsAARQPassed = false;
                LineTrafficControlEventHandler($"\r\n     AARQ Error: {ex.Message.ToString()}\r\n", "Receive");
            }
            return IsAARQPassed;
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
                System.Windows.Forms.Application.DoEvents();
                //Log($"\r\nSend Command:\r\n{GXCommon.ToHex(it, true)}", Color.Green);
                //LineTrafficControlEventHandler($"\r\nSend Command:\r\n{GXCommon.ToHex(it, true)}", "Send");
                reply.Clear();
                ReadDataBlock(it, reply);
                //Thread.Sleep(100);
                SetGetFromMeter.Wait(10);
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
                    System.Windows.Forms.Application.DoEvents();
                    if (!reply.IsStreaming())
                    {
                        WriteTrace("TX:\t" + GXCommon.ToHex(data, true));
                        LineTrafficControlEventHandler($"\r\n(S)  {GXCommon.ToHex(data, true)} {GetTimeStamp()}", "Send");
                        LineTrafficControlEventHandler($"\r\n(XML)\r\n{DecodePushDataToXML(data)} {GetTimeStamp()}\r\n", "Send");
                        p.Reply = null;
                        connection.Send(data, null);
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
                        System.Windows.Forms.Application.DoEvents();
                        p.Reply = null;
                        if (notify.IsComplete && notify.Data.Data != null)
                        {
                            //Handle notify.
                            if (!notify.IsMoreData)
                            {
                                if (notify.PrimeDc != null)
                                {
                                    OnNotification?.Invoke(notify.PrimeDc);
                                    //Console.WriteLine(notify.PrimeDc);
                                }
                                else
                                {
                                    //Show received push message as XML.
                                    string xml;
                                    GXDLMSTranslator t = new GXDLMSTranslator(TranslatorOutputType.SimpleXml);
                                    t.DataToXml(notify.Data, out xml);
                                    OnNotification?.Invoke(xml);
                                    //Console.WriteLine(xml);
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
                            System.Windows.Forms.Application.DoEvents();
                            if (++pos >= RetryCount)
                            {
                                LineTrafficControlEventHandler("\r\nFailed to receive reply from the device in given time.\r\n", "Receive");
                                throw new Exception("Failed to receive reply from the device in given time.");
                            }
                            p.Reply = null;
                            connection.Send(data, null);
                            //Try to read again...
                            LineTrafficControlEventHandler("\r\nData send failed. Try to resend" + pos.ToString() + "/3" + "\r\n", "Receive");
                            //System.Diagnostics.Debug.WriteLine("Data send failed. Try to resend " + pos.ToString() + "/3");
                        }
                        rd.Set(p.Reply);
                    }
                    //Log($"Reply: {GXCommon.ToHex(reply.Data.Data, true)}", Color.Magenta);
                }
                catch (Exception ex)
                {
                    WriteTrace("RX:\t" + rd);
                    throw ex;
                }
            }
            //Log($"Encrypted Response: {GXCommon.ToHex(rd.Data, true)}", Color.Black);
            //string decodedXml = DecodePushDataToXML(rd.Data);
            //Log($"XML:\r\n{decodedXml}", Color.DarkViolet);
            //string decryptData = GetDecryptData(decodedXml);
            //Log($"DECRYPT DATA:\r\n{decryptData}", Color.DarkViolet);
            WriteTrace("RX:\t" + rd);
            LineTrafficControlEventHandler($"\r\n(R)  {rd} {GetTimeStamp()}", "Receive");
            LineTrafficControlEventHandler($"\r\n(XML)\r\n{DecodePushDataToXML(rd.Data)} {GetTimeStamp()}\r\n", "Receive");
            if (reply.Error != 0)
            {
                if (reply.Error == (short)ErrorCode.Rejected)
                {
                    //Thread.Sleep(1000);
                    SetGetFromMeter.Wait(100);
                    ReadDLMSPacket(data, reply);
                }
                else
                {
                    throw new GXDLMSException(reply.Error);
                }
            }
        }
        public bool Write(GXDLMSObject obj, int index, object writeValue = null)
        {
            bool IsSuccess = false;
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
                            byte[] testbb = new byte[response.Length];
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
                            if (reply.Error == 0)
                                IsSuccess = true;
                            //ValueEventArgs e1 = new ValueEventArgs(obj, it, 0, null);
                            //string xml = GXDLMSTranslator.ValueToXml(((IGXDLMSBase)obj).GetValue(dlmsClient.Settings, e1));
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
            return IsSuccess;
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
                    //Thread.Sleep(1000);
                    SetGetFromMeter.Wait(10);
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
                //Console.WriteLine("Can't read " + it.ToString() + ". Not enought acccess rights.");
            }
            return null;
        }

        /// <summary>
        /// Method attribute value.
        /// </summary>
        public bool Method(GXDLMSObject it, int attributeIndex, object value)
        {
            bool IsSuccess = false;
            GXReplyData reply = new GXReplyData();
            ReadDataBlock(dlmsClient.Method(it, attributeIndex, value, GXDLMSConverter.GetDLMSDataType(value)), reply);
            if (reply.Error == 0)
                IsSuccess = true;
            return IsSuccess;
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
            try
            {
                if (dlmsClient.CanRead(createObjToRead, attributeIndex))
                {
                    GXReplyData reply = new GXReplyData();
                    if (!ReadDataBlock(dlmsClient.Read(createObjToRead, attributeIndex), reply))
                    {
                        if (reply.Error != (short)ErrorCode.Rejected)
                        {
                            log.Error(reply.Error.ToString());
                            //throw new GXDLMSException(reply.Error);
                        }
                        reply.Clear();
                        //Thread.Sleep(1000);
                        SetGetFromMeter.Wait(10);
                        if (!ReadDataBlock(dlmsClient.Read(createObjToRead, attributeIndex), reply))
                        {
                            log.Error(reply.Error.ToString());
                            //throw new GXDLMSException(reply.Error);
                        }
                    }
                    if (createObjToRead.GetDataType(attributeIndex) == DataType.None)
                    {
                        createObjToRead.SetDataType(attributeIndex, reply.DataType);
                    }
                    recData = reply.ToString().Replace(" ", "");
                    LineTrafficControlEventHandler($"Received Data:\r\n{reply.ToString()}\r\n", "Receive");
                    return dlmsClient.UpdateValue(createObjToRead, attributeIndex, reply.Value);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                log.Error(ex.StackTrace.ToString());
            }
            return null;
        }

        /// <summary>
        /// This will perform read operation as task
        /// </summary>
        /// <param name="objtype"></param>
        /// <param name="obis"></param>
        /// <param name="attribute"></param>
        public void StartReadOperationAsTask(ObjectType objtype, string obis, int attribute)
        {
            System.Threading.Tasks.Task.Run(() =>
            ReadCOSEMObject(objtype, obis, attribute));
        }

        /// <summary>
        /// Read all data from the meter.
        /// </summary>
        public void ReadAll(string outputFile)
        {
            try
            {
                if (GetAssociationView(outputFile))
                {
                    GetScalersAndUnits();
                    GetProfileGenericColumns();
                }
                GetCompactData();
                GetReadOut();
                GetProfileGenerics();
                if (outputFile != null)
                {
                    try
                    {
                        dlmsClient.Objects.Save(outputFile, new GXXmlWriterSettings() { UseMeterTime = true, IgnoreDefaultValues = false });
                    }
                    catch (Exception)
                    {
                        //It's OK if this fails.
                    }
                }
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// This Takes Object Type (Class), lnName (OBIS) and attribute index of object. DataType of the write Data and sData is Data to write
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="lnName"></param>
        /// <param name="attributeIndex"></param>
        /// <param name="dataType"></param>
        /// <param name="sData"></param>
        public bool WriteData(ObjectType objType, string lnName, int attributeIndex, DataType dataType, object sData)
        {
            LineTrafficControlEventHandler($"\r\n     SET CLASS-{(int)objType} | OBIS-{lnName} [{DLMSParser.GetObisName(((int)objType).ToString(), lnName, ((int)attributeIndex).ToString())}] | Attribute-{attributeIndex}\r\n", "Send");
            return Write(CreateObjectForWrite(objType, lnName, attributeIndex, dataType, AccessMode.ReadWrite), attributeIndex, sData);
        }
        /// <summary>
        /// Set the 0.0.25.136.0.255 NIC Configuration. Takes the HEX data string which needs to be set.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string SetNICConfigData(string Value)
        {
            string resultMessage = "";
            try
            {
                GXReplyData reply = new GXReplyData();
                GXDLMSData gxdlmsData = new GXDLMSData();
                gxdlmsData.LogicalName = "0.0.25.136.0.255";
                gxdlmsData.SetDataType(2, DataType.Array);
                gxdlmsData.Value = (object)GXCommon.HexToBytes(Value);
                foreach (byte[] data in dlmsClient.Write((GXDLMSObject)gxdlmsData, 2))
                {
                    reply.Clear();
                    SetGetFromMeter.Wait(500);
                    ReadDataBlock(data, reply);
                }
                recData = reply.ToString().Replace(" ", "");
                if (reply.Error == 0)
                {
                    resultMessage = "Set Successfully";
                }
                //if (reply.ErrorMessage != "")
                //{
                //    reply.Clear();
                //}
                //else
                //{
                //    resultMessage = "Set Successfully";
                //}
                reply.Clear();
            }
            catch (Exception ex)
            {
                reply.Clear();
                resultMessage = ex.Message.ToString() + $" | {reply.ErrorMessage}";
                log.Error(ex.Message.ToString());
            }
            return resultMessage;
        }

        /// <summary>
        /// Write list of attributes.
        /// </summary>
        public void WriteList(List<KeyValuePair<GXDLMSObject, int>> list)
        {
            byte[][] data = dlmsClient.WriteList(list);
            GXReplyData reply = new GXReplyData();
            foreach (byte[] it in data)
            {
                ReadDataBlock(it, reply);
                reply.Clear();
            }
        }

        /// <summary>
        /// This method is used to update meter firmware.
        /// </summary>
        /// <param name="target">Image transfer object.</param>
        /// <param name="identification">Image identification.</param>
        /// <param name="image">Updated image.</param>
        public void ImageUpdate(GXDLMSImageTransfer target, byte[] identification, byte[] image)
        {
            dlmsClient.Objects.Add(target);
            GetReadOut();
            //Check that image transfer is enabled.
            GXReplyData reply = new GXReplyData();
            ReadDataBlock(dlmsClient.Read(target, 5), reply);
            dlmsClient.UpdateValue(target, 5, reply.Value);
            if (!target.ImageTransferEnabled)
            {
                throw new Exception("Image transfer is not enabled");
            }

            //Step 1: Read image block size.
            ReadDataBlock(dlmsClient.Read(target, 2), reply);
            dlmsClient.UpdateValue(target, 2, reply.Value);

            // Step 2: Initiate the Image transfer process.
            ReadDataBlock(target.ImageTransferInitiate(dlmsClient, identification, image.Length), reply);

            // Step 3: Transfers ImageBlocks.
            int imageBlockCount;
            ReadDataBlock(target.ImageBlockTransfer(dlmsClient, image, out imageBlockCount), reply);

            //Step 4: Check the completeness of the Image.
            ReadDataBlock(dlmsClient.Read(target, 3), reply);
            dlmsClient.UpdateValue(target, 3, reply.Value);

            // Step 5: The Image is verified;
            ReadDataBlock(target.ImageVerify(dlmsClient), reply);
            // Step 6: Before activation, the Image is checked;

            //Get list to images to activate.
            ReadDataBlock(dlmsClient.Read(target, 7), reply);
            dlmsClient.UpdateValue(target, 7, reply.Value);
            bool bFound = false;
            foreach (GXDLMSImageActivateInfo it in target.ImageActivateInfo)
            {
                if (GXCommon.EqualBytes(it.Identification, identification))
                {
                    bFound = true;
                    break;
                }
            }

            //Read image transfer status.
            ReadDataBlock(dlmsClient.Read(target, 6), reply);
            dlmsClient.UpdateValue(target, 6, reply.Value);
            if (target.ImageTransferStatus != Gurux.DLMS.Objects.Enums.ImageTransferStatus.VerificationSuccessful)
            {
                throw new Exception("Image transfer status is " + target.ImageTransferStatus.ToString());
            }

            if (!bFound)
            {
                throw new Exception("Image not found.");
            }

            //Step 7: Activate image.
            ReadDataBlock(target.ImageActivate(dlmsClient), reply);
        }

        /// <summary>
        /// This Takes Object Type (Class), lnName (OBIS) and attribute index of object. DataType of the write Data and sData is Data to write
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="lnName"></param>
        /// <param name="attributeIndex"></param>
        /// <param name="dataType"></param>
        /// <param name="sData"></param>
        public bool Action(ObjectType objType, string lnName, int attributeIndex, DataType dataType, object sData)
        {
            LineTrafficControlEventHandler($"\r\n     ACTION CLASS-{(int)objType} | OBIS-{lnName} [{DLMSParser.GetObisName(((int)objType).ToString(), lnName, ((int)attributeIndex).ToString())}] | Attribute-{attributeIndex}", "Send");
            return Method(CreateObjectForWrite(objType, lnName, attributeIndex, dataType, AccessMode.ReadWrite), attributeIndex, sData);
        }

        /// <summary>
        /// Read association view.
        /// </summary>
        public bool GetAssociationView(string outputFile)
        {
            LineTrafficControlEventHandler($"\r\n     GET Association", "Send");
            if (outputFile != null)
            {
                //Save Association view to the cache so it is not needed to retrieve every time.
                if (File.Exists(outputFile))
                {
                    try
                    {
                        dlmsClient.Objects.Clear();
                        dlmsClient.Objects.AddRange(GXDLMSObjectCollection.Load(outputFile));
                        return false;
                    }
                    catch (Exception)
                    {
                        if (File.Exists(outputFile))
                        {
                            File.Delete(outputFile);
                        }
                    }
                }
            }
            GXReplyData reply = new GXReplyData();
            ReadDataBlock(dlmsClient.GetObjectsRequest(), reply);
            //Log("Association Response " + reply.ToString(), Color.Green);
            dlmsClient.ParseObjects(reply.Data, true);
            //Access rights must read differently when short Name referencing is used.
            if (!dlmsClient.UseLogicalNameReferencing)
            {
                GXDLMSAssociationShortName sn = (GXDLMSAssociationShortName)dlmsClient.Objects.FindBySN(0xFA00);
                if (sn != null && sn.Version > 0)
                {
                    Read(sn, 3);
                }
            }
            if (outputFile != null)
            {
                try
                {
                    dlmsClient.Objects.Save(outputFile, new GXXmlWriterSettings() { Values = false });
                }
                catch (Exception)
                {
                    //It's OK if this fails.
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Read all objects from the meter.
        /// </summary>
        /// <remarks>
        /// It's not normal to read all data from the meter. This is just an example.
        /// </remarks>
        public void GetReadOut()
        {
            foreach (GXDLMSObject it in dlmsClient.Objects)
            {
                // Profile generics are read later because they are special cases.
                // (There might be so lots of data and we so not want waste time to read all the data.)
                if (it is GXDLMSProfileGeneric)
                {
                    continue;
                }
                if (!(it is IGXDLMSBase))
                {
                    //If interface is not implemented.
                    //Example manufacturer spesific interface.
                    if (Trace > TraceLevel.Error)
                    {
                        //Console.WriteLine("Unknown Interface: " + it.ObjectType.ToString());
                    }
                    continue;
                }
                if (Trace > TraceLevel.Warning)
                {
                    //Console.WriteLine("-------- Reading " + it.GetType().Name + " " + it.Name + " " + it.Description);
                }
                //Log("-------- Reading " + it.GetType().Name + " " + it.Name + " " + it.Description, Color.Black);
                foreach (int pos in (it as IGXDLMSBase).GetAttributeIndexToRead(true))
                {
                    try
                    {
                        if (dlmsClient.CanRead(it, pos))
                        {
                            object val = Read(it, pos);
                            ShowValue(val, pos);
                        }
                        else
                        {
                            //Console.WriteLine("Info! " + it.GetType().Name + " " + it.Name + "Index: " + pos + " is not readable.");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message.ToString());
                        //Console.WriteLine("Error! " + it.GetType().Name + " " + it.Name + "Index: " + pos + " " + ex.Message.ToString());
                        //Console.WriteLine(ex.ToString());
                    }
                }
            }
        }

        public Dictionary<string, object> GetSingleEntryProfileDataTable(string profileObis, string scalerObis)
        {
            string recivedObisString = "", recivedValueString = "", recivedScalerObisString = "", recivedScalerValueString = "";
            DataTable obisDataTable = new DataTable();
            DataTable resultDataTable = new DataTable();
            string[] scalerObisArray = null, scalerScalerDataArray = null, mainSourceObisArray = null, finalScalerValuesToFill = null, ScalerMultiFactorArray = null;
            bool result = false;
            try
            {
                SetGetFromMeter.Wait(500);
                //Get Profile Objects
                ReadCOSEMObject(ObjectType.ProfileGeneric, profileObis, 3);
                recivedObisString = recData;
                SetGetFromMeter.Wait(500);
                //Get Profile Values
                ReadCOSEMObject(ObjectType.ProfileGeneric, profileObis, 2);
                recivedValueString = recData;
                SetGetFromMeter.Wait(500);
                //Get Scaler Objects
                ReadCOSEMObject(ObjectType.ProfileGeneric, scalerObis, 3);
                recivedScalerObisString = recData;
                SetGetFromMeter.Wait(500);
                //Get Scaler Values
                ReadCOSEMObject(ObjectType.ProfileGeneric, scalerObis, 2);
                recivedScalerValueString = recData;


                obisDataTable = parse.GetParameterTableHorizontal(recivedObisString, profileObis);
                scalerObisArray = parse.GetScalerProfileObisListParsing(recivedScalerObisString);
                scalerScalerDataArray = parse.GetScalerProfileScalerListParsing(recivedScalerValueString);
                mainSourceObisArray = obisDataTable.AsEnumerable()
                                         .Select(row => row.Field<string>(3)) // Change the type to match the third column's type
                                         .ToArray();
                finalScalerValuesToFill = new string[mainSourceObisArray.Length];
                ScalerMultiFactorArray = new string[mainSourceObisArray.Length];
                foreach (var selectedObis in scalerObisArray)
                {
                    // Find the index of the searchString in the stringArray
                    int index = Array.IndexOf(mainSourceObisArray, selectedObis);
                    int scalarIndex = Array.IndexOf(scalerObisArray, selectedObis);
                    if (index != -1)
                    {
                        if (index > mainSourceObisArray.Length)
                            break;
                        else
                        {
                            finalScalerValuesToFill[index] = scalerScalerDataArray[scalarIndex];
                            //dataGridView.Rows[index].Cells[5].Value = scalerScalerDataArray[scalarIndex];
                            obisDataTable.Rows[index][5] = scalerScalerDataArray[scalarIndex];
                        }
                    }
                }
                resultDataTable = parse.GetValuesDataTableParsing(recivedValueString, profileObis, obisDataTable);
                // Multiply each row value with the corresponding values in "Data" columns
                RenameParameterWithUnit(resultDataTable);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                // Create a dictionary to hold the DataTable and strings
                return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "ProfileObis", recivedObisString},
                            { "ProfileValues", recivedValueString },
                            { "ScalerObis", recivedScalerObisString },
                            { "ScalerValues", recivedScalerValueString }
                        };
            }
            // Create a dictionary to hold the DataTable and strings
            return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "ProfileObis", recivedObisString},
                            { "ProfileValues", recivedValueString },
                            { "ScalerObis", recivedScalerObisString },
                            { "ScalerValues", recivedScalerValueString }
                        };
        }

        public Dictionary<string, object> GetProfileGenericDownloadByOptions(WrapperParser.DownloadOption option)
        {
            Dictionary<string, object> downloadedList = new Dictionary<string, object>();
            staticDownloadedList.Clear();
            List<string> obisList = parse.GetProfileDownloadOBISList(option);
            if (obisList == null || obisList.Count == 0)
                return downloadedList;
            try
            {
                foreach (string obis in obisList)
                {
                    if (stopReadingAllData)
                    {
                        Disconnect();
                        break;
                    }
                    SetGetFromMeter.Wait(500);
                    ReadCOSEMObject(ObjectType.ProfileGeneric, obis, 3);
                    downloadedList.Add($"{obis}|3|{DLMSParser.GetObisName("7", obis, "3")}", recData);
                    staticDownloadedList.Add($"{obis}|3|{DLMSParser.GetObisName("7", obis, "3")}", recData);
                    SetGetFromMeter.Wait(500);
                    ReadCOSEMObject(ObjectType.ProfileGeneric, obis, 2);
                    downloadedList.Add($"{obis}|2|{DLMSParser.GetObisName("7", obis, "2")}", recData);
                    staticDownloadedList.Add($"{obis}|2|{DLMSParser.GetObisName("7", obis, "2")}", recData);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                log.Error(ex.StackTrace.ToString());
            }
            return downloadedList;
        }

        public Dictionary<string, object> GetProfileGenericDownloadByOptionsAsTask(WrapperParser.DownloadOption option)
        {
            Dictionary<string, object> tableNameplate = null;
            System.Threading.Tasks.Task<Dictionary<string, object>> task = System.Threading.Tasks.Task.Run(() => GetProfileGenericDownloadByOptions(option));
            // Wait for it to complete and get the result
            TestStopWatch testStopWatch = new TestStopWatch();
            testStopWatch.Start();
            while (!task.IsCompleted)
            {
                SetGetFromMeter.Wait(100);
                if (testStopWatch.GetElapsedSeconds() > (15 * 60))
                {
                    tableNameplate = new Dictionary<string, object>();
                    task.Dispose();
                    return tableNameplate;
                }
            }
            tableNameplate = task.Result;
            task.Dispose();
            return tableNameplate;
        }



        /// <summary>
        /// Methods to GET EVENTS RELATED and LS DE DATA TABLE
        /// </summary>
        /// <param name="profileObis"></param>
        /// <param name="scalerObis"></param>
        /// <param name="_startIndex"></param>
        /// <param name="_endIndex"></param>
        /// <param name="nType"></param>
        /// <param name="_startDT"></param>
        /// <param name="_endDT"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetProfileDataTable(string profileObis, string scalerObis)
        {
            DLMSParser DLMSparse = new DLMSParser();
            string recivedObisString = "", recivedValueString = "", recivedScalerObisString = "", recivedScalerValueString = "", inUseEntries = "", profileEntries = "", option = "";
            DataTable obisDataTable = new DataTable();
            DataTable resultDataTable = new DataTable();
            string[] scalerObisArray = null, scalerScalerDataArray = null, mainSourceObisArray = null, finalScalerValuesToFill = null, ScalerMultiFactorArray = null;
            try
            {
                SetGetFromMeter.Wait(500);
                //Get Profile Objects
                ReadCOSEMObject(ObjectType.ProfileGeneric, profileObis, 3);
                recivedObisString = recData;
                SetGetFromMeter.Wait(500);
                //Get Profile Values
                ReadCOSEMObject(ObjectType.ProfileGeneric, profileObis, 2);
                recivedValueString = recData;
                SetGetFromMeter.Wait(500);
                //Get Scaler Objects
                ReadCOSEMObject(ObjectType.ProfileGeneric, scalerObis, 3);
                recivedScalerObisString = recData;
                //Get Scaler Values
                ReadCOSEMObject(ObjectType.ProfileGeneric, scalerObis, 2);
                recivedScalerValueString = recData;

                //check weather the object list table is created.
                if (!parse.GetObjectsDataTable(recivedObisString, ref obisDataTable))
                {
                    // Create a dictionary to hold the DataTable and strings
                    return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "ProfileObis", recivedObisString},
                            { "ProfileValues", recivedValueString },
                            { "ScalerObis", recivedScalerObisString },
                            { "ScalerValues", recivedScalerValueString },
                            { "InUseEntries", inUseEntries },
                            { "ProfileEntries", profileEntries }
                        };
                }

                #region Append and find the Scaler Units and Scaler Factor
                scalerObisArray = parse.GetScalerProfileObisListParsing(recivedScalerObisString);
                scalerScalerDataArray = parse.GetScalerProfileScalerListParsing(recivedScalerValueString);
                mainSourceObisArray = new string[obisDataTable.Columns.Count];
                for (int i = 0; i < obisDataTable.Columns.Count; i++)
                {
                    mainSourceObisArray[i] = obisDataTable.Columns[i].ColumnName.Split('-')[1].Trim();
                }
                finalScalerValuesToFill = new string[mainSourceObisArray.Length];
                ScalerMultiFactorArray = new string[mainSourceObisArray.Length];
                foreach (var selectedObis in scalerObisArray)
                {
                    // Find the index of the searchString in the stringArray
                    int index = Array.IndexOf(mainSourceObisArray, selectedObis);
                    int scalarIndex = Array.IndexOf(scalerObisArray, selectedObis);
                    if (index != -1)
                    {
                        finalScalerValuesToFill[index] = scalerScalerDataArray[scalarIndex];
                        ScalerMultiFactorArray[index] = (string)DLMSparse.ScalerhshTable[scalerScalerDataArray[scalarIndex].Substring(2, 2)];
                        obisDataTable.Columns[index].ColumnName = (!string.IsNullOrEmpty((string)DLMSparse.UnithshTable[scalerScalerDataArray[scalarIndex].Substring(6, 2)])) ?
                                                                $"{obisDataTable.Columns[index].ColumnName} - {scalerScalerDataArray[scalarIndex]} - ({(string)DLMSparse.UnithshTable[scalerScalerDataArray[scalarIndex].Substring(6, 2)]})" :
                                                                $"{obisDataTable.Columns[index].ColumnName} - {scalerScalerDataArray[scalarIndex]}";
                    }
                }
                #endregion

                resultDataTable = parse.GetEventsValuesDataTableParsing(recivedValueString, obisDataTable, profileObis);
                // Multiply each row value with the corresponding string array value
                MultiplyRowsWithArray(resultDataTable, ScalerMultiFactorArray);

                #region Add a new column for serial numbers
                DataColumn snColumn = new DataColumn("SN", typeof(string));
                resultDataTable.Columns.Add(snColumn);
                // Set the new column to be the first column
                snColumn.SetOrdinal(0);
                // Populate the SN column with row numbers
                for (int i = 0; i < resultDataTable.Rows.Count; i++)
                {
                    resultDataTable.Rows[i]["SN"] = i + 1;
                }
                resultDataTable.AcceptChanges();
                #endregion
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                // Create a dictionary to hold the DataTable and strings
                return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "ProfileObis", recivedObisString},
                            { "ProfileValues", recivedValueString },
                            { "ScalerObis", recivedScalerObisString },
                            { "ScalerValues", recivedScalerValueString },
                            { "InUseEntries", inUseEntries },
                            { "ProfileEntries", profileEntries }
                        };
            }
            // Create a dictionary to hold the DataTable and strings
            return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "ProfileObis", recivedObisString},
                            { "ProfileValues", recivedValueString },
                            { "ScalerObis", recivedScalerObisString },
                            { "ScalerValues", recivedScalerValueString },
                            { "InUseEntries", inUseEntries },
                            { "ProfileEntries", profileEntries }
                        };

        }



        /// <summary>
        /// To Get Nameplate Profile
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetNameplateProfileDataTable()
        {
            DLMSParser DLMSparse = new DLMSParser();
            string recivedObisString = "", recivedValueString = "", recivedScalerObisString = "", recivedScalerValueString = "", profileObis = "", scalerObis = "", option = "";
            DataTable obisDataTable = new DataTable();
            // Add columns to the DataTable
            obisDataTable.Columns.Add("Obis", typeof(string));
            obisDataTable.Columns.Add("Name", typeof(string));
            obisDataTable.Columns.Add("Data", typeof(string));
            DataTable resultDataTable = new DataTable();
            string[] scalerObisArray = null, scalerScalerDataArray = null, mainSourceObisArray = null, finalScalerValuesToFill = null, ScalerMultiFactorArray = null;
            bool result = false;
            try
            {
                profileObis = "0.0.94.91.10.255";
                //scalerObis = string.Concat("1.0.94.91.3.255".Trim().Split('.').Select(part => int.Parse(part).ToString("X2")));
                #region Data Getting Logic
                SetGetFromMeter.Wait(500);
                //Get Profile Objects
                ReadCOSEMObject(ObjectType.ProfileGeneric, profileObis, 3);
                recivedObisString = recData;
                SetGetFromMeter.Wait(500);
                //Get Profile Values
                ReadCOSEMObject(ObjectType.ProfileGeneric, profileObis, 2);
                recivedValueString = recData;
                obisDataTable = parse.GetNameplateClassObisAttTable(recivedObisString, profileObis);
                #region Add a new column for serial numbers
                //DataColumn snColumn = new DataColumn("SN", typeof(string));
                //obisDataTable.Columns.Add(snColumn);
                //// Set the new column to be the first column
                //snColumn.SetOrdinal(0);
                //// Populate the SN column with row numbers
                //for (int i = 0; i < obisDataTable.Rows.Count; i++)
                //{
                //    obisDataTable.Rows[i]["SN"] = i + 1;
                //}
                //obisDataTable.AcceptChanges();
                #endregion

                resultDataTable = parse.GetNameplateValuesDataTable(recivedValueString, obisDataTable);

                #endregion
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                // Create a dictionary to hold the DataTable and strings
                return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "ProfileObis", recivedObisString},
                            { "ProfileValues", recivedValueString }
                        };
            }

            // Create a dictionary to hold the DataTable and strings
            return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "ProfileObis", recivedObisString},
                            { "ProfileValues", recivedValueString }
                        };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="associationOBIS"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetAssociationDataTable(string associationOBIS = "0.0.40.0.0.255")
        {
            DLMSParser DLMSparse = new DLMSParser();
            string recivedObjectString = "";
            DataTable obisDataTable = new DataTable();
            DataTable resultDataTable = new DataTable();
            try
            {
                SetGetFromMeter.Wait(500);
                //Get Profile Objects
                ReadCOSEMObject(ObjectType.AssociationLogicalName, associationOBIS, 2);
                recivedObjectString = recData;
                //resultDataTable = parse.GetObjectList(recivedObjectString);
                int obisCount = 0;
                resultDataTable = DLMSAssociationLN.GetObjectListTable(recivedObjectString, DLMSAssociationLN.AssociationType.Current_Association, out obisCount);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                // Create a dictionary to hold the DataTable and strings
                return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "Data", recivedObjectString}
                        };
            }
            // Create a dictionary to hold the DataTable and strings
            return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "Data", recivedObjectString}
                        };
        }

        static void MultiplyRowsWithArray(DataTable dataTable, string[] scalerMultiFactorArray)
        {
            double n;
            // Iterate through each row of the DataTable
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                // Iterate through each cell in the row
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    // Check if the string array value at the same index is not empty or null
                    if (!string.IsNullOrEmpty(scalerMultiFactorArray[j]))
                    {
                        if (double.TryParse(dataTable.Rows[i][j].ToString(), out n))
                        {
                            // Multiply the cell value with the corresponding string array value
                            double cellValue = Convert.ToDouble(dataTable.Rows[i][j]);
                            double factor = Convert.ToDouble(scalerMultiFactorArray[j]);
                            dataTable.Rows[i][j] = (cellValue * factor).ToString();
                        }
                    }
                }
            }
        }

        public void GetProfileGenerics()
        {
            //Find profile generics objects and read them.
            foreach (GXDLMSObject it in dlmsClient.Objects.GetObjects(ObjectType.ProfileGeneric))
            {
                //If trace is info.
                if (Trace > TraceLevel.Warning)
                {
                    Console.WriteLine("-------- Reading " + it.GetType().Name + " " + it.Name + " " + it.Description);
                }
                long entriesInUse = -1;
                if (dlmsClient.CanRead(it, 7))
                {
                    entriesInUse = Convert.ToInt64(Read(it, 7));
                }
                long entries = -1;
                if (dlmsClient.CanRead(it, 8))
                {
                    entries = Convert.ToInt64(Read(it, 8));
                }
                //If trace is info.
                if (Trace > TraceLevel.Warning)
                {
                    Console.WriteLine("Entries: " + entriesInUse + "/" + entries);
                }
                //If there are no columns or rows.
                if (entriesInUse == 0 || (it as GXDLMSProfileGeneric).CaptureObjects.Count == 0)
                {
                    continue;
                }
                //All meters are not supporting parameterized read.
                if ((dlmsClient.NegotiatedConformance & (Gurux.DLMS.Enums.Conformance.ParameterizedAccess | Gurux.DLMS.Enums.Conformance.SelectiveAccess)) != 0)
                {
                    try
                    {
                        //Read first row from Profile Generic.
                        object[] rows = ReadRowsByEntry(it as GXDLMSProfileGeneric, 1, 1);
                        //If trace is info.
                        if (Trace > TraceLevel.Warning)
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (object[] row in rows)
                            {
                                foreach (object cell in row)
                                {
                                    if (cell is byte[])
                                    {
                                        sb.Append(GXCommon.ToHex((byte[])cell, true));
                                    }
                                    else
                                    {
                                        sb.Append(Convert.ToString(cell));
                                    }
                                    sb.Append(" | ");
                                }
                                sb.Append("\r\n");
                            }
                            Console.WriteLine(sb.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message.ToString());
                        Console.WriteLine("Error! Failed to read first row: " + ex.Message.ToString());
                        //Continue reading.
                    }
                }
                //All meters are not supporting parameterized read.
                if ((dlmsClient.NegotiatedConformance & (Gurux.DLMS.Enums.Conformance.ParameterizedAccess | Gurux.DLMS.Enums.Conformance.SelectiveAccess)) != 0)
                {
                    try
                    {
                        //Read last day from Profile Generic.
                        object[] rows = ReadRowsByRange(it as GXDLMSProfileGeneric, DateTime.Now.Date, DateTime.MaxValue);
                        //If trace is info.
                        if (Trace > TraceLevel.Warning)
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (object[] row in rows)
                            {
                                foreach (object cell in row)
                                {
                                    if (cell is byte[])
                                    {
                                        sb.Append(GXCommon.ToHex((byte[])cell, true));
                                    }
                                    else
                                    {
                                        sb.Append(Convert.ToString(cell));
                                    }
                                    sb.Append(" | ");
                                }
                                sb.Append("\r\n");
                            }
                            Console.WriteLine(sb.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message.ToString());
                        Console.WriteLine("Error! Failed to read last day: " + ex.Message.ToString());
                        //Continue reading.
                    }
                }
            }
        }

        /// <summary>
        /// Read Profile Generic Columns by entry.
        /// </summary>
        public object[] ReadRowsByEntry(GXDLMSProfileGeneric it, UInt32 index, UInt32 count)
        {
            GXReplyData reply = new GXReplyData();
            ReadDataBlock(dlmsClient.ReadRowsByEntry(it, index, count), reply);
            return (object[])dlmsClient.UpdateValue(it, 2, reply.Value);
        }

        /// <summary>
        /// Read Profile Generic Columns by range.
        /// </summary>
        public object[] ReadRowsByRange(GXDLMSProfileGeneric it, DateTime start, DateTime end)
        {
            GXReplyData reply = new GXReplyData();
            ReadDataBlock(dlmsClient.ReadRowsByRange(it, start, end), reply);
            return (object[])dlmsClient.UpdateValue(it, 2, reply.Value);
        }

        /// <summary>
        /// Show compact data.
        /// </summary>
        public void GetCompactData()
        {
            //Find compact data objects and read them.
            foreach (GXDLMSCompactData it in dlmsClient.Objects.GetObjects(ObjectType.CompactData))
            {
                //If trace is info.
                if (Trace > TraceLevel.Warning)
                {
                    Console.WriteLine("-------- Reading " + it.GetType().Name + " " + it.Name + " " + it.Description);
                }
                //Read Capture objects.
                if (dlmsClient.CanRead(it, 3))
                {
                    Read(it, 3);
                }
                //Read template description.
                if (dlmsClient.CanRead(it, 5))
                {
                    Read(it, 5);
                }
                //Read buffer.
                if (dlmsClient.CanRead(it, 2))
                {
                    Read(it, 2);
                }
                Gurux.DLMS.Enums.Standard standard = dlmsClient.Standard;
                List<DataType> types = new List<DataType>();
                foreach (var c in it.CaptureObjects)
                {
                    types.Add(c.Key.GetUIDataType(c.Value.AttributeIndex));
                }
                List<object> rows = GXDLMSCompactData.GetData(it.TemplateDescription, it.Buffer);
                //Convert cols to readable format.
                foreach (GXStructure row in rows)
                {
                    for (int col = 0; col != types.Count; ++col)
                    {
                        if (types[col] != DataType.None)
                        {
                            row[col] = GXDLMSClient.ChangeType(row[col] as byte[], types[col]);
                        }
                        else if (row[col] is GXArray)
                        {
                            row[col] = GXDLMSTranslator.ValueToXml(row[col]);
                        }
                        else if (row[col] is GXStructure)
                        {
                            row[col] = GXDLMSTranslator.ValueToXml(row[col]);
                        }
                        else if (row[col] is byte[] b)
                        {
                            row[col] = GXDLMSTranslator.ToHex(b);
                        }
                    }
                    Console.WriteLine(row);
                }
            }
        }

        /// <summary>
        /// Read profile generic columns.
        /// </summary>
        public void GetProfileGenericColumns()
        {
            //Read Profile Generic columns first.
            foreach (GXDLMSObject it in dlmsClient.Objects.GetObjects(ObjectType.ProfileGeneric))
            {
                try
                {
                    //If info.
                    if (Trace > TraceLevel.Warning)
                    {
                        Console.WriteLine(it.LogicalName);
                    }
                    Read(it, 3);
                    //If info.
                    if (Trace > TraceLevel.Warning)
                    {
                        GXDLMSObject[] cols = (it as GXDLMSProfileGeneric).GetCaptureObject();
                        StringBuilder sb = new StringBuilder();
                        bool First = true;
                        foreach (GXDLMSObject col in cols)
                        {
                            if (!First)
                            {
                                sb.Append(" | ");
                            }
                            First = false;
                            sb.Append(col.Name);
                            sb.Append(" ");
                            sb.Append(col.Description);
                        }
                        Console.WriteLine(sb.ToString());
                    }
                }
                catch (Exception)
                {
                    //Continue reading.
                }
            }
        }

        /// <summary>
        /// Read scalers and units.
        /// </summary>
        public void GetScalersAndUnits()
        {
            GXDLMSObjectCollection objs = dlmsClient.Objects.GetObjects(new ObjectType[] { ObjectType.Register, ObjectType.ExtendedRegister, ObjectType.DemandRegister });
            //If trace is info.
            if (Trace > TraceLevel.Warning)
            {
                Console.WriteLine("Read scalers and units from the device.");
            }
            //Access services are available only for general protection.
            if ((dlmsClient.NegotiatedConformance & Conformance.Access) != 0 &&
                (dlmsClient.Ciphering.Security == Security.None ||
                (dlmsClient.NegotiatedConformance & Conformance.GeneralProtection) != 0))
            {
                List<GXDLMSAccessItem> list = new List<GXDLMSAccessItem>();
                foreach (GXDLMSObject it in objs)
                {
                    if ((it is GXDLMSRegister || it is GXDLMSExtendedRegister) && dlmsClient.CanRead(it, 3))
                    {
                        list.Add(new GXDLMSAccessItem(AccessServiceCommandType.Get, it, 3));
                    }
                    else if (it is GXDLMSDemandRegister && dlmsClient.CanRead(it, 4))
                    {
                        list.Add(new GXDLMSAccessItem(AccessServiceCommandType.Get, it, 4));
                    }
                }
                ReadByAccess(list);
            }
            else if ((dlmsClient.NegotiatedConformance & Gurux.DLMS.Enums.Conformance.MultipleReferences) != 0)
            {
                List<KeyValuePair<GXDLMSObject, int>> list = new List<KeyValuePair<GXDLMSObject, int>>();
                foreach (GXDLMSObject it in objs)
                {
                    if ((it is GXDLMSRegister || it is GXDLMSExtendedRegister) && dlmsClient.CanRead(it, 3))
                    {
                        list.Add(new KeyValuePair<GXDLMSObject, int>(it, 3));
                    }
                    if (it is GXDLMSDemandRegister && dlmsClient.CanRead(it, 4))
                    {
                        list.Add(new KeyValuePair<GXDLMSObject, int>(it, 4));
                    }
                }
                if (list.Count != 0)
                {
                    try
                    {
                        ReadList(list);
                    }
                    catch (Exception)
                    {
                        dlmsClient.NegotiatedConformance &= ~Gurux.DLMS.Enums.Conformance.MultipleReferences;
                    }
                }
            }
            if ((dlmsClient.NegotiatedConformance & Gurux.DLMS.Enums.Conformance.MultipleReferences) == 0)
            {
                //Read values one by one.
                foreach (GXDLMSObject it in objs)
                {
                    try
                    {
                        if (it is GXDLMSRegister && dlmsClient.CanRead(it, 3))
                        {
                            Console.WriteLine(it.Name);
                            Read(it, 3);
                        }
                        if (it is GXDLMSDemandRegister && dlmsClient.CanRead(it, 4))
                        {
                            Console.WriteLine(it.Name);
                            Read(it, 4);
                        }
                    }
                    catch
                    {
                        //Actaric SL7000 can return error here. Continue reading.
                    }
                }
            }
        }

        /// <summary>
        /// Read values using Access request.
        /// </summary>
        /// <param name="list">Object to read.</param>
        void ReadByAccess(List<GXDLMSAccessItem> list)
        {
            if (list.Count != 0)
            {
                GXReplyData reply = new GXReplyData();
                byte[][] data = dlmsClient.AccessRequest(DateTime.MinValue, list);
                ReadDataBlock(data, reply);
                dlmsClient.ParseAccessResponse(list, reply.Data);
            }
        }

        /// <summary>
        /// Read list of attributes.
        /// </summary>
        public void ReadList(List<KeyValuePair<GXDLMSObject, int>> list)
        {
            byte[][] data = dlmsClient.ReadList(list);
            GXReplyData reply = new GXReplyData();
            List<object> values = new List<object>();
            foreach (byte[] it in data)
            {
                ReadDataBlock(it, reply);
                if (!reply.IsMoreData)
                {
                    //Value is null if data is send in multiple frames.
                    if (reply.Value is IEnumerable<object>)
                    {
                        values.AddRange((IEnumerable<object>)reply.Value);
                    }
                }
                reply.Clear();
            }
            if (values.Count != list.Count)
            {
                throw new Exception("Invalid reply. Read items count do not match.");
            }
            dlmsClient.UpdateValues(list, values);
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

        #region NIC Related Method
        /*
        /// <summary>
        /// Build NIC and Module FOTA Strings. Arg 1 should be true for negative cases 
        /// </summary>
        /// <param name="isNegativeCase"></param>
        /// <param name="NICFOTAReset"></param>
        /// <param name="ModuleFOTAReset"></param>
        /// <returns></returns>
        public List<string> BuildFotaConfigurationString(bool isNegativeCase = false, bool NICFOTAReset = false, bool ModuleFOTAReset = false)
        {
            List<string> nicConfigData = new List<string>();
            byte[] NICFOTA = (byte[])null;
            byte[] ModuleFOTA = (byte[])null;
            try
            {
                if (NICFOTAReset)
                    NICFOTA = Encoding.ASCII.GetBytes("Zero");
                else
                    NICFOTA = GetNICFotaDataBytes(isNegativeCase);


                if (ModuleFOTAReset)
                    ModuleFOTA = Encoding.ASCII.GetBytes("Zero");
                else
                    ModuleFOTA = GetModuleFotaDataBytes(isNegativeCase);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
            }
            try
            {
                nicConfigData = CalculateFotaHexString(NICFOTA, ModuleFOTA);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
            }
            return nicConfigData;
        }
        public byte[] GetNICFotaDataBytes(bool isNegativeCase)
        {
            Dictionary<int, string> nicDetails = new Dictionary<int, string>();
            nicDetails = FOTAHelper.nicFOTAData;
            byte[] NICDataArray = (byte[])null;
            try
            {
                int TransMode = 0;
                if (nicDetails[7] == "Active")
                    TransMode = 0;
                else if (nicDetails[7] == "Passive")
                    TransMode = 1;
                string ftpPort = nicDetails[6].ToString();
                //NICDataArray = CreateFotaByteSequence(fotaInfo.FWFileName, fotaInfo.FTPAddress, ftpPort, fotaInfo.FTPUserName, fotaInfo.FTPPassword, TransMode);
                if (isNegativeCase)
                    NICDataArray = CreateFotaByteSequence(nicDetails[2], nicDetails[4], ftpPort, nicDetails[3], nicDetails[5], TransMode);
                else
                    NICDataArray = CreateFotaByteSequence(nicDetails[1], nicDetails[4], ftpPort, nicDetails[3], nicDetails[5], TransMode);
            }
            catch
            {
            }
            return NICDataArray;
        }
        public byte[] GetModuleFotaDataBytes(bool isNegativeCase)
        {
            Dictionary<int, string> moduleDetails = new Dictionary<int, string>();
            moduleDetails = FOTAHelper.moduleFOTAData;
            byte[] ModuleDataArray = (byte[])null;
            try
            {
                int TransMode = 0;
                if (moduleDetails[7] == "Active")
                    TransMode = 0;
                else if (moduleDetails[7] == "Passive")
                    TransMode = 1;
                string ftpPort = moduleDetails[6].ToString();
                //ModuleDataArray = CreateFotaByteSequence(fotaInfo.FWFileName, fotaInfo.FTPAddress, ftpPort, fotaInfo.FTPUserName, fotaInfo.FTPPassword, TransMode);
                if (isNegativeCase)
                    ModuleDataArray = CreateFotaByteSequence(moduleDetails[2], moduleDetails[4], ftpPort, moduleDetails[3], moduleDetails[5], TransMode);
                else
                    ModuleDataArray = CreateFotaByteSequence(moduleDetails[1], moduleDetails[4], ftpPort, moduleDetails[3], moduleDetails[5], TransMode);
            }
            catch
            {
            }
            return ModuleDataArray;
        }
        public List<string> CalculateFotaHexString(byte[] NICFOTA, byte[] ModuleFOTA)
        {
            List<string> setString = new List<string>();
            int num = 0;
            if (NICFOTA != null)
            {
                byte[] bytes = new byte[1] { Convert.ToByte(NICFOTA.Length) };
                string str6 = !(GXCommon.ToHex(NICFOTA) == "5A 65 72 6F") ? "0202110D09" + GXCommon.ToHex(bytes) + GXCommon.ToHex(NICFOTA) : "0202110D0900";
                //setString += str6;
                str6 = "0101" + str6;
                setString.Add(str6);
                ++num;
            }
            if (ModuleFOTA != null)
            {
                byte[] bytes = new byte[1] { Convert.ToByte(ModuleFOTA.Length) };
                string str7 = !(GXCommon.ToHex(ModuleFOTA) == "5A 65 72 6F") ? "0202110E09" + GXCommon.ToHex(bytes) + GXCommon.ToHex(ModuleFOTA) : "0202110E0900";
                //setString += str7;
                str7 = "0101" + str7;
                setString.Add(str7);
                ++num;
            }
            return setString;
        }
        public byte[] CreateFotaByteSequence(string FileName, string FTPAddress, string Port, string Username, string Password, int TransMode)
        {
            byte[] fotaBytes = (byte[])null;
            try
            {
                string str1 = EncodeStringWithLengthPrefix(FileName).Trim();
                string str2 = EncodeStringWithLengthPrefix(FTPAddress).Trim();
                ushort result;
                ushort.TryParse(Port, out result);
                string str3 = "02" + GXCommon.ToHex(ConvertInt32ToByteArray(result)).Trim();
                string str4 = EncodeStringWithLengthPrefix(Username).Trim();
                string str5 = EncodeStringWithLengthPrefix(Password).Trim();
                string str6 = "01" + GXCommon.ToHex(new byte[1]
                {
          Convert.ToByte(TransMode)
                });
                fotaBytes = GXCommon.HexToBytes("06" + str1 + str2 + str3 + str4 + str5 + str6);
            }
            catch
            {
            }
            return fotaBytes;
        }
        public string EncodeStringWithLengthPrefix(string Data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(Data);
            return GXCommon.ToHex(new byte[1] { Convert.ToByte(bytes.Length) }) + GXCommon.ToHex(bytes);
        }
        public static byte[] ConvertInt32ToByteArray(ushort I32)
        {
            byte[] bytes = BitConverter.GetBytes(I32);
            Array.Reverse((Array)bytes);
            return bytes;
        }
        */
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
            string logFile = Path.Combine(LOG_DIRECTORY, $"WRAPPERTestLog_{DateTime.Now:ddMMyyyy}.txt");
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

        #region Dispose Method
        public void Dispose()
        {
            Close();
            Disconnect();
            LineTrafficSaveEventHandler();
        }
        #endregion
    }
}
