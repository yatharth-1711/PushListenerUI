using AutoTest.FrameWork;
using AutoTest.FrameWork.Converts;
using Gurux.Common;
using Gurux.DLMS;
using Gurux.DLMS.Enums;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Objects;
using Gurux.DLMS.Objects.Enums;
using Gurux.DLMS.Secure;
using Gurux.Net;
using log4net;
using MeterComm;
using MeterComm.DLMS;
using MeterReader.CommonClasses;
using MeterReader.Converter;
using MeterReader.DLMSInterfaceClasses;
using MeterReader.DLMSInterfaceClasses.ProfileGeneric;
using MeterReader.NicConfiguration;
using MeterReader.TestHelperClasses;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace MeterReader.DLMSNetSerialCommunication
{
    public class WrapperComm : IDisposable
    {
        FOTAInfo fotaInfo = new FOTAInfo();
        FOTARoot fotaRoot = new FOTARoot();
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
        //delegate for Error Text
        public delegate void KeepAliveControl();
        //initial  Event
        public static event KeepAliveControl KeepAliveControlEventHandler = delegate { }; // add empty delegate!;
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
        public static string errorMessage = "";
        static DLMSParser DLMSparse = new DLMSParser();
        public static DataGridView dataGridView;
        public static ProgressBar PB_PGRead;
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
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
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
            errorMessage = "";
            LineTrafficControlEventHandler($"\r\n     Server Connect Request {GetTimeStamp()}", "Send");
            bool IsConnected = false;
            try
            {
                string ip = WrapperInfo.hostName;
                int port = WrapperInfo.port;
                connection = new GXNet(NetworkType.Tcp, ip, port);
                connection.UseIPv6 = true;
                connection.WaitTime = (int)DLMSInfo.ResponseTimeout;
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
                    errorMessage = $"Server Connect Error";
                    LineTrafficControlEventHandler($"\r\n     Server Connect Error {GetTimeStamp()}\r\n", "Receive");
                }
                //if (IsloggerON)
                //_logService.LogMessage(_logBox, $"Connected at IP: {ip} at Port: {port}",Color.Green);
            }
            catch (Exception ex)
            {
                //log.Error($"Error: {ex.Message.ToString()}");
                LineTrafficControlEventHandler($"\r\n     Server Connect Error: {ex.Message.ToString()} {GetTimeStamp()}\r\n", "Receive");
                errorMessage = $"Server Connect Error: {ex.Message.ToString()}";
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
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
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
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
                LineTrafficControlEventHandler($"     DISCONNECT SERVER Error: {ex.Message.ToString()}\r\n", "Receive");
                //MessageBox.Show($"Disconnection error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Close connection to the meter by sending Release and Disconnect Request.
        /// </summary>
        public void Close()
        {
            //LineTrafficControlEventHandler($"\r\n     DISCONNECT Request\r\n", "Send");
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
                        log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
                        //LineTrafficControlEventHandler($"\r\n     DISCONNECT FAILED Error: {ex.Message.ToString()}\r\n", "Receive");
                    }
                    GXReplyData reply = new GXReplyData();
                    if (connection.IsOpen)
                    {
                        ReadDLMSPacket(dlmsClient.DisconnectRequest(), reply);
                        //LineTrafficControlEventHandler($"     DISCONNECT Success\r\n", "Send");
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
                    //LineTrafficControlEventHandler($"     DISCONNECT FAILED Error: {ex.Message.ToString()}\r\n", "Receive");
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
                    log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
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
                    log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
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
            errorMessage = "";
            LineTrafficControlEventHandler($"\r\n     AARQ Request {GetTimeStamp()}", "Send");
            bool IsAARQPassed = false;
            try
            {
                SetGetFromMeter.Wait(50);
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
                    log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
                    IsAARQPassed = false;
                    errorMessage = $"AARQ Error: {ex.Message.ToString()}";
                    LineTrafficControlEventHandler($"\r\n     AARQ Error: {ex.Message.ToString()} {GetTimeStamp()}\r\n", "Receive");
                    //Log(ex.Message.ToString(), Color.Red);
                }
                SetGetFromMeter.Wait(50);
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
                    errorMessage = $"AARQ Error: {reply.ErrorMessage}";
                    LineTrafficControlEventHandler($"\r\n     AARQ Error: {reply.ErrorMessage} {GetTimeStamp()}\r\n", "Receive");
                }
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message.ToString()}");
                IsAARQPassed = false;
                errorMessage = $"AARQ Error: {ex.Message.ToString()}";
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
                    log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
                    IsAARQPassed = false;
                    LineTrafficControlEventHandler($"\r\n     AARQ Error: {ex.Message.ToString()} {GetTimeStamp()}\r\n", "Receive");
                    //Log(ex.Message.ToString(), Color.Red);
                }
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
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
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
            //try
            //{
            if (data == null)
            {
                return true;
            }
            foreach (byte[] it in data)
            {
                SetGetFromMeter.Wait(1);
                //System.Windows.Forms.Application.DoEvents();
                //Log($"\r\nSend Command:\r\n{GXCommon.ToHex(it, true)}", Color.Green);
                //LineTrafficControlEventHandler($"\r\nSend Command:\r\n{GXCommon.ToHex(it, true)}", "Send");
                reply.Clear();
                ReadDataBlock(it, reply);
                //System.Windows.Forms.Application.DoEvents();
                //SetGetFromMeter.Wait(10);
                //Log($"Decoded Message:\r\n{GXCommon.ToHex(reply.Data.Data, true)}", Color.Red);
                //LineTrafficControlEventHandler($"\r\nDecoded Message:\r\n{GXCommon.ToHex(reply.Data.Data, true)}\r\n", "Send");
                KeepAliveControlEventHandler();
            }
            //}
            //catch (Exception ex)
            //{
            //    return reply.Error == 250;
            //}
            return reply.Error == 0;
        }

        /// <summary>
        /// Read data block from the device.
        /// </summary>
        /// <param name="data">data to send</param>
        /// <param name="text">Progress text.</param>
        /// <param name="multiplier"></param>
        /// <returns>Received data.</returns>
        /*
        public void ReadDataBlock(byte[] data, GXReplyData reply)
        {
            ReadDLMSPacket(data, reply);
            partialRecData = "";
            SetGetFromMeter.Wait(1);
            lock (connection.Synchronous)
            {
                //try
                //{
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
                    //System.Windows.Forms.Application.DoEvents();
                    partialRecData = reply.ToString().Replace(" ", "");
                    if (reply.Error != 0)
                        return;
                }
                //}
                //catch (Exception ex)
                //{
                //    log.Error(ex.ToString());
                //}
            }
            //Log($"{GXCommon.ToHex(reply.Data.Data, true)}\n", Color.Black);    
        }
        */
        public void ReadDataBlock(byte[] data, GXReplyData reply)
        {
            // First block
            ReadDLMSPacket(data, reply);

            partialRecData = string.Empty;
            SetGetFromMeter.Wait(1);

            // Loop until all data blocks received
            while (reply.IsMoreData &&
                   (dlmsClient.ConnectionState != Gurux.DLMS.Enums.ConnectionState.None ||
                    dlmsClient.PreEstablishedConnection))
            {
                if (reply.Error != 0)
                    return;

                if (reply.IsStreaming())
                {
                    data = null;
                }
                else
                {
                    data = dlmsClient.ReceiverReady(reply);
                }

                ReadDLMSPacket(data, reply);

                // Preserve your original logic
                partialRecData = reply.ToString().Replace(" ", "");
                SetGetFromMeter.Wait(1);
            }
        }


        /// <summary>
        /// Read DLMS Data from the device.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <returns>Received data.</returns>
        /*
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
                    //System.Windows.Forms.Application.DoEvents();
                    if (!reply.IsStreaming())
                    {
                        WriteTrace("TX:\t" + GXCommon.ToHex(data, true));
                        LineTrafficControlEventHandler($"\r\n(S)  {GXCommon.ToHex(data, true)} {GetTimeStamp()}", "Send");
                        if (WrapperInfo.IsLogXML)
                            LineTrafficControlEventHandler($"\r\n(XML)\r\n{DecodePushDataToXML(data)} {GetTimeStamp()}\r\n", "Send");
                        p.Reply = null;
                        connection.Send(data, null);
                        //System.Windows.Forms.Application.DoEvents();
                        //SetGetFromMeter.Wait(50);
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
                        //System.Diagnostics.Debug.WriteLine("Data send failed. Try to resend " + pos.ToString() + "/3");
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
                        //System.Windows.Forms.Application.DoEvents();
                        //SetGetFromMeter.Wait(1);
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
                            if (++pos >= RetryCount)
                            {
                                LineTrafficControlEventHandler("\r\nFailed to receive reply from the device in given time.\r\n", "Receive");
                                throw new Exception("Failed to receive reply from the device in given time.");
                            }
                            p.Reply = null;
                            connection.Send(data, null);
                            //SetGetFromMeter.Wait(1);
                            //System.Windows.Forms.Application.DoEvents();
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
            if (WrapperInfo.IsLogXML)
                LineTrafficControlEventHandler($"\r\n(XML)\r\n{DecodePushDataToXML(rd.Data)} {GetTimeStamp()}", "Receive");
            LineTrafficControlEventHandler($"\r\nReceived Response:\r\n{reply.ToString()}\r\n", "Receive");
            if (reply.Error != 0)
            {
                if (reply.Error == (short)ErrorCode.Rejected)
                {
                    //SetGetFromMeter.Wait(1);
                    ReadDLMSPacket(data, reply);
                    //System.Windows.Forms.Application.DoEvents();
                }
                else
                {
                    throw new GXDLMSException(reply.Error);
                }
            }
        }
        */
        public void ReadDLMSPacket(byte[] data, GXReplyData reply)
        {
            if (data == null && !reply.IsStreaming())
                return;

            GXReplyData notify = new GXReplyData();
            reply.Error = 0;

            object eop = (byte)0x7E;
            if (dlmsClient.InterfaceType != InterfaceType.HDLC &&
                dlmsClient.InterfaceType != InterfaceType.HdlcWithModeE)
            {
                eop = null;
            }

            GXByteBuffer rd = new GXByteBuffer();
            ReceiveParameters<byte[]> p = new ReceiveParameters<byte[]>()
            {
                Eop = eop,
                Count = dlmsClient.GetFrameSize(rd),
                AllData = true,
                WaitTime = WaitTime
            };

            bool lockTaken = false;

            try
            {
                // 🔒 TIME-BOUND LOCK (KEY FIX)
                if (!Monitor.TryEnter(connection.Synchronous, TimeSpan.FromSeconds(10)))
                    throw new TimeoutException("DLMS connection busy.");

                lockTaken = true;

                int pos = 0;
                bool succeeded = false;

                while (!succeeded && pos < RetryCount)
                {
                    if (!reply.IsStreaming())
                    {
                        WriteTrace("TX:\t" + GXCommon.ToHex(data, true));
                        LineTrafficControlEventHandler($"\r\n(S) {GXCommon.ToHex(data, true)} {GetTimeStamp()}", "Send");
                        if (WrapperInfo.IsLogXML)
                            LineTrafficControlEventHandler($"\r\n(XML)\r\n{DecodePushDataToXML(data)} {GetTimeStamp()}\r\n", "Send");

                        connection.Send(data, null);
                    }

                    p.Reply = null;
                    succeeded = connection.Receive(p);

                    if (!succeeded)
                    {
                        pos++;
                        if (p.Eop == null)
                            p.Count = 1;

                        if (pos >= RetryCount)
                        {
                            LineTrafficControlEventHandler($"\r\nFailed to receive reply from the device in given time.\r\n", "Receive");
                            throw new Exception("Failed to receive reply from the device in given time.");
                        }
                        LineTrafficControlEventHandler("\r\n" + "Data send failed. Try to resend " + pos.ToString() + "/3" + "\r\n", "Receive");
                    }
                }

                rd.Set(p.Reply);
                pos = 0;

                while (!dlmsClient.GetData(rd, reply, notify))
                {
                    if (notify.IsComplete && notify.Data?.Data != null)
                    {
                        if (!notify.IsMoreData)
                        {
                            if (notify.PrimeDc != null)
                                OnNotification?.Invoke(notify.PrimeDc);
                            else
                            {
                                GXDLMSTranslator t =
                                    new GXDLMSTranslator(TranslatorOutputType.SimpleXml);
                                t.DataToXml(notify.Data, out string xml);
                                OnNotification?.Invoke(xml);
                            }
                            notify.Clear();
                            continue;
                        }
                    }

                    if (p.Eop == null)
                        p.Count = dlmsClient.GetFrameSize(rd);

                    if (!connection.Receive(p))
                    {
                        if (++pos >= RetryCount)
                        {
                            LineTrafficControlEventHandler("\r\nFailed to receive reply from the device in given time.\r\n", "Receive");
                            throw new Exception("Failed to receive reply from the device in given time.");
                        }
                    }
                    else
                    {
                        rd.Set(p.Reply);
                    }
                }
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(connection.Synchronous);
            }

            // ===== LOGGING (unchanged) =====
            WriteTrace("RX:\t" + rd);
            LineTrafficControlEventHandler($"\r\n(R) {rd} {GetTimeStamp()}", "Receive");
            if (WrapperInfo.IsLogXML)
                LineTrafficControlEventHandler($"\r\n(XML)\r\n{DecodePushDataToXML(rd.Data)} {GetTimeStamp()}", "Receive");
            LineTrafficControlEventHandler($"\r\nReceived Response:\r\n{reply.ToString()}\r\n", "Receive");
            if (reply.Error != 0)
                throw new GXDLMSException(reply.Error);
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
                SetGetFromMeter.Wait(1);
                if (!ReadDataBlock(dlmsClient.Read(it, attributeIndex), reply))
                {
                    if (reply.Error != (short)ErrorCode.Rejected)
                    {
                        throw new GXDLMSException(reply.Error);
                    }
                    reply.Clear();
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
                recData = reply.ToString().Replace(" ", "");
                LineTrafficControlEventHandler($"\r\nReceived Data:\r\n{reply.ToString()}\r\n", "Receive");
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
            GXReplyData reply = new GXReplyData();
            recData = "";
            errorMessage = "";
            try
            {
                if (dlmsClient.CanRead(createObjToRead, attributeIndex))
                {
                    if (!ReadDataBlock(dlmsClient.Read(createObjToRead, attributeIndex), reply))
                    {
                        if (reply.Error != (short)ErrorCode.Rejected)
                        {
                            log.Error(reply.Error.ToString());
                            //throw new GXDLMSException(reply.Error);
                        }
                        reply.Clear();
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
                errorMessage = ex.Message.ToString();
                recData = reply.ToString().Replace(" ", "");
                //log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
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
                    SetGetFromMeter.Wait(10);
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
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
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
            SetGetFromMeter.Wait(50);
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
            recData = reply.ToString().Replace(" ", "");
            LineTrafficControlEventHandler($"\r\nReceived Object List Data:\r\n{reply.ToString()}\r\n", "Receive");
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
                        log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
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
                SetGetFromMeter.Wait(10);
                //Get Profile Objects
                ReadCOSEMObject(ObjectType.ProfileGeneric, profileObis, 3);
                recivedObisString = recData;
                SetGetFromMeter.Wait(10);
                //Get Profile Values
                if (profileObis == "1.0.94.91.0.255")
                    ReadCOSEMObject(ObjectType.ProfileGeneric, profileObis, 2);
                else
                    ReadProfileGenericBuffer(profileObis, 1, 1);
                recivedValueString = recData;
                SetGetFromMeter.Wait(10);
                //Get Scaler Objects
                ReadCOSEMObject(ObjectType.ProfileGeneric, scalerObis, 3);
                recivedScalerObisString = recData;
                SetGetFromMeter.Wait(10);
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
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
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
        public Dictionary<string, object> GetSingleEntryLSorDEDataTable(Dictionary<string, object> dataObject)
        {
            DLMSParser dlmsParse = new DLMSParser();
            string recivedObisString = (string)dataObject["ProfileObis"], recivedValueString = (string)dataObject["ProfileValues"], recivedScalerObisString = (string)dataObject["ScalerObis"], recivedScalerValueString = (string)dataObject["ScalerValues"];
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
                #region Data Getting Logic
                obisDataTable = dlmsParse.GetParameterTableHorizontal(recivedObisString);
                #region Add a new column for serial numbers
                DataColumn snColumn = new DataColumn("SN", typeof(string));
                obisDataTable.Columns.Add(snColumn);
                // Set the new column to be the first column
                snColumn.SetOrdinal(0);
                // Populate the SN column with row numbers
                for (int i = 0; i < obisDataTable.Rows.Count; i++)
                {
                    obisDataTable.Rows[i]["SN"] = i + 1;
                }
                obisDataTable.AcceptChanges();
                #endregion
                scalerObisArray = dlmsParse.GetScalerProfileObisListParsing(recivedScalerObisString);
                scalerScalerDataArray = dlmsParse.GetScalerProfileScalerListParsing(recivedScalerValueString);
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
                resultDataTable = dlmsParse.GetBillingValuesDataTableParsing(recivedValueString, obisDataTable);
                // Multiply each row value with the corresponding values in "Data" columns
                RenameParameterWithUnit(resultDataTable, dlmsParse);
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
        static void RenameParameterWithUnit(DataTable dataTable, DLMSParser parse)
        {
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
                    SetGetFromMeter.Wait(10);
                    ReadCOSEMObject(ObjectType.ProfileGeneric, obis, 3);
                    downloadedList.Add($"{obis}|3|{DLMSParser.GetObisName("7", obis, "3")}", recData.Trim());
                    staticDownloadedList.Add($"{obis}|3|{DLMSParser.GetObisName("7", obis, "3")}", recData.Trim());
                    SetGetFromMeter.Wait(10);
                    ReadCOSEMObject(ObjectType.ProfileGeneric, obis, 2);
                    downloadedList.Add($"{obis}|2|{DLMSParser.GetObisName("7", obis, "2")}", recData.Trim());
                    staticDownloadedList.Add($"{obis}|2|{DLMSParser.GetObisName("7", obis, "2")}", recData.Trim());
                    if (!string.IsNullOrEmpty(WrapperComm.errorMessage))
                    {
                        Disconnect();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");

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
                SetGetFromMeter.Wait(50);
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
        private void PB_PGUpdate(int _value)
        {
            PB_PGRead.Value = _value;
            PB_PGRead.Invalidate();
            PB_PGRead.Update();
        }
        private void setRowNumber(DataGridView DG)
        {
            string[] rowNumbers = new string[DG.RowCount + 1];
            foreach (DataGridViewRow row in DG.Rows)
            {
                rowNumbers[row.Index + 1] = (row.Index + 1).ToString();
            }
            // Create a new DataGridViewTextBoxColumn
            DataGridViewTextBoxColumn newColumn = new DataGridViewTextBoxColumn();
            newColumn.HeaderText = "SN"; // Set the header text for the new column
            // Add the new column to the beginning of the DataGridView's columns
            DG.Columns.Insert(0, newColumn);
            // Populate the cells of the new column with the elements of the string array
            for (int i = 0; i < rowNumbers.Length - 1; i++)
            {
                DG.Rows[i].Cells[0].Value = rowNumbers[i + 1];
            }
            foreach (DataGridViewColumn column in DG.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            // Set the alignment for the specified column
            DG.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DG.Invalidate();
        }
        private void FormatGrid(string profile, DataTable dataTable)
        {
            switch (profile)
            {
                case "Instantaneous":
                    try
                    {
                        dataGridView.DataSource = null;
                        dataGridView.Columns.Clear();
                        dataGridView.DataSource = dataTable;
                        setRowNumber(dataGridView);
                        dataGridView.ScrollBars = ScrollBars.Both;
                        foreach (DataGridViewColumn column in dataGridView.Columns)
                        {
                            // Set alignment of cell data to center
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            column.Frozen = false;
                            if (column.Index <= 5)
                                column.Frozen = true;
                            else
                            {
                                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                                if (column.HeaderText.Contains("Data"))
                                    column.Width = 140;
                                else if (column.HeaderText.Contains("Value"))
                                    column.Width = 145;
                                else
                                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            }
                            if (column.HeaderText.Contains("Parameter") || column.HeaderText.Contains("Data"))
                                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        }
                        dataGridView.Columns[0].Width = 40;
                        dataGridView.Columns[1].Width = 180;
                        dataGridView.Columns[2].Width = 45;
                        dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView.Columns[3].Width = 100;
                        dataGridView.Columns[4].Width = 60;
                        dataGridView.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView.Columns[5].Width = 70;
                        dataGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    catch
                    {
                        break;
                    }
                    break;
                case "Billing Profile":
                    try
                    {
                        dataGridView.DataSource = null;
                        dataGridView.Columns.Clear();
                        dataGridView.DataSource = dataTable;
                        dataGridView.Columns[0].Width = 40;
                        dataGridView.Columns[1].Width = 250;
                        dataGridView.Columns[2].Width = 60;
                        dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView.Columns[3].Width = 180;
                        dataGridView.Columns[4].Width = 60;
                        dataGridView.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView.Columns[5].Width = 80;
                        dataGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        setRowNumber(dataGridView);
                        dataGridView.ScrollBars = ScrollBars.Both;
                        foreach (DataGridViewColumn column in dataGridView.Columns)
                        {
                            // Set alignment of cell data to center
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            column.Frozen = false;
                            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            if (column.Index <= 5)
                                column.Frozen = true;
                            else
                            {
                                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            }
                            if (column.Index > 5)
                                column.Width = 180;
                            if (column.HeaderText.Contains("Parameter") || column.HeaderText.Contains("Data"))
                                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        }
                        dataGridView.Columns[0].Width = 40;
                    }
                    catch
                    {
                        break;
                    }
                    break;
                case "Nameplate":
                    try
                    {
                        dataGridView.DataSource = null;
                        dataGridView.Columns.Clear();
                        dataGridView.DataSource = dataTable;
                        setRowNumber(dataGridView);
                        dataGridView.ScrollBars = ScrollBars.Both;
                        //foreach (DataGridViewColumn column in dataGridView.Columns)
                        //{
                        //    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        //}
                        foreach (DataGridViewColumn column in dataGridView.Columns)
                        {
                            // Set alignment of cell data to center
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            column.Frozen = false;
                            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            if (column.Index <= 5)
                                column.Frozen = true;
                            else
                            {
                                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                                if (column.HeaderText.Contains("Data"))
                                    column.Width = 160;
                                else if (column.HeaderText.Contains("Value"))
                                    column.Width = 160;
                                else
                                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            }
                            if (column.HeaderText.Contains("Parameter") || column.HeaderText.Contains("Data"))
                                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        }
                        dataGridView.Columns[0].Width = 40;
                        dataGridView.Columns[1].Width = 180;
                        dataGridView.Columns[2].Width = 45;
                        dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView.Columns[3].Width = 100;
                        dataGridView.Columns[4].Width = 60;
                        dataGridView.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView.Columns[5].Width = 50;
                        dataGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    catch
                    {
                        break;
                    }
                    break;
                case "Daily Energy":
                    try
                    {
                        dataGridView.DataSource = null;
                        dataGridView.Columns.Clear();
                        dataGridView.DataSource = dataTable;
                        setRowNumber(dataGridView);
                        dataGridView.ScrollBars = ScrollBars.Both;
                        foreach (DataGridViewColumn column in dataGridView.Columns)
                        {
                            // Set alignment of cell data to center
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            column.Frozen = false;
                            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            if (column.Index <= 5)
                                column.Frozen = true;
                            if (column.HeaderText.Contains("Parameter") || column.HeaderText.Contains("Data"))
                                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        }
                        dataGridView.Columns[0].Width = 40;
                    }
                    catch
                    {
                        break;
                    }
                    break;
                case "Load Survey":
                    try
                    {
                        dataGridView.DataSource = null;
                        dataGridView.Columns.Clear();
                        dataGridView.DataSource = dataTable;
                        setRowNumber(dataGridView);
                        dataGridView.ScrollBars = ScrollBars.Both;
                        foreach (DataGridViewColumn column in dataGridView.Columns)
                        {
                            // Set alignment of cell data to center
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            column.Frozen = false;
                            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            if (column.Index <= 5)
                                column.Frozen = true;
                            if (column.HeaderText.Contains("Parameter") || column.HeaderText.Contains("Data"))
                                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        }
                        dataGridView.Columns[0].Width = 40;
                    }
                    catch
                    {
                        break;
                    }
                    break;
                case "Load Survey Vertical":
                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    setRowNumber(dataGridView);
                    dataGridView.ScrollBars = ScrollBars.Both;
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        // Set alignment of cell data to center
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        if (column.Index < 2)
                            column.Frozen = true;
                        if (column.Index > 0)
                            column.Width = 150;
                    }
                    dataGridView.Columns[0].Width = 40;
                    break;
                case "Load Survey Vertical-AllData":
                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    setRowNumber(dataGridView);
                    dataGridView.ScrollBars = ScrollBars.Both;
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        // Set alignment of cell data to center
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        if (column.Index < 2)
                            column.Frozen = true;
                        if (column.Index > 0)
                            column.Width = 150;
                    }
                    dataGridView.Columns[0].Width = 40;
                    break;
                case "Daily Energy Vertical":
                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    setRowNumber(dataGridView);
                    dataGridView.ScrollBars = ScrollBars.Both;
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        // Set alignment of cell data to center
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        if (column.Index < 2)
                            column.Frozen = true;
                        if (column.Index > 0)
                            column.Width = 150;
                    }
                    dataGridView.Columns[0].Width = 40;
                    break;
                case "Daily Energy Vertical-AllData":
                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    setRowNumber(dataGridView);
                    dataGridView.ScrollBars = ScrollBars.Both;
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        // Set alignment of cell data to center
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        if (column.Index < 2)
                            column.Frozen = true;
                        if (column.Index > 0)
                            column.Width = 150;
                    }
                    dataGridView.Columns[0].Width = 40;
                    break;
                case "Power Related Events":
                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    setRowNumber(dataGridView);
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        // Set alignment of cell data to center
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    dataGridView.Columns[0].Width = 40;
                    break;
                case "Transaction Events":
                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    setRowNumber(dataGridView);
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        // Set alignment of cell data to center
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    dataGridView.Columns[0].Width = 40;
                    dataGridView.Refresh();
                    break;
                case "Other Tamper Events":
                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    setRowNumber(dataGridView);
                    dataGridView.ScrollBars = ScrollBars.Both;
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        // Set alignment of cell data to center
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        if (column.Index < 3)
                            column.Frozen = true;
                        else
                        {
                            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        }
                        if (column.Index > 0)
                            column.Width = 150;
                    }
                    dataGridView.Columns[0].Width = 40;
                    break;
                case "Non Roll Over Events":

                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    setRowNumber(dataGridView);
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        // Set alignment of cell data to center
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    dataGridView.Columns[0].Width = 40;
                    break;
                case "Control Events":
                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    setRowNumber(dataGridView);
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    dataGridView.Columns[0].Width = 40;
                    break;
            }
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        static int FindColumnIndexByName(DataGridView dataGridView, string columnName)
        {
            // Iterate through the columns and compare names
            for (int i = 0; i < dataGridView.Columns.Count; i++)
            {
                if (dataGridView.Columns[i].Name == columnName)
                {
                    // Column found, return its index
                    return i;
                }
            }

            // Column not found, return -1
            return -1;
        }
        public static int MapRange(double value, double minValue, double maxValue, int lowerRange, int upperRange)
        {
            // Map the value from the range [minValue, maxValue] to the range [50, 90]
            // Formula: ((value - minValue) / (maxValue - minValue)) * (newMax - newMin) + newMin
            return (int)(((value - minValue) / (maxValue - minValue)) * (upperRange - lowerRange) + lowerRange);
        }
        public DataTable ReadProfileGenericEvents(string option = "Power Related Events", int _startIndex = 0, int _endIndex = 0, byte nType = 2)
        {
            // Create a DataTable
            DataTable obisDataTable = new DataTable();
            // Add columns to the DataTable
            obisDataTable.Columns.Add("Obis", typeof(string));
            obisDataTable.Columns.Add("Name", typeof(string));
            obisDataTable.Columns.Add("Data", typeof(string));
            DataTable resultDataTable = new DataTable();
            DLMSParser parse = new DLMSParser(dataGridView);
            string recivedObisString = string.Empty;
            string recivedValueString = string.Empty;
            string recivedScalerObisString = string.Empty;
            string recivedScalerValueString = string.Empty;
            bool result = false;
            string[] scalerObisArray = null;
            string[] scalerScalerDataArray = null;
            string[] mainSourceObisArray = null;
            string[] finalScalerValuesToFill = null;
            string[] ScalerMultiFactorArray = null;
            result = false;
            string profileObisInt = "";
            string scalarProfileObisInt = "";
            switch (option)
            {
                case "Voltage Related Events":
                    profileObisInt = "0.0.99.98.0.255";
                    scalarProfileObisInt = "1.0.94.91.7.255";
                    break;
                case "Current Related Events":
                    profileObisInt = "0.0.99.98.1.255";
                    scalarProfileObisInt = "1.0.94.91.7.255";
                    break;
                case "Power Related Events":
                    profileObisInt = "0.0.99.98.2.255";
                    scalarProfileObisInt = "1.0.94.91.7.255";
                    break;
                case "Transaction Events":
                    profileObisInt = "0.0.99.98.3.255";
                    scalarProfileObisInt = "1.0.94.91.7.255";
                    break;
                case "Other Tamper Events":
                    profileObisInt = "0.0.99.98.4.255";
                    scalarProfileObisInt = "1.0.94.91.7.255";
                    break;
                case "Non Roll Over Events":
                    profileObisInt = "0.0.99.98.5.255";
                    scalarProfileObisInt = "1.0.94.91.7.255";
                    break;
                case "Control Events":
                    profileObisInt = "0.0.99.98.6.255";
                    scalarProfileObisInt = "1.0.94.91.7.255";
                    break;
                case "Billing Profile":
                    profileObisInt = "1.0.98.1.0.255";
                    scalarProfileObisInt = "1.0.94.91.6.255";
                    break;
                case "Instantaneous":
                    profileObisInt = "1.0.94.91.0.255";
                    scalarProfileObisInt = "1.0.94.91.3.255";
                    break;
                case "Mode of Relay Operation Profile":
                    profileObisInt = "0.0.99.98.129.255";
                    scalarProfileObisInt = "1.0.94.91.7.255";
                    break;
            }
            /*
            switch (option)
            {
                case "Voltage Related Events":
                    if (DLMSProfileGenericHelper.IsVoltageAvailable)
                    {
                        recivedObisString = DLMSProfileGenericHelper.GetProfileByLogicalName(profileObisInt).capture_objects;
                        recivedScalerObisString = DLMSProfileGenericHelper.GetProfileByLogicalName(scalarProfileObisInt).capture_objects;
                        recivedScalerValueString = DLMSProfileGenericHelper.GetProfileByLogicalName(scalarProfileObisInt).buffer;
                    }
                    else
                    {
                        ReadCOSEMObject(ObjectType.ProfileGeneric, profileObisInt, 3);
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedObisString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile(profileObisInt, profile =>
                            {
                                profile.capture_objects = recivedObisString;
                            });
                        }
                        else
                        {
                            recivedObisString = "";
                            DLMSProfileGenericHelper.SetProfileStatus(profileObisInt, false);
                        }
                        //Scalar Capture Objects
                        ReadCOSEMObject(ObjectType.ProfileGeneric, scalarProfileObisInt, 3);
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedScalerObisString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile(scalarProfileObisInt, profile =>
                            {
                                profile.capture_objects = recivedScalerObisString;
                            });
                        }
                        else
                        {
                            recivedScalerObisString = "";
                            DLMSProfileGenericHelper.SetProfileStatus(scalarProfileObisInt, false);
                        }
                        //Scalar Buffer
                        ReadCOSEMObject(ObjectType.ProfileGeneric, scalarProfileObisInt, 2);
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedScalerValueString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile(scalarProfileObisInt, profile =>
                            {
                                profile.capture_objects = recivedScalerValueString;
                            });
                        }
                        else
                        {
                            recivedScalerValueString = "";
                            DLMSProfileGenericHelper.SetProfileStatus(scalarProfileObisInt, false);
                        }
                    }
                    break;
                case "Current Related Events":
                    if (DLMSProfileGenericHelper.IsCurrentAvailable)
                    {
                        recivedObisString = DLMSProfileGenericHelper.GetProfileByLogicalName("0.0.99.98.1.255").capture_objects;
                        recivedScalerObisString = DLMSProfileGenericHelper.GetProfileByLogicalName("1.0.94.91.7.255").capture_objects;
                        recivedScalerValueString = DLMSProfileGenericHelper.GetProfileByLogicalName("1.0.94.91.7.255").buffer;
                    }
                    else
                    {
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "0.0.99.98.1.255", 3);
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedObisString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("0.0.99.98.1.255", profile =>
                            {
                                profile.capture_objects = recivedObisString;
                            });
                        }
                        else
                        {
                            recivedObisString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("0.0.99.98.1.255", false);
                        }
                        //Scalar Capture Objects
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "1.0.94.91.7.255", 3);
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedScalerObisString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("1.0.94.91.7.255", profile =>
                            {
                                profile.capture_objects = recivedScalerObisString;
                            });
                        }
                        else
                        {
                            recivedScalerObisString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("1.0.94.91.7.255", false);
                        }
                        //Scalar Buffer
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "1.0.94.91.7.255", 2);
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedScalerValueString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("1.0.94.91.7.255", profile =>
                            {
                                profile.capture_objects = recivedScalerValueString;
                            });
                        }
                        else
                        {
                            recivedScalerValueString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("1.0.94.91.7.255", false);
                        }
                    }
                    break;
                case "Power Related Events":
                    if (DLMSProfileGenericHelper.IsCurrentAvailable)
                    {
                        recivedObisString = DLMSProfileGenericHelper.GetProfileByLogicalName("0.0.99.98.2.255").capture_objects;
                        recivedScalerObisString = DLMSProfileGenericHelper.GetProfileByLogicalName("1.0.94.91.7.255").capture_objects;
                        recivedScalerValueString = DLMSProfileGenericHelper.GetProfileByLogicalName("1.0.94.91.7.255").buffer;
                    }
                    else
                    {
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "0.0.99.98.2.255", 3);
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedObisString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("0.0.99.98.2.255", profile =>
                            {
                                profile.capture_objects = recivedObisString;
                            });
                        }
                        else
                        {
                            recivedObisString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("0.0.99.98.2.255", false);
                        }
                        //Scalar Capture Objects
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "1.0.94.91.7.255", 3);
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedScalerObisString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("1.0.94.91.7.255", profile =>
                            {
                                profile.capture_objects = recivedScalerObisString;
                            });
                        }
                        else
                        {
                            recivedScalerObisString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("1.0.94.91.7.255", false);
                        }
                        //Scalar Buffer
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "1.0.94.91.7.255", 2);
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedScalerValueString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("1.0.94.91.7.255", profile =>
                            {
                                profile.capture_objects = recivedScalerValueString;
                            });
                        }
                        else
                        {
                            recivedScalerValueString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("1.0.94.91.7.255", false);
                        }
                    }
                    break;
                case "Transaction Events":
                    if (DLMSProfileGenericHelper.IsCurrentAvailable)
                    {
                        recivedObisString = DLMSProfileGenericHelper.GetProfileByLogicalName("0.0.99.98.3.255").capture_objects;
                        recivedScalerObisString = DLMSProfileGenericHelper.GetProfileByLogicalName("1.0.94.91.7.255").capture_objects;
                        recivedScalerValueString = DLMSProfileGenericHelper.GetProfileByLogicalName("1.0.94.91.7.255").buffer;
                    }
                    else
                    {
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "0.0.99.98.3.255", 3);
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedObisString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("0.0.99.98.3.255", profile =>
                            {
                                profile.capture_objects = recivedObisString;
                            });
                        }
                        else
                        {
                            recivedObisString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("0.0.99.98.3.255", false);
                        }
                        //Scalar Capture Objects
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "1.0.94.91.7.255", 3);
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedScalerObisString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("1.0.94.91.7.255", profile =>
                            {
                                profile.capture_objects = recivedScalerObisString;
                            });
                        }
                        else
                        {
                            recivedScalerObisString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("1.0.94.91.7.255", false);
                        }
                        //Scalar Buffer
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "1.0.94.91.7.255", 2);
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedScalerValueString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("1.0.94.91.7.255", profile =>
                            {
                                profile.capture_objects = recivedScalerValueString;
                            });
                        }
                        else
                        {
                            recivedScalerValueString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("1.0.94.91.7.255", false);
                        }
                    }
                    break;
                case "Other Tamper Events":
                    if (DLMSProfileGenericHelper.IsCurrentAvailable)
                    {
                        recivedObisString = DLMSProfileGenericHelper.GetProfileByLogicalName("0.0.99.98.4.255").capture_objects;
                        recivedScalerObisString = DLMSProfileGenericHelper.GetProfileByLogicalName("1.0.94.91.7.255").capture_objects;
                        recivedScalerValueString = DLMSProfileGenericHelper.GetProfileByLogicalName("1.0.94.91.7.255").buffer;
                    }
                    else
                    {
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "0.0.99.98.4.255", 3);
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedObisString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("0.0.99.98.4.255", profile =>
                            {
                                profile.capture_objects = recivedObisString;
                            });
                        }
                        else
                        {
                            recivedObisString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("0.0.99.98.4.255", false);
                        }
                        //Scalar Capture Objects
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "1.0.94.91.7.255", 3);
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedScalerObisString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("1.0.94.91.7.255", profile =>
                            {
                                profile.capture_objects = recivedScalerObisString;
                            });
                        }
                        else
                        {
                            recivedScalerObisString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("1.0.94.91.7.255", false);
                        }
                        //Scalar Buffer
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "1.0.94.91.7.255", 2);
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedScalerValueString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("1.0.94.91.7.255", profile =>
                            {
                                profile.capture_objects = recivedScalerValueString;
                            });
                        }
                        else
                        {
                            recivedScalerValueString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("1.0.94.91.7.255", false);
                        }
                    }
                    break;
                case "Non Roll Over Events":
                    if (DLMSProfileGenericHelper.IsCurrentAvailable)
                    {
                        recivedObisString = DLMSProfileGenericHelper.GetProfileByLogicalName("0.0.99.98.5.255").capture_objects;
                        recivedScalerObisString = DLMSProfileGenericHelper.GetProfileByLogicalName("1.0.94.91.7.255").capture_objects;
                        recivedScalerValueString = DLMSProfileGenericHelper.GetProfileByLogicalName("1.0.94.91.7.255").buffer;
                    }
                    else
                    {
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "0.0.99.98.5.255", 3);
                        if (!string.IsNullOrEmpty(WrapperComm.errorMessage))
                        {
                            log.Error($"Error Getting {option}. Received data is: {WrapperComm.recData.Trim()}");
                            MessageBox.Show($"{WrapperComm.errorMessage}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return obisDataTable;
                        }
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedObisString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("0.0.99.98.5.255", profile =>
                            {
                                profile.capture_objects = recivedObisString;
                            });
                        }
                        else
                        {
                            recivedObisString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("0.0.99.98.5.255", false);
                        }
                        //Scalar Capture Objects
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "1.0.94.91.7.255", 3);
                        if (!string.IsNullOrEmpty(WrapperComm.errorMessage))
                        {
                            log.Error($"Error Getting {option}. Received data is: {WrapperComm.recData.Trim()}");
                            MessageBox.Show($"{WrapperComm.errorMessage}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return obisDataTable;
                        }
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedScalerObisString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("1.0.94.91.7.255", profile =>
                            {
                                profile.capture_objects = recivedScalerObisString;
                            });
                        }
                        else
                        {
                            recivedScalerObisString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("1.0.94.91.7.255", false);
                        }
                        //Scalar Buffer
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "1.0.94.91.7.255", 2);
                        if (!string.IsNullOrEmpty(WrapperComm.errorMessage))
                        {
                            log.Error($"Error Getting {option}. Received data is: {WrapperComm.recData.Trim()}");
                            MessageBox.Show($"{WrapperComm.errorMessage}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return obisDataTable;
                        }
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedScalerValueString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("1.0.94.91.7.255", profile =>
                            {
                                profile.capture_objects = recivedScalerValueString;
                            });
                        }
                        else
                        {
                            recivedScalerValueString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("1.0.94.91.7.255", false);
                        }
                    }
                    break;
                case "Control Events":
                    if (DLMSProfileGenericHelper.IsCurrentAvailable)
                    {
                        recivedObisString = DLMSProfileGenericHelper.GetProfileByLogicalName("0.0.99.98.6.255").capture_objects;
                        recivedScalerObisString = DLMSProfileGenericHelper.GetProfileByLogicalName("1.0.94.91.7.255").capture_objects;
                        recivedScalerValueString = DLMSProfileGenericHelper.GetProfileByLogicalName("1.0.94.91.7.255").buffer;
                    }
                    else
                    {
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "0.0.99.98.6.255", 3);
                        if (!string.IsNullOrEmpty(WrapperComm.errorMessage))
                        {
                            log.Error($"Error Getting {option}. Received data is: {WrapperComm.recData.Trim()}");
                            MessageBox.Show($"{WrapperComm.errorMessage}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return obisDataTable;
                        }
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedObisString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("0.0.99.98.6.255", profile =>
                            {
                                profile.capture_objects = recivedObisString;
                            });
                        }
                        else
                        {
                            recivedObisString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("0.0.99.98.6.255", false);
                        }
                        //Scalar Capture Objects
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "1.0.94.91.7.255", 3);
                        if (!string.IsNullOrEmpty(WrapperComm.errorMessage))
                        {
                            log.Error($"Error Getting {option}. Received data is: {WrapperComm.recData.Trim()}");
                            MessageBox.Show($"{WrapperComm.errorMessage}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return obisDataTable;
                        }
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedScalerObisString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("1.0.94.91.7.255", profile =>
                            {
                                profile.capture_objects = recivedScalerObisString;
                            });
                        }
                        else
                        {
                            recivedScalerObisString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("1.0.94.91.7.255", false);
                        }
                        //Scalar Buffer
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "1.0.94.91.7.255", 2);
                        if (!string.IsNullOrEmpty(WrapperComm.errorMessage))
                        {
                            log.Error($"Error Getting {option}. Received data is: {WrapperComm.recData.Trim()}");
                            MessageBox.Show($"{WrapperComm.errorMessage}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return obisDataTable;
                        }
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedScalerValueString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("1.0.94.91.7.255", profile =>
                            {
                                profile.capture_objects = recivedScalerValueString;
                            });
                        }
                        else
                        {
                            recivedScalerValueString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("1.0.94.91.7.255", false);
                        }
                    }
                    break;
                case "Billing Profile":
                    if (DLMSProfileGenericHelper.IsCurrentAvailable)
                    {
                        recivedObisString = DLMSProfileGenericHelper.GetProfileByLogicalName("1.0.98.1.0.255").capture_objects;
                        recivedScalerObisString = DLMSProfileGenericHelper.GetProfileByLogicalName("1.0.94.91.6.255").capture_objects;
                        recivedScalerValueString = DLMSProfileGenericHelper.GetProfileByLogicalName("1.0.94.91.6.255").buffer;
                    }
                    else
                    {
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "1.0.98.1.0.255", 3);
                        if (!string.IsNullOrEmpty(WrapperComm.errorMessage))
                        {
                            log.Error($"Error Getting {option}. Received data is: {WrapperComm.recData.Trim()}");
                            MessageBox.Show($"{WrapperComm.errorMessage}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return obisDataTable;
                        }
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedObisString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("1.0.98.1.0.255", profile =>
                            {
                                profile.capture_objects = recivedObisString;
                            });
                        }
                        else
                        {
                            recivedObisString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("1.0.98.1.0.255", false);
                        }
                        //Scalar Capture Objects
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "1.0.94.91.6.255", 3);
                        if (!string.IsNullOrEmpty(WrapperComm.errorMessage))
                        {
                            log.Error($"Error Getting {option}. Received data is: {WrapperComm.recData.Trim()}");
                            MessageBox.Show($"{WrapperComm.errorMessage}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return obisDataTable;
                        }
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedScalerObisString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("1.0.94.91.6.255", profile =>
                            {
                                profile.capture_objects = recivedScalerObisString;
                            });
                        }
                        else
                        {
                            recivedScalerObisString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("1.0.94.91.6.255", false);
                        }
                        //Scalar Buffer
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "1.0.94.91.6.255", 2);
                        if (!string.IsNullOrEmpty(WrapperComm.errorMessage))
                        {
                            log.Error($"Error Getting {option}. Received data is: {WrapperComm.recData.Trim()}");
                            MessageBox.Show($"{WrapperComm.errorMessage}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return obisDataTable;
                        }
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedScalerValueString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("1.0.94.91.6.255", profile =>
                            {
                                profile.capture_objects = recivedScalerValueString;
                            });
                        }
                        else
                        {
                            recivedScalerValueString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("1.0.94.91.6.255", false);
                        }
                    }
                    break;
                case "Instantaneous":
                    if (DLMSProfileGenericHelper.IsCurrentAvailable)
                    {
                        recivedObisString = DLMSProfileGenericHelper.GetProfileByLogicalName("1.0.94.91.0.255").capture_objects;
                        recivedScalerObisString = DLMSProfileGenericHelper.GetProfileByLogicalName("1.0.94.91.3.255").capture_objects;
                        recivedScalerValueString = DLMSProfileGenericHelper.GetProfileByLogicalName("1.0.94.91.3.255").buffer;
                    }
                    else
                    {
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "1.0.94.91.0.255", 3);
                        if (!string.IsNullOrEmpty(WrapperComm.errorMessage))
                        {
                            log.Error($"Error Getting {option}. Received data is: {WrapperComm.recData.Trim()}");
                            MessageBox.Show($"{WrapperComm.errorMessage}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return obisDataTable;
                        }
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedObisString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("1.0.94.91.0.255", profile =>
                            {
                                profile.capture_objects = recivedObisString;
                            });
                        }
                        else
                        {
                            recivedObisString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("1.0.94.91.0.255", false);
                        }
                        //Scalar Capture Objects
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "1.0.94.91.3.255", 3);
                        if (!string.IsNullOrEmpty(WrapperComm.errorMessage))
                        {
                            log.Error($"Error Getting {option}. Received data is: {WrapperComm.recData.Trim()}");
                            MessageBox.Show($"{WrapperComm.errorMessage}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return obisDataTable;
                        }
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedScalerObisString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("1.0.94.91.3.255", profile =>
                            {
                                profile.capture_objects = recivedScalerObisString;
                            });
                        }
                        else
                        {
                            recivedScalerObisString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("1.0.94.91.3.255", false);
                        }
                        //Scalar Buffer
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "1.0.94.91.3.255", 2);
                        if (!string.IsNullOrEmpty(WrapperComm.errorMessage))
                        {
                            log.Error($"Error Getting {option}. Received data is: {WrapperComm.recData.Trim()}");
                            MessageBox.Show($"{WrapperComm.errorMessage}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return obisDataTable;
                        }
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedScalerValueString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("1.0.94.91.3.255", profile =>
                            {
                                profile.capture_objects = recivedScalerValueString;
                            });
                        }
                        else
                        {
                            recivedScalerValueString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("1.0.94.91.3.255", false);
                        }
                    }
                    break;
                case "Mode of Relay Operation Profile":
                    if (DLMSProfileGenericHelper.IsCurrentAvailable)
                    {
                        recivedObisString = DLMSProfileGenericHelper.GetProfileByLogicalName("0.0.99.98.129.255").capture_objects;
                        recivedScalerObisString = DLMSProfileGenericHelper.GetProfileByLogicalName("1.0.94.91.3.255").capture_objects;
                        recivedScalerValueString = DLMSProfileGenericHelper.GetProfileByLogicalName("1.0.94.91.3.255").buffer;
                    }
                    else
                    {
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "0.0.99.98.129.255", 3);
                        if (!string.IsNullOrEmpty(WrapperComm.errorMessage))
                        {
                            log.Error($"Error Getting {option}. Received data is: {WrapperComm.recData.Trim()}");
                            MessageBox.Show($"{WrapperComm.errorMessage}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return obisDataTable;
                        }
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedObisString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("0.0.99.98.129.255", profile =>
                            {
                                profile.capture_objects = recivedObisString;
                            });
                        }
                        else
                        {
                            recivedObisString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("0.0.99.98.129.255", false);
                        }
                        //Scalar Capture Objects
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "1.0.94.91.7.255", 3);
                        if (!string.IsNullOrEmpty(WrapperComm.errorMessage))
                        {
                            log.Error($"Error Getting {option}. Received data is: {WrapperComm.recData.Trim()}");
                            MessageBox.Show($"{WrapperComm.errorMessage}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return obisDataTable;
                        }
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedScalerObisString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("1.0.94.91.7.255", profile =>
                            {
                                profile.capture_objects = recivedScalerObisString;
                            });
                        }
                        else
                        {
                            recivedScalerObisString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("1.0.94.91.7.255", false);
                        }
                        //Scalar Buffer
                        ReadCOSEMObject(ObjectType.ProfileGeneric, "1.0.94.91.7.255", 2);
                        if (!string.IsNullOrEmpty(WrapperComm.errorMessage))
                        {
                            log.Error($"Error Getting {option}. Received data is: {WrapperComm.recData.Trim()}");
                            MessageBox.Show($"{WrapperComm.errorMessage}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return obisDataTable;
                        }
                        if (WrapperComm.recData.Trim().Length > 14)
                        {
                            recivedScalerValueString = WrapperComm.recData.Trim();
                            DLMSProfileGenericHelper.UpdateProfile("1.0.94.91.7.255", profile =>
                            {
                                profile.capture_objects = recivedScalerValueString;
                            });
                        }
                        else
                        {
                            recivedScalerValueString = "";
                            DLMSProfileGenericHelper.SetProfileStatus("1.0.94.91.7.255", false);
                        }
                    }
                    break;
            }
            */
            ReadCOSEMObject(ObjectType.ProfileGeneric, profileObisInt, 3);
            if (WrapperComm.recData.Trim().Length > 14)
            {
                recivedObisString = WrapperComm.recData.Trim();
                DLMSProfileGenericHelper.UpdateProfile(profileObisInt, profile =>
                {
                    profile.capture_objects = recivedObisString;
                });
            }
            else
            {
                recivedObisString = "";
                DLMSProfileGenericHelper.SetProfileStatus(profileObisInt, false);
            }
            //Scalar Capture Objects
            ReadCOSEMObject(ObjectType.ProfileGeneric, scalarProfileObisInt, 3);
            if (WrapperComm.recData.Trim().Length > 14)
            {
                recivedScalerObisString = WrapperComm.recData.Trim();
                DLMSProfileGenericHelper.UpdateProfile(scalarProfileObisInt, profile =>
                {
                    profile.capture_objects = recivedScalerObisString;
                });
            }
            else
            {
                recivedScalerObisString = "";
                DLMSProfileGenericHelper.SetProfileStatus(scalarProfileObisInt, false);
            }
            //Scalar Buffer
            ReadCOSEMObject(ObjectType.ProfileGeneric, scalarProfileObisInt, 2);
            if (WrapperComm.recData.Trim().Length > 14)
            {
                recivedScalerValueString = WrapperComm.recData.Trim();
                DLMSProfileGenericHelper.UpdateProfile(scalarProfileObisInt, profile =>
                {
                    profile.capture_objects = recivedScalerValueString;
                });
            }
            else
            {
                recivedScalerValueString = "";
                DLMSProfileGenericHelper.SetProfileStatus(scalarProfileObisInt, false);
            }
            PB_PGUpdate(20);

            obisDataTable = parse.GetClassObisAttScalerListParsing(recivedObisString, option);
            if (option == "Billing Profile")
            {
                dataGridView.DataSource = null;
                dataGridView.Columns.Clear();
                dataGridView.DataSource = obisDataTable;
                setRowNumber(dataGridView);
                dataGridView.ScrollBars = ScrollBars.Both;
                dataGridView.Columns[0].Width = 40;
                dataGridView.Columns[1].Width = 250;
                dataGridView.Columns[2].Width = 60;
                dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView.Columns[3].Width = 180;
                dataGridView.Columns[4].Width = 60;
                dataGridView.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView.Columns[5].Width = 80;
                dataGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView.Refresh();

                scalerObisArray = parse.GetScalerProfileObisListParsing(recivedScalerObisString);
                scalerScalerDataArray = parse.GetScalerProfileScalerListParsing(recivedScalerValueString);
                mainSourceObisArray = obisDataTable.AsEnumerable()
                                         .Select(row => row.Field<string>(2)) // Change the type to match the third column's type
                                         .ToArray();
                finalScalerValuesToFill = new string[mainSourceObisArray.Length];
                foreach (var selectedObis in scalerObisArray)
                {
                    // Find the index of the searchString in the stringArray
                    int index = Array.IndexOf(mainSourceObisArray, selectedObis);
                    int scalarIndex = Array.IndexOf(scalerObisArray, selectedObis);
                    if (index == -1)
                    {
                        MessageBox.Show("Object present in Scaler Profile not available in Billing Profile", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        break;
                    }
                    if (index > mainSourceObisArray.Length)
                        break;
                    else
                    {
                        finalScalerValuesToFill[index] = scalerScalerDataArray[scalarIndex];
                        dataGridView.Rows[index].Cells[5].Value = scalerScalerDataArray[scalarIndex];
                    }
                }
                dataGridView.Refresh();
            }
            else if (option == "Instantaneous")
            {
                dataGridView.DataSource = null;
                dataGridView.Columns.Clear();
                dataGridView.DataSource = obisDataTable;
                setRowNumber(dataGridView);
                dataGridView.ScrollBars = ScrollBars.Both;
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    // Set alignment of cell data to center
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.Frozen = false;
                    //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    if (column.Index <= 5)
                        column.Frozen = true;
                    else
                    {
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        if (column.HeaderText.Contains("Data"))
                            column.Width = 140;
                        else if (column.HeaderText.Contains("Value"))
                            column.Width = 145;
                        else
                            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    }
                    if (column.HeaderText.Contains("Parameter") || column.HeaderText.Contains("Data"))
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                dataGridView.Columns[0].Width = 40;
                dataGridView.Columns[1].Width = 180;
                dataGridView.Columns[2].Width = 45;
                dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView.Columns[3].Width = 100;
                dataGridView.Columns[4].Width = 60;
                dataGridView.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView.Columns[5].Width = 70;
                dataGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                scalerObisArray = parse.GetScalerProfileObisListParsing(recivedScalerObisString);
                scalerScalerDataArray = parse.GetScalerProfileScalerListParsing(recivedScalerValueString);
                mainSourceObisArray = obisDataTable.AsEnumerable()
                                         .Select(row => row.Field<string>(2)) // Change the type to match the third column's type
                                         .ToArray();
                finalScalerValuesToFill = new string[mainSourceObisArray.Length];
                ScalerMultiFactorArray = new string[mainSourceObisArray.Length];
                foreach (var selectedObis in scalerObisArray)
                {
                    // Find the index of the searchString in the stringArray
                    int index = Array.IndexOf(mainSourceObisArray, selectedObis);
                    int scalarIndex = Array.IndexOf(scalerObisArray, selectedObis);
                    if (index == -1)
                    {
                        MessageBox.Show("Object present in Scaler Profile not available in Billing Profile", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        break;
                    }
                    if (index > mainSourceObisArray.Length)
                        break;
                    else
                    {
                        finalScalerValuesToFill[index] = scalerScalerDataArray[scalarIndex];
                        dataGridView.Rows[index].Cells[5].Value = scalerScalerDataArray[scalarIndex];
                    }
                }
                //dataGridView.Refresh();
            }
            else if (option.Contains("Events") || option == "Mode of Relay Operation Profile")
            {
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
                    if (index == -1)
                    {

                    }
                    else
                    {
                        finalScalerValuesToFill[index] = scalerScalerDataArray[scalarIndex];
                        ScalerMultiFactorArray[index] = (string)parse.ScalerhshTable[scalerScalerDataArray[scalarIndex].Substring(2, 2)];
                        obisDataTable.Columns[index].ColumnName = (!string.IsNullOrEmpty((string)parse.UnithshTable[scalerScalerDataArray[scalarIndex].Substring(6, 2)])) ?
                                                                $"{obisDataTable.Columns[index].ColumnName} - {scalerScalerDataArray[scalarIndex]} - ({(string)parse.UnithshTable[scalerScalerDataArray[scalarIndex].Substring(6, 2)]})" :
                                                                $"{obisDataTable.Columns[index].ColumnName} - {scalerScalerDataArray[scalarIndex]}";
                    }
                }
                PB_PGUpdate(30);
                if (option == "Power Related Events")
                {
                    FormatGrid(option, obisDataTable);
                }
                else if (option == "Transaction Events")
                {
                    FormatGrid(option, obisDataTable);
                }
                else if (option == "Other Tamper Events")
                {
                    FormatGrid(option, obisDataTable);
                }
                else if (option == "Non Roll Over Events")
                {
                    FormatGrid(option, obisDataTable);
                }
                else if (option == "Control Events")
                {
                    FormatGrid(option, obisDataTable);
                }
                else
                {
                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = obisDataTable;
                    setRowNumber(dataGridView);
                    dataGridView.ScrollBars = ScrollBars.Both;
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        // Set alignment of cell data to center
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        column.Width = 170;
                        //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        if (column.Index < 2)
                            column.Frozen = true;
                    }
                    dataGridView.Columns[0].Width = 40;
                    dataGridView.Refresh();
                }
            }
            result = false;
            PB_PGUpdate(40);

            if (nType != 0)
                ReadProfileGenericBuffer(profileObisInt, _startIndex, _endIndex, nType);
            else
                ReadCOSEMObject(ObjectType.ProfileGeneric, profileObisInt, 2);
            if (!string.IsNullOrEmpty(WrapperComm.errorMessage))
            {
                log.Error($"Error Getting {option}. Received data is: {WrapperComm.recData.Trim()}");
                MessageBox.Show($"{WrapperComm.errorMessage}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return obisDataTable;
            }
            else if (string.IsNullOrEmpty(WrapperComm.errorMessage))
            {
                recivedValueString = WrapperComm.recData.Trim();
                DLMSProfileGenericHelper.UpdateProfile(profileObisInt, profile =>
                {
                    profile.buffer = recivedValueString;
                });
            }
            else
            {
                recivedValueString = "";
                DLMSProfileGenericHelper.SetProfileStatus(profileObisInt, false);
            }

            PB_PGUpdate(50);
            if (option == "Billing Profile" || option == "Instantaneous")
            {
                resultDataTable = parse.GetBillingValuesDataTableParsing(recivedValueString, obisDataTable);
                // Multiply each row value with the corresponding values in "Data" columns
                RenameParameterWithUnit(resultDataTable);
                if (option == "Billing Profile")
                    //MultiplyScalerWithData(resultDataTable, parse);
                    FormatGrid(option, resultDataTable);
                if (option == "Instantaneous")
                {
                    resultDataTable.Columns.Add("Individual-Data", typeof(string));
                    resultDataTable.Columns.Add("Individual-Value", typeof(string));
                    resultDataTable.Columns.Add("Individual-Scaler", typeof(string));
                    int individualDataColumnIndex = FindColumnIndexByName(dataGridView, "Individual-Data");
                    int individualScalerColumnIndex = FindColumnIndexByName(dataGridView, "Individual-Scaler");
                    int individualValueColumnIndex = FindColumnIndexByName(dataGridView, "Individual-Value");
                    FormatGrid(option, resultDataTable);
                    //dataGridView.Refresh();
                    // Find the column index by column name
                    int referenceDataColumnIndex = FindColumnIndexByName(dataGridView, "Entry 1 Data");
                    int referenceValueColumnIndex = FindColumnIndexByName(dataGridView, "Entry 1 Value");
                    int referenceScalerColumnIndex = FindColumnIndexByName(dataGridView, "Scaler");

                    string[] class_Instantaneous = resultDataTable.AsEnumerable()
                                             .Select(row => row.Field<string>(1)) // Change the type to match the column's type
                                             .ToArray();
                    string[] obis_Instantaneous = resultDataTable.AsEnumerable()
                                             .Select(row => row.Field<string>(2)) // Change the type to match the column's type
                                             .ToArray();
                    string[] attribute_Instantaneous = resultDataTable.AsEnumerable()
                                             .Select(row => row.Field<string>(3)) // Change the type to match the column's type
                                             .ToArray();
                    string[] refData = resultDataTable.AsEnumerable()
                                             .Select(row => row.Field<string>(5)) // Change the type to match the column's type
                                             .ToArray();
                    string[] refScaler = resultDataTable.AsEnumerable()
                                             .Select(row => row.Field<string>(4)) // Change the type to match the column's type
                                             .ToArray();
                    string[] refValue = resultDataTable.AsEnumerable()
                                             .Select(row => row.Field<string>(6)) // Change the type to match the column's type
                                             .ToArray();
                    string dataIndividual = string.Empty;
                    string scalerIndividual = string.Empty;
                    string valueIndividual = string.Empty;
                    for (int i = 0; i < resultDataTable.Rows.Count; i++)
                    {
                        PB_PGUpdate(MapRange(i, 0, resultDataTable.Rows.Count, 50, 90));
                        ReadCOSEMObject((ObjectType)Convert.ToInt32(class_Instantaneous[i].Trim()), obis_Instantaneous[i].Trim(), Convert.ToInt32(attribute_Instantaneous[i].Trim()));
                        if (string.IsNullOrEmpty(errorMessage))
                        {
                            dataIndividual = recData;
                            dataGridView.Rows[i].Cells[individualDataColumnIndex].Value = recData;
                            valueIndividual = parse.GetProfileValueString(dataIndividual);
                            dataGridView.Rows[i].Cells[individualValueColumnIndex].Value = valueIndividual;
                        }
                        else
                        {
                            log.Error($"Error Getting Individual Object for {option} Class - {class_Instantaneous[i].Trim()} Obis - {obis_Instantaneous[i].Trim()} Attribute - {attribute_Instantaneous[i].Trim()}. Received data is: {recData.Trim()}");
                            MessageBox.Show($"Error Getting Individual Object for {option} \nClass - {class_Instantaneous[i].Trim()} \nObis - {obis_Instantaneous[i].Trim()} \nAttribute - {attribute_Instantaneous[i].Trim()}.", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        if (i > 0 && (class_Instantaneous[i].Trim() == "3" || class_Instantaneous[i].Trim() == "4") && attribute_Instantaneous[i] == "2")
                        {
                            ReadCOSEMObject((ObjectType)Convert.ToInt32(class_Instantaneous[i].Trim()), obis_Instantaneous[i].Trim(), 3);
                            if (string.IsNullOrEmpty(errorMessage))
                            {
                                scalerIndividual = recData;
                                if (scalerIndividual.Substring(0, 4) == "0202")
                                {
                                    scalerIndividual = scalerIndividual.Substring(4);
                                    if ((mainSourceObisArray[i].Trim() == "1.0.1.6.0.255" && attribute_Instantaneous[i].Trim() == "5") || (mainSourceObisArray[i].Trim() == "1.0.9.6.0.255" && attribute_Instantaneous[i].Trim() == "5"))//MD-W(Imp) Date Time and MD-VA(Imp) Date Time
                                    {
                                        scalerIndividual = "";
                                        dataGridView.Rows[i].Cells[individualScalerColumnIndex].Value = scalerIndividual;

                                    }
                                    else
                                    {
                                        dataGridView.Rows[i].Cells[individualScalerColumnIndex].Value = scalerIndividual;
                                        if (string.IsNullOrEmpty((string)parse.UnithshTable[scalerIndividual.Trim().Substring(6, 2)]) && (string)parse.UnithshTable[scalerIndividual.Trim().Substring(6, 2)] == "1")
                                        {
                                            valueIndividual = parse.GetProfileValueString(dataIndividual);
                                            dataGridView.Rows[i].Cells[individualValueColumnIndex].Value = valueIndividual.Trim();
                                        }
                                        else
                                        {
                                            try
                                            {
                                                valueIndividual = parse.GetProfileValueString(dataIndividual);
                                                double scaledValue = Convert.ToDouble((string)parse.ScalerhshTable[scalerIndividual.Trim().Substring(2, 2)]) * Convert.ToDouble(valueIndividual);
                                                dataGridView.Rows[i].Cells[individualValueColumnIndex].Value = scaledValue.ToString();
                                                valueIndividual = scaledValue.ToString();
                                            }
                                            catch (Exception ex)
                                            {
                                                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
                                                //MessageBox.Show(ex.Message, "ERROR");
                                                dataGridView.Rows[i].Cells[individualValueColumnIndex].Value = valueIndividual;
                                            }
                                        }
                                    }

                                }
                            }
                            else if (!string.IsNullOrEmpty(errorMessage))
                            {

                                dataGridView.Rows[i].Cells[individualScalerColumnIndex].Value = "";
                                scalerIndividual = "";
                                valueIndividual = parse.GetProfileValueString(dataIndividual);
                                dataGridView.Rows[i].Cells[individualValueColumnIndex].Value = valueIndividual.Trim();

                                log.Error($"Error Getting Individual Object for {option} Class - {class_Instantaneous[i].Trim()} Obis - {obis_Instantaneous[i].Trim()} Attribute - {3}. Received data is: {recData.Trim()}");
                                MessageBox.Show($"Error Getting Individual Object for {option} \nClass - {class_Instantaneous[i].Trim()} \nObis - {obis_Instantaneous[i].Trim()} \nAttribute - {3}.", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                resultDataTable = parse.GetEventsValuesDataTableParsing(recivedValueString, obisDataTable, option);
                PB_PGUpdate(60);
                // Multiply each row value with the corresponding string array value
                MultiplyRowsWithArray(resultDataTable, ScalerMultiFactorArray);
                if (option == "Power Related Events")
                {
                    FormatGrid(option, resultDataTable);
                }
                else if (option == "Transaction Events")
                {
                    FormatGrid(option, resultDataTable);
                }
                else if (option == "Other Tamper Events")
                {
                    FormatGrid(option, resultDataTable);
                }
                else if (option == "Non Roll Over Events")
                {
                    FormatGrid(option, resultDataTable);
                }
                else if (option == "Control Events")
                {
                    FormatGrid(option, resultDataTable);
                }
                else
                {
                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = resultDataTable;
                    setRowNumber(dataGridView);
                    dataGridView.ScrollBars = ScrollBars.Both;
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        // Set alignment of cell data to center
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        column.Width = 170;
                        //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        if (column.Index < 2)
                            column.Frozen = true;
                    }
                    //dataGridView.Refresh();
                    dataGridView.Columns[0].Width = 40;
                }
                PB_PGUpdate(70);
            }
            PB_PGUpdate(80);
            dataGridView.Invalidate();
            resultDataTable = DataGridViewOperations.ConvertDataGridViewToDataTable(ref dataGridView);
            return resultDataTable;
        }

        public bool AllProfileObjectList()
        {
            //// Create a DataTable
            DataTable resultDataTable = new DataTable();
            DLMSParser parse = new DLMSParser(dataGridView);
            bool result = false;
            string recivedObisString = string.Empty;
            string[] scalerObisArray = null;
            #region Add columns to the DataGridView
            dataGridView.DataSource = null;
            dataGridView.Columns.Clear();
            dataGridView.Columns.Add("SN", "SN");//1
            dataGridView.Columns.Add("Instant-C", "Instant-C");//2
            dataGridView.Columns.Add("Instant-O", "Instant-O");//3
            dataGridView.Columns.Add("Instant-A", "Instant-A");//4
            dataGridView.Columns.Add("Instant-S-C", "Instant-S-C");//5
            dataGridView.Columns.Add("Instant-S-O", "Instant-S-O");//6
            dataGridView.Columns.Add("Instant-S-A", "Instant-S-A");//7
            dataGridView.Columns.Add("Nameplate-C", "Nameplate-C");//8
            dataGridView.Columns.Add("Nameplate-O", "Nameplate-O");//9
            dataGridView.Columns.Add("Nameplate-A", "Nameplate-A");//10
            dataGridView.Columns.Add("Voltage-C", "Voltage-C");//11
            dataGridView.Columns.Add("Voltage-O", "Voltage-O");//12
            dataGridView.Columns.Add("Voltage-A", "Voltage-A");//13
            dataGridView.Columns.Add("Current-C", "Current-C");//14
            dataGridView.Columns.Add("Current-O", "Current-O");//15
            dataGridView.Columns.Add("Current-A", "Current-A");//16
            dataGridView.Columns.Add("Power-C", "Power-C");//17
            dataGridView.Columns.Add("Power-O", "Power-O");//18
            dataGridView.Columns.Add("Power-A", "Power-A");//19
            dataGridView.Columns.Add("Transaction-C", "Transaction-C");//20
            dataGridView.Columns.Add("Transaction-O", "Transaction-O");//21
            dataGridView.Columns.Add("Transaction-A", "Transaction-A");//22
            dataGridView.Columns.Add("Other-C", "Other-C");//23
            dataGridView.Columns.Add("Other-O", "Other-O");//24
            dataGridView.Columns.Add("Other-A", "Other-A");//25
            dataGridView.Columns.Add("NonRollOver-C", "NonRollOver-C");//26
            dataGridView.Columns.Add("NonRollOver-O", "NonRollOver-O");//27
            dataGridView.Columns.Add("NonRollOver-A", "NonRollOver-A");//28
            dataGridView.Columns.Add("Control-C", "Control-C");//29
            dataGridView.Columns.Add("Control-O", "Control-O");//30
            dataGridView.Columns.Add("Control-A", "Control-A");//31
            dataGridView.Columns.Add("ModeOfRelayO-C", "ModeOfRelayO-C");//32
            dataGridView.Columns.Add("ModeOfRelayO-O", "ModeOfRelayO-O");//33
            dataGridView.Columns.Add("ModeOfRelayO-A", "ModeOfRelayO-A");//34
            dataGridView.Columns.Add("Events-S-C", "Events-S-C");//35
            dataGridView.Columns.Add("Events-S-O", "Events-S-O");//36
            dataGridView.Columns.Add("Events-S-A", "Events-S-A");//37
            dataGridView.Columns.Add("Billing-C", "Billing-C");//38
            dataGridView.Columns.Add("Billing-O", "Billing-O");//39
            dataGridView.Columns.Add("Billing-A", "Billing-A");//40
            dataGridView.Columns.Add("Billing-S-C", "Billing-S-C");//41
            dataGridView.Columns.Add("Billing-S-O", "Billing-S-O");//42
            dataGridView.Columns.Add("Billing-S-A", "Billing-S-A");//43
            dataGridView.Columns.Add("LS-C", "LS-C");//44
            dataGridView.Columns.Add("LS-O", "LS-O");//45
            dataGridView.Columns.Add("LS-A", "LS-A");//46
            dataGridView.Columns.Add("LS-S-C", "LS-S-C");//47
            dataGridView.Columns.Add("LS-S-O", "LS-S-O");//48
            dataGridView.Columns.Add("LS-S-A", "LS-S-A");//49
            dataGridView.Columns.Add("DE-C", "DE-C");//50
            dataGridView.Columns.Add("DE-O", "DE-O");//51
            dataGridView.Columns.Add("DE-A", "DE-A");//52
            dataGridView.Columns.Add("DE-S-C", "DE-S-C");//53
            dataGridView.Columns.Add("DE-S-O", "DE-S-O");//54
            dataGridView.Columns.Add("DE-S-A", "DE-S-A");//55
            dataGridView.Columns.Add("Remarks", "Remarks");//56
            for (int i = 1; i <= 200; i++)
            {
                dataGridView.Rows.Add(i.ToString()); // Add a new row and populate the first cell with the SN value
            }
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                // Set alignment of cell data to center
                column.Frozen = false;
                //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                if (column.Index < 1)
                    column.Frozen = true;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                if (column.HeaderText.Contains("-O") || column.HeaderText.Contains("Remarks"))
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                else
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

                #region Colum color for different profile
                //instant
                if (column.Index >= 1 && column.Index <= 6)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightBlue;
                    }
                //nameplate
                if (column.Index >= 7 && column.Index <= 9)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightGreen;
                    }
                //voltage
                if (column.Index >= 10 && column.Index <= 12)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightCyan;
                    }
                //current
                if (column.Index >= 13 && column.Index <= 15)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightGoldenrodYellow;
                    }
                //power               
                if (column.Index >= 16 && column.Index <= 18)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightPink;
                    }
                //Transactions
                if (column.Index >= 19 && column.Index <= 21)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightSkyBlue;
                    }
                //Other
                if (column.Index >= 22 && column.Index <= 24)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LemonChiffon;
                    }
                //Non Roll Over
                if (column.Index >= 25 && column.Index <= 27)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightBlue;
                    }
                //Control
                if (column.Index >= 28 && column.Index <= 30)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightCoral;
                    }
                //Relay
                if (column.Index >= 31 && column.Index <= 33)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightCyan;
                    }
                //Events
                if (column.Index >= 34 && column.Index <= 36)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightGoldenrodYellow;
                    }
                //Billing
                if (column.Index >= 37 && column.Index <= 42)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightGray;
                    }
                //LS
                if (column.Index >= 43 && column.Index <= 48)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightGreen;
                    }
                //DE
                if (column.Index >= 49 && column.Index <= 54)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightPink;
                    }
                #endregion
            }
            PB_PGUpdate(20);
            #endregion

            string[] Profiles = { "7-1.0.94.91.0.255-3" , //instant
                "7-1.0.94.91.3.255-3" , //instant scaler
                "7-0.0.94.91.10.255-3" , //nameplate
                "7-0.0.99.98.0.255-3" , //voltage
                "7-0.0.99.98.1.255-3" , //current
                "7-0.0.99.98.2.255-3" , //power
                "7-0.0.99.98.3.255-3" , //transaction
                "7-0.0.99.98.4.255-3" , //other
                "7-0.0.99.98.5.255-3" , //non roll over
                "7-0.0.99.98.6.255-3" , //control
                "7-0.0.99.98.129.255-3" , //relay profile
                "7-1.0.94.91.7.255-3" , //Events
                "7-1.0.98.1.0.255-3" , //billing
                "7-1.0.94.91.6.255-3" , //billing scaler
                "7-1.0.99.1.0.255-3" , //LS
                "7-1.0.94.91.4.255-3" , //LS scaler
                "7-1.0.99.2.0.255-3" , //DE
                "7-1.0.94.91.5.255-3"}; //DE scaler

            foreach (var Profile in Profiles)
            {
                int _class = Convert.ToInt16(Profile.ToString().Trim().Split('-')[0]);
                string _obis = Profile.ToString().Trim().Split('-')[1];
                int _attribute = Convert.ToInt16(Profile.ToString().Trim().Split('-')[2]);
                //SetGetFromMeter.Wait(50);
                ReadCOSEMObject(ObjectType.ProfileGeneric, _obis, _attribute);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    //log.Error($"Error Getting {Profile} Class - {_class} Obis - {_obis} Att. - {_attribute}. {errorMessage}");
                    //MessageBox.Show($"{errorMessage} Error in getting Objects. Click OK to Continue.", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    result = false;
                }
                else if (WrapperComm.recData.Trim().Length > 14)
                {
                    result = true;
                }
                else
                {
                    result = true;
                }
                if (result == false)
                {
                    log.Error($"Error Getting {Profile} Class - {_class} Obis - {_obis} Att. - {_attribute}. {errorMessage}");
                    //MessageBox.Show($"{errorMessage} Error Getting {Profile} Class - {_class} Obis - {_obis} Att. - {_attribute}. Click OK to Continue.", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    recivedObisString = WrapperComm.recData.Trim();
                    string[] Class;
                    string[] Obis;
                    string[] Attribute;
                    if (result == true)
                    {
                        if (Profile == "7-1.0.94.91.0.255-3" ||
                            Profile == "7-1.0.94.91.3.255-3" ||
                            Profile == "7-0.0.94.91.10.255-3" ||
                            Profile == "7-0.0.99.98.0.255-3" ||
                            Profile == "7-0.0.99.98.1.255-3" ||
                            Profile == "7-0.0.99.98.2.255-3" ||
                            Profile == "7-0.0.99.98.3.255-3" ||
                            Profile == "7-0.0.99.98.4.255-3" ||
                            Profile == "7-0.0.99.98.5.255-3" ||
                            Profile == "7-0.0.99.98.6.255-3" ||
                            Profile == "7-0.0.99.98.129.255-3" ||
                            Profile == "7-1.0.94.91.7.255-3" ||
                            Profile == "7-1.0.98.1.0.255-3" ||
                            Profile == "7-1.0.94.91.6.255-3" ||
                            Profile == "7-1.0.99.1.0.255-3" ||
                            Profile == "7-1.0.94.91.4.255-3" ||
                            Profile == "7-1.0.99.2.0.255-3" ||
                            Profile == "7-1.0.94.91.5.255-3"
                            )
                        {
                            resultDataTable = parse.GetProfileObjectTable(recivedObisString, Profile);
                            Class = resultDataTable.AsEnumerable()
                                                .Select(row => row.Field<string>(0)) // Change the type to match the column's type
                                                .ToArray();
                            Obis = resultDataTable.AsEnumerable()
                                                .Select(row => row.Field<string>(1)) // Change the type to match the column's type
                                                .ToArray();
                            Attribute = resultDataTable.AsEnumerable()
                                                .Select(row => row.Field<string>(2)) // Change the type to match the column's type
                                                .ToArray();
                            switch (Profile)
                            {
                                case "7-1.0.94.91.0.255-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[1].Value = Class[i];
                                        dataGridView.Rows[i].Cells[2].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[3].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(25);
                                    break;
                                case "7-1.0.94.91.3.255-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[4].Value = Class[i];
                                        dataGridView.Rows[i].Cells[5].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[6].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(30);
                                    break;
                                case "7-0.0.94.91.10.255-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[7].Value = Class[i];
                                        dataGridView.Rows[i].Cells[8].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[9].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(40);
                                    break;
                                case "7-0.0.99.98.0.255-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[10].Value = Class[i];
                                        dataGridView.Rows[i].Cells[11].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[12].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(45);
                                    break;
                                case "7-0.0.99.98.1.255-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[13].Value = Class[i];
                                        dataGridView.Rows[i].Cells[14].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[15].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(50);
                                    break;
                                case "7-0.0.99.98.2.255-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[16].Value = Class[i];
                                        dataGridView.Rows[i].Cells[17].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[18].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(55);
                                    break;
                                case "7-0.0.99.98.3.255-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[19].Value = Class[i];
                                        dataGridView.Rows[i].Cells[20].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[21].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(60);
                                    break;
                                case "7-0.0.99.98.4.255-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[22].Value = Class[i];
                                        dataGridView.Rows[i].Cells[23].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[24].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(65);
                                    break;
                                case "7-0.0.99.98.5.255-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[25].Value = Class[i];
                                        dataGridView.Rows[i].Cells[26].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[27].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(70);
                                    break;
                                case "7-0.0.99.98.6.255-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[28].Value = Class[i];
                                        dataGridView.Rows[i].Cells[29].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[30].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(75);
                                    break;
                                case "7-0.0.99.98.129.255-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[31].Value = Class[i];
                                        dataGridView.Rows[i].Cells[32].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[33].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(80);
                                    break;
                                case "7-1.0.94.91.7.255-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[34].Value = Class[i];
                                        dataGridView.Rows[i].Cells[35].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[36].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(83);
                                    break;
                                case "7-1.0.98.1.0.255-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[37].Value = Class[i];
                                        dataGridView.Rows[i].Cells[38].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[39].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(86);
                                    break;
                                case "7-1.0.94.91.6.255-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[40].Value = Class[i];
                                        dataGridView.Rows[i].Cells[41].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[42].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(89);
                                    break;
                                case "7-1.0.99.1.0.255-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[43].Value = Class[i];
                                        dataGridView.Rows[i].Cells[44].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[45].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(91);
                                    break;
                                case "7-1.0.94.91.4.255-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[46].Value = Class[i];
                                        dataGridView.Rows[i].Cells[47].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[48].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(93);
                                    break;
                                case "7-1.0.99.2.0.255-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[49].Value = Class[i];
                                        dataGridView.Rows[i].Cells[50].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[51].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(95);
                                    break;
                                case "7-1.0.94.91.5.255-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[52].Value = Class[i];
                                        dataGridView.Rows[i].Cells[53].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[54].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(97);
                                    break;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool ReadObjectList(DataTable dataTable, string meterDataRead, bool attributeDownload)
        {
            DLMSParser parse = new DLMSParser(dataGridView);
            // Add columns to the DataGridView
            dataGridView.DataSource = null;
            dataGridView.DataSource = dataTable;

            dataGridView.Columns[0].Width = 40;//SN
            dataGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView.Columns[1].Width = 70;//Class
            dataGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView.Columns[2].Width = 70;//Version
            dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView.Columns[3].Width = 100;//OBIS
            dataGridView.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView.Columns[4].Width = 350;
            dataGridView.Columns[5].Width = 200;//Attribute Access
            dataGridView.Columns[6].Width = 200;//Method Access

            for (int index = 0; index < dataTable.Rows.Count; ++index)
            {
                if (dataGridView.Rows[index].Cells[4].Value.ToString().Contains("Name not Available"))
                {
                    dataGridView.Rows[index].Cells[4].Style.BackColor = Color.Red;
                }
            }
            dataGridView.Refresh();
            #region Individual Data
            if (attributeDownload)
            {
                dataGridView.Columns.Add("SCALER", "SCALER");
                dataGridView.Columns[7].Width = 70;//Scaler
                dataGridView.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                int individualScalerColumnIndex = FindColumnIndexByName(dataGridView, "SCALER");

                string[] classArray = new string[dataGridView.Rows.Count];
                string[] obisArray = new string[dataGridView.Rows.Count];
                string[] classArrayInHEX = new string[dataGridView.Rows.Count];
                bool parameter = false;
                string tempRenameScaler = string.Empty;
                for (int index = 0; index < dataGridView.Rows.Count; index++)
                {
                    classArrayInHEX[index] = dataGridView.Rows[index].Cells[1].Value.ToString().Trim();
                    classArray[index] = int.Parse(classArrayInHEX[index], NumberStyles.HexNumber).ToString();
                    obisArray[index] = dataGridView.Rows[index].Cells[3].Value.ToString().Trim();
                    string[] strArray;
                    switch (classArray[index])
                    {
                        case "3":
                        case "4":
                        case "5":
                            ReadCOSEMObject((ObjectType)Convert.ToInt16(classArrayInHEX[index], 16), parse.HexObisToDecObis(obisArray[index]), 3);
                            if (string.IsNullOrEmpty(errorMessage))
                                dataGridView.Rows[index].Cells[7].Value = recData.Trim().Substring(4);
                            break;
                    }
                    tempRenameScaler = (string)dataGridView.Rows[index].Cells[individualScalerColumnIndex].Value;
                    if (!string.IsNullOrEmpty(tempRenameScaler))
                    {
                        if (tempRenameScaler.Substring(0, 2) == "0F")
                        {
                            if (!string.IsNullOrEmpty((string)parse.UnithshTable[tempRenameScaler.Substring(6, 2)]))
                            {
                                dataGridView.Rows[index].Cells[4].Value = dataGridView.Rows[index].Cells[4].Value + " ( " + (string)parse.UnithshTable[tempRenameScaler.Substring(6, 2)] + " )";
                            }
                        }
                    }
                }
            }
            #endregion
            if (dataGridView.Columns.Count > 4)
            {
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    if (column.Index < 5)
                        column.Frozen = true;
                }
            }
            dataGridView.Invalidate();
            return true;
        }
        public bool ReadObjectListAllAttributes()
        {
            DLMSParser parse = new DLMSParser(dataGridView);
            dataGridView.Columns.Add("Attribute-1 Data", "Attribute-1 Data");
            dataGridView.Columns.Add("Attribute-1 Value", "Attribute-1 Value");
            dataGridView.Columns.Add("Attribute-2 Data", "Attribute-2 Data");
            dataGridView.Columns.Add("Attribute-2 Value", "Attribute-2 Value");
            dataGridView.Columns.Add("Attribute-3 Data", "Attribute-3 Data");
            dataGridView.Columns.Add("Attribute-3 Value", "Attribute-3 Value");
            dataGridView.Columns.Add("Attribute-4 Data", "Attribute-4 Data");
            dataGridView.Columns.Add("Attribute-4 Value", "Attribute-4 Value");
            dataGridView.Columns.Add("Attribute-5 Data", "Attribute-5 Data");
            dataGridView.Columns.Add("Attribute-5 Value", "Attribute-5 Value");
            dataGridView.Columns.Add("Attribute-6 Data", "Attribute-6 Data");
            dataGridView.Columns.Add("Attribute-6 Value", "Attribute-6 Value");
            dataGridView.Columns.Add("Attribute-7 Data", "Attribute-7 Data");
            dataGridView.Columns.Add("Attribute-7 Value", "Attribute-7 Value");
            dataGridView.Columns.Add("Attribute-8 Data", "Attribute-8 Data");
            dataGridView.Columns.Add("Attribute-8 Value", "Attribute-8 Value");
            dataGridView.Columns.Add("Attribute-9 Data", "Attribute-9 Data");
            dataGridView.Columns.Add("Attribute-9 Value", "Attribute-9 Value");
            dataGridView.Columns.Add("Attribute-10 Data", "Attribute-10 Data");
            dataGridView.Columns.Add("Attribute-10 Value", "Attribute-10 Value");
            dataGridView.Columns.Add("Attribute-10 Data", "Attribute-11 Data");
            dataGridView.Columns.Add("Attribute-10 Value", "Attribute-11 Value");
            dataGridView.Refresh();
            int classColumnIndex = FindColumnIndexByName(dataGridView, "CLASS ID");
            int obisColumnIndex = FindColumnIndexByName(dataGridView, "OBIS");
            int accessColumnIndex = FindColumnIndexByName(dataGridView, "ATTRIBUTE ACCESS");
            string[] classArray = new string[dataGridView.Rows.Count];
            string[] classArrayInHEX = new string[dataGridView.Rows.Count];
            string[] obisArray = new string[dataGridView.Rows.Count];
            string[] accessArray = new string[dataGridView.Rows.Count];
            //if (!SignOnDLMS())
            //{
            //    CommonHelper.DisplayDLMSSignONError();
            //    PB_PGUpdate(100);
            //    return false;
            //}
            int attributeStartColumn = FindColumnIndexByName(dataGridView, "Attribute-1 Data");
            bool parameter = false;
            dataGridView.SuspendLayout();
            for (int index = 0; index < dataGridView.Rows.Count; index++)
            {
                classArrayInHEX[index] = dataGridView.Rows[index].Cells[1].Value.ToString().Trim();
                classArray[index] = int.Parse(classArrayInHEX[index], NumberStyles.HexNumber).ToString();
                obisArray[index] = dataGridView.Rows[index].Cells[3].Value.ToString().Trim();
                accessArray[index] = dataGridView.Rows[index].Cells[5].Value.ToString().Trim();
                string[] accessAttributeArray = accessArray[index].Split(' ');
                for (int i = 1; i <= accessAttributeArray.Length - 1; i++)
                {
                    if (classArray[index] == "7" && i == 2)
                    {
                        i++;
                        attributeStartColumn += 2;
                    }
                    else if (classArray[index] == "7" && i == 3)
                    {
                        i++;
                        attributeStartColumn += 2;
                    }
                    if (classArray[index] == "15" && i == 2)
                    {
                        i++;
                        attributeStartColumn += 2;
                    }
                    ReadCOSEMObject((ObjectType)Convert.ToInt16(classArrayInHEX[index], 16), parse.HexObisToDecObis(obisArray[index]), i);
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        string data = recData.Trim();
                        dataGridView.Rows[index].Cells[attributeStartColumn].Value = data;
                        if (data == "0B" || data == "0D")
                            dataGridView.Rows[index].Cells[attributeStartColumn].Style.BackColor = System.Drawing.Color.Green;
                        else
                        {
                            if (!accessAttributeArray[i].Trim().Contains("read"))
                                dataGridView.Rows[index].Cells[attributeStartColumn].Style.BackColor = System.Drawing.Color.Red;
                        }
                        string value = string.Empty;
                        if (obisArray[index] == "0000608000FF" || obisArray[index] == "0000608001FF" || obisArray[index] == "0100608000FF" || obisArray[index] == "0100608001FF")//LCD PArameters Auto Push
                        {
                            string tempString = string.Empty;
                            int nStart = 4;
                            while (nStart <= data.Substring(4).Length + 2)
                            {
                                tempString += int.Parse(data.Substring(nStart, 2), NumberStyles.HexNumber);
                                nStart += 2;
                                if (nStart <= data.Substring(4).Length + 2)
                                    tempString += ",";
                            }
                            value = tempString;
                            dataGridView.Rows[index].Cells[attributeStartColumn + 1].Value = value;
                        }
                        else
                        {
                            value = parse.GetProfileValueString(data);
                            dataGridView.Rows[index].Cells[attributeStartColumn + 1].Value = value;
                        }
                        if (string.IsNullOrEmpty(value))
                            dataGridView.Rows[index].Cells[attributeStartColumn + 1].Style.BackColor = System.Drawing.Color.Green;
                    }
                    else if (!string.IsNullOrEmpty(errorMessage))
                    {
                        dataGridView.Rows[index].Cells[attributeStartColumn].Value = errorMessage;
                    }
                    else
                    {
                        MessageBox.Show($"There is some error in getting data from Meter for Class - {index}  OBIS - {DLMSParser.GetObis(obisArray[index])} Attribute - {i}. Kindly Retry!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    attributeStartColumn += 2;
                }
                attributeStartColumn = FindColumnIndexByName(dataGridView, "Attribute-1 Data"); ;
            }
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView.ResumeLayout();
            return true;
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
            //DLMSParser DLMSparse = new DLMSParser();
            string recivedObisString = "", recivedValueString = "", recivedScalerObisString = "", recivedScalerValueString = "", inUseEntries = "", profileEntries = "", option = "";
            DataTable obisDataTable = new DataTable();
            DataTable resultDataTable = new DataTable();
            string[] scalerObisArray = null, scalerScalerDataArray = null, mainSourceObisArray = null, finalScalerValuesToFill = null, ScalerMultiFactorArray = null;
            try
            {
                SetGetFromMeter.Wait(10);
                //Get Profile Objects
                ReadCOSEMObject(ObjectType.ProfileGeneric, profileObis, 3);
                recivedObisString = recData;
                SetGetFromMeter.Wait(10);
                //Get Profile Values
                ReadCOSEMObject(ObjectType.ProfileGeneric, profileObis, 2);
                recivedValueString = recData;
                SetGetFromMeter.Wait(10);
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
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
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
                option = "Nameplate Profile";
                profileObis = "0.0.94.91.10.255";
                //scalerObis = string.Concat("1.0.94.91.3.255".Trim().Split('.').Select(part => int.Parse(part).ToString("X2")));
                #region Data Getting Logic
                SetGetFromMeter.Wait(10);
                //Get Profile Objects
                ReadCOSEMObject(ObjectType.ProfileGeneric, profileObis, 3);
                recivedObisString = recData;
                SetGetFromMeter.Wait(10);
                //Get Profile Values
                ReadCOSEMObject(ObjectType.ProfileGeneric, profileObis, 2);
                recivedValueString = recData;
                obisDataTable = DLMSparse.GetClassObisAttScalerListParsing(recivedObisString, option);
                #region Add a new column for serial numbers
                DataColumn snColumn = new DataColumn("SN", typeof(string));
                obisDataTable.Columns.Add(snColumn);
                // Set the new column to be the first column
                snColumn.SetOrdinal(0);
                // Populate the SN column with row numbers
                for (int i = 0; i < obisDataTable.Rows.Count; i++)
                {
                    obisDataTable.Rows[i]["SN"] = i + 1;
                }
                obisDataTable.AcceptChanges();
                #endregion

                resultDataTable = DLMSparse.GetBillingValuesDataTableParsing(recivedValueString, obisDataTable);
                //resultDataTable = parse.GetNameplateValuesDataTable(recivedValueString, obisDataTable);

                #endregion
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
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
            //DLMSParser DLMSparse = new DLMSParser();
            string recivedObjectString = "";
            DataTable obisDataTable = new DataTable();
            DataTable resultDataTable = new DataTable();
            try
            {
                SetGetFromMeter.Wait(10);
                //Get Profile Objects
                ReadCOSEMObject(ObjectType.AssociationLogicalName, associationOBIS, 2);
                recivedObjectString = recData;
                //resultDataTable = parse.GetObjectList(recivedObjectString);
                int obisCount = 0;
                resultDataTable = DLMSAssociationLN.GetObjectListTable(recivedObjectString, DLMSAssociationLN.AssociationType.Current_Association, out obisCount);
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
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
                        log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
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
                        log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
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
            recData = reply.ToString().Replace(" ", "");
            LineTrafficControlEventHandler($"Received Data:\r\n{reply.ToString()}\r\n", "Receive");
            return (object[])dlmsClient.UpdateValue(it, 2, reply.Value);
        }
        public void ReadProfileGenericBuffer(string profileObis, int _startIndex = 0, int _endIndex = 0, byte nType = 2, string _startDT = null, string _endDT = null)
        {
            LineTrafficControlEventHandler($"\r\n     GET CLASS-{7} | OBIS-{profileObis} [{DLMSParser.GetObisName((7).ToString(), profileObis, (2).ToString())}] | Attribute-{2}", "Send");
            GXReplyData reply = new GXReplyData();
            try
            {
                foreach (GXDLMSObject it in dlmsClient.Objects.GetObjects(ObjectType.ProfileGeneric))
                {
                    if (it.LogicalName == profileObis)
                    {
                        if (profileObis != "1.0.99.1.0.255" && profileObis != "1.0.99.2.0.255")//LS and DE
                        {
                            ReadRowsByEntry(it as GXDLMSProfileGeneric, Convert.ToUInt16(_startIndex), Convert.ToUInt16(_endIndex - _startIndex + 1));
                            break;
                        }
                        else
                        {
                            ReadRowsByRange(it as GXDLMSProfileGeneric, DateTime.ParseExact(_startDT, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), DateTime.ParseExact(_endDT, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture));
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");

            }
        }

        /// <summary>
        /// Read Profile Generic Columns by range.
        /// </summary>
        public object[] ReadRowsByRange(GXDLMSProfileGeneric it, DateTime start, DateTime end)
        {
            GXReplyData reply = new GXReplyData();
            ReadDataBlock(dlmsClient.ReadRowsByRange(it, start, end), reply);
            recData = reply.ToString().Replace(" ", "");
            LineTrafficControlEventHandler($"Received Data:\r\n{reply.ToString()}\r\n", "Receive");
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
                    LineTrafficControlEventHandler($"\r\n     GET CLASS-{7} | OBIS-{it.LogicalName} [{DLMSParser.GetObisName((7).ToString(), it.LogicalName, (3).ToString())}] | Attribute-{3}", "Send");
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
                            LineTrafficControlEventHandler($"\r\n     GET CLASS-{7} | OBIS-{it.LogicalName} [{DLMSParser.GetObisName((7).ToString(), it.LogicalName, (3).ToString())}] | Attribute-{3}", "Send");
                            Read(it, 3);
                        }
                        if (it is GXDLMSDemandRegister && dlmsClient.CanRead(it, 4))
                        {
                            Console.WriteLine(it.Name);
                            LineTrafficControlEventHandler($"\r\n     GET CLASS-{7} | OBIS-{it.LogicalName} [{DLMSParser.GetObisName((7).ToString(), it.LogicalName, (3).ToString())}] | Attribute-{3}", "Send");
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


        public int SetObjectValue(int _class, string _obis, int _attribute, string sData)
        {
            LineTrafficControlEventHandler($"\r\n     SET CLASS-{_class} | OBIS-{_obis} [{DLMSParser.GetObisName((_class).ToString(), _obis, (_attribute).ToString())}] | Attribute-{_attribute}", "Send");
            errorMessage = "";
            int nRetVal = 100;
            DataType dataType = DataType.String;
            object value = null;
            GXReplyData reply = new GXReplyData();
            GXDLMSObject gXDLMSObject = new GXDLMSObject();
            try
            {
                switch ((ObjectType)_class)
                {
                    //Class ID 1
                    case ObjectType.Data:
                        GXDLMSData gxdlmsData = new GXDLMSData(_obis);
                        gxdlmsData.SetDataType(_attribute, (DataType)Convert.ToInt32(sData.Substring(0, 2), 16));
                        gxdlmsData.SetAccess(_attribute, AccessMode.ReadWrite);
                        //gxdlmsData.SetUIDataType(_attribute, (DataType)Convert.ToInt32(sData.Substring(0, 2), 16));
                        switch ((DataType)Convert.ToInt32(sData.Substring(0, 2), 16))
                        {
                            case DataType.Array:
                                gxdlmsData.Value = (object)GXCommon.HexToBytes(sData);
                                break;
                            case DataType.Bcd:
                                gxdlmsData.Value = (object)GXCommon.HexToBytes(sData);
                                break;
                            case DataType.BitString:
                                int index = 4;
                                if (sData.Substring(2, 2) == "82")
                                    index = 8;
                                string binary = string.Concat(
                                                Enumerable.Range(0, sData.Substring(index).Length / 2)
                                                .Select(i => Convert.ToString(Convert.ToByte(sData.Substring(index).Substring(i * 2, 2), 16), 2).PadLeft(8, '0'))
                                                 );
                                GXBitString bitString = new GXBitString(binary);
                                gxdlmsData.Value = bitString;
                                break;
                            case DataType.Boolean:
                                gxdlmsData.Value = Convert.ToBoolean(sData.Substring(2));
                                break;
                            case DataType.CompactArray:
                                break;
                            case DataType.Date:
                                break;
                            case DataType.DateTime:
                                break;
                            case DataType.Enum:
                                gxdlmsData.Value = Convert.ToInt16(sData.Substring(2), 16);
                                break;
                            case DataType.Float32:
                                gxdlmsData.Value = UInt32.Parse(sData.Substring(2), NumberStyles.HexNumber);
                                break;
                            case DataType.Float64:
                                gxdlmsData.Value = UInt64.Parse(sData.Substring(2), NumberStyles.HexNumber);
                                break;
                            case DataType.Int16:
                                gxdlmsData.Value = Convert.ToInt16(sData.Substring(2), 16);
                                break;
                            case DataType.Int32:
                                gxdlmsData.Value = Convert.ToInt32(sData.Substring(2), 16);
                                break;
                            case DataType.Int64:
                                gxdlmsData.Value = Convert.ToInt64(sData.Substring(2), 16);
                                break;
                            case DataType.Int8:
                                gxdlmsData.Value = Convert.ToInt16(sData.Substring(2), 16);
                                break;
                            case DataType.None:
                                break;
                            case DataType.OctetString:
                                if (sData.Substring(0, 4) == "090C" && sData.Substring(sData.Length - 2) == "00")
                                {
                                    // Convert bytes to GXDateTime using DLMS type conversion
                                    GXDateTime newDateTime = (GXDateTime)GXDLMSClient.ChangeType(GXCommon.HexToBytes(sData.Substring(4)), DataType.DateTime);
                                    gxdlmsData.Value = newDateTime;
                                }
                                else
                                    gxdlmsData.Value = (object)GXCommon.HexToBytes(sData);
                                break;
                            case DataType.String:
                                gxdlmsData.Value = (object)GXCommon.HexToBytes(sData);
                                break;
                            case DataType.StringUTF8:
                                gxdlmsData.Value = (object)GXCommon.HexToBytes(sData);
                                break;
                            case DataType.Structure:
                                gxdlmsData.Value = GXCommon.HexToBytes(sData.Substring(0));
                                break;
                            case DataType.Time:
                                break;
                            case DataType.DeltaInt8:
                                break;
                            case DataType.DeltaInt16:
                                break;
                            case DataType.DeltaInt32:
                                break;
                            case DataType.DeltaUInt8:
                                break;
                            case DataType.DeltaUInt16:
                                break;
                            case DataType.DeltaUInt32:
                                break;
                            //12
                            case DataType.UInt16:
                                gxdlmsData.Value = Convert.ToUInt16(sData.Substring(2), 16);
                                break;
                            case DataType.UInt32:
                                gxdlmsData.Value = Convert.ToUInt32(sData.Substring(2), 16);
                                break;
                            case DataType.UInt64:
                                gxdlmsData.Value = Convert.ToUInt64(sData.Substring(2), 16);
                                break;
                            case DataType.UInt8:
                                gxdlmsData.Value = Convert.ToUInt16(sData.Substring(2), 16);
                                break;
                            default:
                                break;
                        }
                        //gxdlmsData.Value = (object)(GXCommon.HexToBytes(sData));
                        gXDLMSObject = (GXDLMSObject)gxdlmsData;
                        break;
                    //Class ID 8
                    case ObjectType.Clock:
                        GXDLMSClock gXDLMSClock = new GXDLMSClock(_obis);
                        gXDLMSClock.SetDataType(_attribute, (DataType)Convert.ToInt32(sData.Substring(0, 2), 16));
                        gXDLMSClock.SetAccess(_attribute, AccessMode.ReadWrite);
                        switch (_attribute)
                        {
                            case 2:
                                // Convert bytes to GXDateTime using DLMS type conversion
                                GXDateTime newDateTime = (GXDateTime)GXDLMSClient.ChangeType(GXCommon.HexToBytes(sData.Substring(4)), DataType.DateTime);
                                gXDLMSClock.Time = newDateTime;
                                break;
                            default:
                                gXDLMSObject = new GXDLMSObject();
                                break;
                        }
                        gXDLMSObject = (GXDLMSObject)gXDLMSClock;
                        break;
                    //Class ID 40
                    case ObjectType.PushSetup:
                        GXDLMSPushSetup gXDLMSPushSetup = new GXDLMSPushSetup(_obis);
                        gXDLMSPushSetup.SetDataType(_attribute, (DataType)Convert.ToInt32(sData.Substring(0, 2), 16));
                        gXDLMSPushSetup.SetAccess(_attribute, AccessMode.ReadWrite);
                        switch (_attribute)
                        {
                            case 3:
                                gXDLMSPushSetup.Service = ServiceType.Tcp;
                                gXDLMSPushSetup.Message = MessageType.CosemApdu;

                                if (sData.Substring(10, 2) == "00")
                                    gXDLMSPushSetup.Destination = "";
                                else
                                {
                                    int length = Convert.ToInt32(sData.Substring(10, 2), 16) * 2;
                                    gXDLMSPushSetup.Destination = parse.HexString2Ascii(sData.Substring(12, length));
                                }
                                //byte[] dataBytes = GXCommon.HexToBytes(sData);
                                break;
                            case 5:
                                gXDLMSPushSetup.RandomisationStartInterval = Convert.ToUInt16(sData.Substring(2), 16);
                                break;
                            default:
                                gXDLMSObject = new GXDLMSObject();
                                break;
                        }
                        gXDLMSObject = (GXDLMSObject)gXDLMSPushSetup;
                        break;
                    //CLass ID 22
                    case ObjectType.ActionSchedule:
                        GXDLMSActionSchedule actionSchedule = new GXDLMSActionSchedule(_obis);
                        actionSchedule.SetDataType(_attribute, (DataType)Convert.ToInt32(sData.Substring(0, 2), 16));
                        actionSchedule.SetAccess(_attribute, AccessMode.ReadWrite);
                        switch (_attribute)
                        {
                            case 4:
                                actionSchedule.ExecutionTime = WrapperParser.ParseSingleActionScheduleExecutionTimeHex(sData);// ParseExecutionTimeHex(sData);                   
                                break;
                            default:
                                gXDLMSObject = new GXDLMSObject();
                                break;
                        }
                        gXDLMSObject = (GXDLMSObject)actionSchedule;
                        break;
                    //Class ID 23
                    case ObjectType.IecHdlcSetup:
                        GXDLMSHdlcSetup gXDLMSHdlcSetup = new GXDLMSHdlcSetup(_obis);
                        gXDLMSHdlcSetup.SetDataType(_attribute, (DataType)Convert.ToInt32(sData.Substring(0, 2), 16));
                        gXDLMSHdlcSetup.SetAccess(_attribute, AccessMode.ReadWrite);
                        switch (_attribute)
                        {
                            case 9:
                                gXDLMSHdlcSetup.DeviceAddress = Convert.ToInt32(sData.Substring(2), 16);
                                break;
                            default:
                                break;
                        }
                        gXDLMSObject = (GXDLMSObject)gXDLMSHdlcSetup;
                        break;
                    //Class ID 45
                    case ObjectType.GprsSetup:
                        GXDLMSGprsSetup gXDLMSGprsSetup = new GXDLMSGprsSetup(_obis);
                        gXDLMSGprsSetup.SetDataType(_attribute, (DataType)Convert.ToInt32(sData.Substring(0, 2), 16));
                        gXDLMSGprsSetup.SetAccess(_attribute, AccessMode.ReadWrite);
                        switch (_attribute)
                        {
                            case 2:
                                if (sData.Substring(2, 2) == "00")
                                    gXDLMSGprsSetup.APN = "";
                                else
                                {
                                    gXDLMSGprsSetup.APN = parse.HexString2Ascii(sData.Substring(4));
                                }
                                break;
                            default:
                                gXDLMSObject = new GXDLMSObject();
                                break;
                        }
                        gXDLMSObject = (GXDLMSObject)gXDLMSGprsSetup;
                        break;
                    //Class ID 70
                    case ObjectType.DisconnectControl:
                        GXDLMSDisconnectControl gXDLMSDisconnectControl = new GXDLMSDisconnectControl(_obis);
                        gXDLMSDisconnectControl.SetDataType(_attribute, (DataType)Convert.ToInt32(sData.Substring(0, 2), 16));
                        gXDLMSDisconnectControl.SetAccess(_attribute, AccessMode.ReadWrite);
                        switch (_attribute)
                        {
                            case 4:
                                gXDLMSDisconnectControl.ControlMode = (ControlMode)(Convert.ToInt32(sData.Substring(2, 2), 16));
                                break;
                            default:
                                gXDLMSObject = new GXDLMSObject();
                                break;
                        }
                        gXDLMSObject = (GXDLMSObject)gXDLMSDisconnectControl;
                        break;
                    //Class ID 71
                    case ObjectType.Limiter:
                        GXDLMSLimiter gXDLMSLimiter = new GXDLMSLimiter(_obis);
                        gXDLMSLimiter.SetDataType(_attribute, (DataType)Convert.ToInt32(sData.Substring(0, 2), 16));
                        gXDLMSLimiter.SetAccess(_attribute, AccessMode.ReadWrite);
                        switch (_attribute)
                        {
                            case 4:
                                gXDLMSLimiter.ThresholdNormal = (object)GXCommon.HexToBytes(sData);
                                break;
                            default:
                                gXDLMSObject = new GXDLMSObject();
                                break;
                        }
                        gXDLMSObject = (GXDLMSObject)gXDLMSLimiter;
                        break;
                    // Class ID 20
                    case ObjectType.ActivityCalendar:
                        GXDLMSActivityCalendar gXDLMSActivityCalendar = new GXDLMSActivityCalendar(_obis);
                        gXDLMSActivityCalendar.SetDataType(_attribute, (DataType)Convert.ToInt32(sData.Substring(0, 2), 16));
                        gXDLMSActivityCalendar.SetAccess(_attribute, AccessMode.ReadWrite);
                        switch (_attribute)
                        {
                            case 6:
                                gXDLMSActivityCalendar.CalendarNamePassive = parse.HexString2Ascii(sData.Substring(4));
                                break;
                            case 7:
                                gXDLMSActivityCalendar.SeasonProfilePassive = parse.ParseSeasonProfile(sData);
                                break;
                            case 8:
                                //GXDLMSWeekProfile[] weekProfiles = (GXDLMSWeekProfile[])GXDLMSClient.ChangeType(
                                //                                        GXCommon.HexToBytes(sData),
                                //                                        DataType.Array
                                //                                        );
                                gXDLMSActivityCalendar.WeekProfileTablePassive = parse.ParseWeekProfile(sData);
                                break;
                            case 9:
                                //GXDLMSDayProfile[] dayProfiles = (GXDLMSDayProfile[])GXDLMSClient.ChangeType(
                                //                                        GXCommon.HexToBytes(sData),
                                //                                        DataType.Array
                                //                                        );
                                gXDLMSActivityCalendar.DayProfileTablePassive = parse.ParseDayProfile(sData);
                                break;
                            case 10:
                                GXDateTime newDateTime = (GXDateTime)GXDLMSClient.ChangeType(GXCommon.HexToBytes(sData.Substring(4)), DataType.DateTime);
                                gXDLMSActivityCalendar.Time = newDateTime;
                                break;
                            default:
                                gXDLMSObject = new GXDLMSObject();
                                break;
                        }
                        gXDLMSObject = (GXDLMSObject)gXDLMSActivityCalendar;
                        break;
                }
                if (!string.IsNullOrEmpty(gXDLMSObject.LogicalName))
                {
                    try
                    {
                        foreach (byte[] data in dlmsClient.Write(gXDLMSObject, _attribute))
                        {
                            reply.Clear();
                            SetGetFromMeter.Wait(10);
                            ReadDataBlock(data, reply);
                        }
                        recData = reply.ToString().Replace(" ", "");
                        if (reply.Error == 0)
                        {
                            nRetVal = 0;
                            errorMessage = "Set Successfully";
                        }
                        else
                        {
                            nRetVal = 1;
                            errorMessage = $"Error in Setting [{recData}] {reply.ErrorMessage}";
                        }
                    }
                    catch (Exception ex)
                    {
                        recData = reply.ToString().Replace(" ", "");
                        nRetVal = 1;
                        errorMessage = ex.Message.ToString();
                        //errorMessage = $"Error in Setting [{recData}] {reply.ErrorMessage} {ex.Message.ToString()}";
                    }
                }
                else
                {
                    recData = "";
                    nRetVal = 100;
                    errorMessage = $"Not Implemented";
                }
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
            }
            return nRetVal;
        }

        public bool SetAction(int _class, string _obis, int _method, string sData)
        {
            LineTrafficControlEventHandler($"\r\n     ACTION CLASS-{_class} | OBIS-{_obis} [{DLMSParser.GetObisName((_class).ToString(), _obis, (_method).ToString())}] | Method-{_method}", "Send");
            errorMessage = "";
            bool IsSuccess = false;
            GXReplyData reply = new GXReplyData();
            try
            {
                switch ((ObjectType)_class)
                {
                    //Class ID 20
                    case ObjectType.ActivityCalendar:
                        GXDLMSActivityCalendar gXDLMSActivityCalendar = new GXDLMSActivityCalendar(_obis);
                        gXDLMSActivityCalendar.SetMethodAccess(_method, MethodAccessMode.AuthenticatedAccess);
                        switch (_method)
                        {
                            case 1:
                                ReadDataBlock(dlmsClient.Method((GXDLMSObject)gXDLMSActivityCalendar, _method, 0, DataType.Int8), reply);
                                break;
                            default:
                                reply = new GXReplyData();
                                break;
                        }
                        break;
                    //Class ID 70
                    case ObjectType.DisconnectControl:
                        GXDLMSDisconnectControl gXDLMSDisconnectControl = new GXDLMSDisconnectControl(_obis);
                        gXDLMSDisconnectControl.SetMethodAccess(_method, MethodAccessMode.AuthenticatedAccess);
                        switch (_method)
                        {
                            case 1:
                            case 2:
                                if (sData.Length == 4)
                                    ReadDataBlock(dlmsClient.Method((GXDLMSObject)gXDLMSDisconnectControl, _method, Convert.ToInt16(sData.Substring(2), 16), DataType.Int8), reply);
                                else
                                    reply = new GXReplyData();
                                break;
                            default:
                                reply = new GXReplyData();
                                break;
                        }
                        break;
                    //Class ID 9
                    case ObjectType.ScriptTable:
                        GXDLMSScriptTable gXDLMSScriptTable = new GXDLMSScriptTable(_obis);
                        gXDLMSScriptTable.SetMethodAccess(_method, MethodAccessMode.AuthenticatedAccess);
                        switch (_method)
                        {
                            case 1:
                                if (sData.Length == 6)
                                    ReadDataBlock(dlmsClient.Method((GXDLMSObject)gXDLMSScriptTable, _method, Convert.ToUInt16(sData.Substring(2, 2), 16), DataType.UInt16), reply);
                                else
                                    reply = new GXReplyData();
                                break;
                            default:
                                reply = new GXReplyData();
                                break;
                        }
                        break;
                    default:
                        break;
                }
                if (reply.IsComplete && !(reply.SystemTitle is null))
                {
                    recData = reply.ToString().Replace(" ", "");
                    if (reply.Error == 0)
                    {
                        IsSuccess = true;
                        errorMessage = "Set Successfully";
                    }
                    else
                    {
                        IsSuccess = false;
                        errorMessage = $"Error in Setting [{recData}] {reply.ErrorMessage}";
                    }
                }
                else
                {
                    IsSuccess = false;
                    errorMessage = $"Not Implemented";
                }
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
            }
            return IsSuccess;

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
               log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
            }
            try
            {
                nicConfigData = CalculateFotaHexString(NICFOTA, ModuleFOTA);
            }
            catch (Exception ex)
            {
               log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
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
            //DLMSParser parse = new DLMSParser();
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
                    unitString = (string)DLMSparse.UnithshTable[unitString.Substring(6, 2)];
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
                //translator.AuthenticationKey = GXCommon.GetAsByteArray("NbPdClEkakNb04ab");
                translator.AuthenticationKey = GXCommon.GetAsByteArray($"{DLMSInfo.TxtEK}");
                //translator.BlockCipherKey = GXCommon.GetAsByteArray("NbPdClEkakNb04ab");
                translator.BlockCipherKey = GXCommon.GetAsByteArray($"{DLMSInfo.TxtEK}");
                translator.InvocationCounter = 0;
                //translator.DedicatedKey = GXCommon.GetAsByteArray("NbPdClEkakNb04ab");
                translator.DedicatedKey = GXCommon.GetAsByteArray($"{DLMSInfo.TxtEK}");
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
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
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
