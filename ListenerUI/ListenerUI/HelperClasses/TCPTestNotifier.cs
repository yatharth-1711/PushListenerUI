using Gurux.Common;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Secure;
using log4net;
using log4net.Util;
using MeterReader.TestHelperClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Data;
using Gurux.DLMS;
using Gurux.DLMS.Objects.Enums;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using MeterComm;
using AutoTest.FrameWork;
using MeterComm.DLMS;
using System.Globalization;
using AutoTest.FrameWork.Converts;
using MeterReader.DLMSNetSerialCommunication;
using meterReader.AesGcmParameter;
using Indali.Common;
using System.Runtime.InteropServices.ComTypes;
using System.IO;


namespace MeterReader.DLMSNetSerialCommunication
{
    public class TCPTestNotifier : IDisposable
    {
        #region Event Handlers
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly TestLogService _logService;
        public delegate void LineTrafficControl(string updatedText, string status);
        public static event LineTrafficControl LineTrafficControlEventHandler = delegate { };
        public delegate void LogControl(string updatedText, Color color);
        public static event LogControl LogControlEventHandler = delegate { };
        public delegate void LineTrafficSave();
        public static event LineTrafficSave LineTrafficSaveEventHandler = delegate { };
        DLMSParser parse = new DLMSParser();
        #endregion

        #region Objects and Global Variables
        //TestConfiguration _testConfiguration;
        TcpListener server;
        private Thread listenerThread;
        GXDLMSSecureClient dlmsClient;
        public static bool isListening = false;
        public string responseString = string.Empty;
        RichTextBox _logBox = new RichTextBox();
        #endregion


        #region PUSH PROCESSING GLOBAL VARIABLE
        public GXDLMSSecureClient client = new GXDLMSSecureClient();
        public string input_packet;
        public string[] Enc_packets;
        public string[] Dec_packets;
        public byte[] EK;
        public byte[] AK;
        public byte[] ST;
        public Gurux.DLMS.Enums.Security GXSecurity;
        public byte[] cipherText;
        public uint IC = 0;
        #endregion
        public static DataTable ListenerDataTable { get; set; } = new DataTable()
        {
            Columns =
            {
                new DataColumn("System Time Stamp", typeof(string)),
                new DataColumn("Meter RTC", typeof(string)),
                new DataColumn("Ciphered Data", typeof(string)),
                new DataColumn("Decoded Data", typeof(string)),
                new DataColumn("Decrypt Data", typeof(string)),
                new DataColumn("XML Data", typeof(string))
            }
        };
        public TCPTestNotifier() { }
        #region Listerner Connection
        public void Connect(RichTextBox logBox)
        {
            _logBox = logBox;
            // Initialize Secure DLMS Client
            dlmsClient = new Gurux.DLMS.Secure.GXDLMSSecureClient(
                true, // Logical Name referencing
                0,    // Client Address
                1,    // Server Address
                Gurux.DLMS.Enums.Authentication.High, // Choose correct authentication level
                $"{DLMSInfo.MeterAuthPasswordWrite}",          // Meter password if needed
                Gurux.DLMS.Enums.InterfaceType.WRAPPER // TCP/IP wrapper mode
            );
            // Configure Ciphering
            dlmsClient.Ciphering.SystemTitle = GXCommon.GetAsByteArray(DLMSInfo.TxtSysT);
            dlmsClient.Ciphering.Security = Gurux.DLMS.Enums.Security.AuthenticationEncryption;
            dlmsClient.Ciphering.BlockCipherKey = GXCommon.GetAsByteArray(DLMSInfo.TxtEK); ;
            dlmsClient.Ciphering.AuthenticationKey = GXCommon.GetAsByteArray(DLMSInfo.TxtAK);
            listenerThread = new Thread(StartServer);
            listenerThread.IsBackground = true;
            listenerThread.Start();
        }
        private void StartServer()
        {
            isListening = true;
            try
            {
                IPAddress ipv6Address = new IPAddress(IPAddress.Parse(NetworkHelper.GetIPv6Address()).GetAddressBytes());
                server = new TcpListener(ipv6Address, WrapperInfo.port);
                server.Start();

                while (isListening)
                {
                    using (TcpClient client = server.AcceptTcpClient())
                    using (NetworkStream stream = client.GetStream())
                    {
                        List<byte> receivedBytes = new List<byte>();
                        byte[] buffer = new byte[1024 * 2];
                        DateTime startTime = DateTime.Now;
                        try
                        {
                            while (true)
                            {
                                int bytesRead = 0;
                                try
                                {
                                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                                }
                                catch (IOException)
                                {
                                    break;
                                }
                                if (bytesRead > 0)
                                {
                                    receivedBytes.AddRange(buffer.Take(bytesRead));
                                    bool isLastBlock = (buffer[0] & 0x80) == 0x80;
                                    if (isLastBlock)
                                        break;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        catch (Exception exInner)
                        {
                            log.Error($"Stream read error: {exInner.Message}");
                        }
                        string typeOfData = string.Empty;
                        if (receivedBytes.Count > 0)
                        {
                            string timeStamp = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss:fff tt");
                            string recEncryptedHex = BitConverter.ToString(receivedBytes.ToArray()).Replace("-", " ");
                            // Decryption Logic
                            this.EK = Encoding.ASCII.GetBytes(DLMSInfo.TxtEK);
                            this.AK = Encoding.ASCII.GetBytes(DLMSInfo.TxtAK);
                            this.ST = Encoding.ASCII.GetBytes(DLMSInfo.TxtSysT);
                            this.GXSecurity = Security.AuthenticationEncryption;
                            string DecryptedData = null;
                            input_packet = recEncryptedHex.Replace(" ", "").ToUpper();
                            Enc_packets = input_packet.Split(new string[1] { "00010001" }, StringSplitOptions.None);
                            for (int index = 1; index < Enc_packets.Length; ++index)
                            {
                                Enc_packets[index] = "00010001" + Enc_packets[index];
                                cipherText = TSTCommon.HexToBytes(Find_DB08(Enc_packets[index]));
                                DecryptedData += DecryptData(true, EK, AK, ST, GXSecurity, IC, cipherText) + "\n";
                                //Console.WriteLine($"Decrypted value ==> {DecryptedData}");
                            }
                            // Push Type Identification
                            string decodedHex = DecodePushData(receivedBytes.ToArray(), receivedBytes.Count);
                            if (DecryptedData.Contains("00 00 19 09 00 FF")) typeOfData = "Instant Push";
                            else if (DecryptedData.Contains("00 04 19 09 00 FF")) typeOfData = "Alert Push";
                            else if (DecryptedData.Contains("00 05 19 09 00 FF")) typeOfData = "Load Survey Push";
                            else if (DecryptedData.Contains("00 06 19 09 00 FF")) typeOfData = "Daily Energy Push";
                            else if (DecryptedData.Contains("00 82 19 09 00 FF")) typeOfData = "Self Registration Push";
                            else if (DecryptedData.Contains("00 84 19 09 00 FF")) typeOfData = "Billing Push";
                            else if (DecryptedData.Contains("00 86 19 09 00 FF")) typeOfData = "Tamper Push";
                            else if (DecryptedData.Contains("00 00 19 09 81 FF")) typeOfData = "Current Bill Push";
                            else typeOfData = "Unknown Push Type";
                            string sData = DecryptedData.Replace(" ", "");
                            int counter = 10;
                            DateTime packetSendingTime = DateTime.ParseExact(parse.Getdate(sData.Substring(12, 24), 0, false), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                            counter = 36;
                            while (sData.Substring(counter, 2) != "02")
                            {
                                counter += 4;
                            }
                            counter += 4;//40
                            int lengthOfSerialNumber = Convert.ToInt32(sData.Substring(counter + 2, 2).ToString().Trim(), 16) * 2;
                            string DeviceID = parse.GetProfileValueString(sData.Substring(counter, lengthOfSerialNumber + 4).ToString()).ToString();
                            counter += lengthOfSerialNumber + 4;//66
                            int OBISLength = Convert.ToInt32(sData.Substring(counter + 2, 2).ToString().Trim(), 16) * 2;
                            counter += OBISLength + 4;
                            DateTime packetGenerateTime = DateTime.ParseExact(parse.Getdate(sData.Substring(counter + 4, 24), 0, false), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                            LogControlEventHandler($"------------------------------------------------------------------------------------------------------------------", Color.Green);
                            if (PushPacketManager.DeviceID == DeviceID)
                            {
                                // Logging
                                LogControlEventHandler($"     Device ID: {DeviceID}\t\t{timeStamp}: {typeOfData} Received", Color.Green);
                                LogControlEventHandler($"     Packet Generated in meter: {packetGenerateTime.ToString("dd/MM/yyyy hh:mm:ss tt")}\t\tPacket Sent by meter: {packetSendingTime.ToString("dd/MM/yyyy hh:mm:ss tt")}", Color.Blue);
                            }
                            else
                            {
                                LogControlEventHandler($"     Device ID: {DeviceID}\t\t{timeStamp}: {typeOfData} Received", Color.Red);
                                LogControlEventHandler($"     Packet Generated in meter: {packetGenerateTime.ToString("dd/MM/yyyy hh:mm:ss tt")}\t\tPacket Sent by meter: {packetSendingTime.ToString("dd/MM/yyyy hh:mm:ss tt")}", Color.Red);
                            }
                            //Line Traffic
                            LineTrafficControlEventHandler($"\n     Push Packet Received:  {typeOfData} {timeStamp}", "Send");
                            LineTrafficControlEventHandler($"     CIPHERED DATA", "Send");
                            LineTrafficControlEventHandler($"     {recEncryptedHex} {timeStamp}", "Receive");
                            string decodedXml = DecodePushDataToXML(receivedBytes.ToArray(), receivedBytes.Count);
                            ListenerDataTable.Rows.Add($"{timeStamp}-{typeOfData}-{DeviceID}", packetSendingTime.ToString("dd/MM/yyyy hh:mm:ss tt"), recEncryptedHex, decodedHex, DecryptedData, decodedXml);
                            ListenerDataTable.AcceptChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                log.Error(ex.StackTrace.ToString());
            }
        }
        public void StopServer()
        {
            if (server != null)
            {
                isListening = false;
                server.Stop();
                listenerThread?.Abort();
                //_logService.LogMessage(_logBox, $"Server stopped.", Color.Black);
            }
        }
        //BY YS
        /// <summary>
        /// Decrypt the ciphered data received from meter 
        /// </summary>
        /// <param name="Decrypt"></param>
        /// <param name="EK"></param>
        /// <param name="AK"></param>
        /// <param name="ST"></param>
        /// <param name="GXCIPH"></param>
        /// <param name="IC"></param>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string DecryptData(bool Decrypt, byte[] EK, byte[] AK, byte[] ST, Gurux.DLMS.Enums.Security GXCIPH, uint IC, byte[] cipherText)
        {
            string str;
            string reply = string.Empty;
            try
            {
                client.ClientAddress = 11;
                client.Authentication = Gurux.DLMS.Enums.Authentication.HighGMAC;
                client.Ciphering.BlockCipherKey = EK;
                client.Ciphering.AuthenticationKey = AK;
                client.Ciphering.SystemTitle = ST;
                client.Ciphering.Security = GXCIPH;
                client.UseLogicalNameReferencing = true;
                client.ServerAddress = 1;
                client.ServerAddressSize = (byte)1;
                Gurux.DLMS.GXByteBuffer data = new Gurux.DLMS.GXByteBuffer();
                data.Set(cipherText);
                AesGcmParameter p = new AesGcmParameter((byte)30, Security.AuthenticationEncryption, IC, client.Ciphering.SystemTitle, client.Ciphering.BlockCipherKey, client.Ciphering.AuthenticationKey);
                Gurux.DLMS.GXByteBuffer gxByteBuffer = new Gurux.DLMS.GXByteBuffer();
                byte[] numArray = !Decrypt ? GXDLMSCiphering.EncryptAesGcm(p, cipherText) : GXDLMSCiphering.DecryptAesGcm(p, data);
                gxByteBuffer.Set(numArray);
                str = gxByteBuffer.ToString();
            }
            catch
            {
                throw new Exception("Error in decryption");
            }
            return str;
        }

        /// <summary>
        /// Decode the ciphered data to get packet type and profile name 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private string DecodePushDataToXML(byte[] data, int length)
        {
            string dataXML = "";
            try
            {
                Gurux.DLMS.GXReplyData reply = new Gurux.DLMS.GXReplyData();
                // Decrypt and parse push message
                dlmsClient.GetData(data, reply);
                Gurux.DLMS.GXDLMSTranslator translator = new Gurux.DLMS.GXDLMSTranslator(TranslatorOutputType.SimpleXml);
                translator.Comments = true;
                translator.Security = Gurux.DLMS.Enums.Security.AuthenticationEncryption;
                translator.SecuritySuite = Gurux.DLMS.Objects.Enums.SecuritySuite.Suite0;
                translator.SystemTitle = GXCommon.GetAsByteArray(DLMSInfo.TxtSysT);
                translator.AuthenticationKey = GXCommon.GetAsByteArray(DLMSInfo.TxtEK);
                translator.BlockCipherKey = GXCommon.GetAsByteArray(DLMSInfo.TxtEK);
                translator.InvocationCounter = 0;
                translator.DedicatedKey = GXCommon.GetAsByteArray(DLMSInfo.TxtEK);
                //translator.PduOnly = true;

                Gurux.DLMS.GXByteBuffer bb = new Gurux.DLMS.GXByteBuffer();
                bb.Set(data);
                GXDLMSTranslatorMessage msg = new GXDLMSTranslatorMessage();
                msg.Message = bb;
                msg.InterfaceType = Gurux.DLMS.Enums.InterfaceType.WRAPPER;

                dataXML = translator.MessageToXml(bb.Data);
                translator.Clear();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                log.Error(ex.StackTrace.ToString());
                //_logService.LogMessage(_logBox, $"Push Data Decode Error: {ex.Message}", Color.Red);
            }
            return dataXML;
        }

        /// <summary>
        /// Decode push data to get type of data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private string DecodePushData(byte[] data, int length)
        {
            string dataHex = "";
            try
            {
                GXReplyData reply = new GXReplyData();

                // Decrypt and parse push message
                dlmsClient.GetData(data, reply);

                if (reply.Data is GXByteBuffer bufferData)
                {
                    byte[] dataFinal = bufferData.Data;
                    dataHex = GXCommon.ToHex(dataFinal, true);
                }
                else
                {
                    log.Error("Push Data could not be decoded.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"Push Data could not be decoded. {ex.Message.ToString()}");
            }
            return dataHex;
        }
        public string Find_DB08(string data)
        {
            for (int index = 0; index < data.Length - 4; ++index)
            {
                if (data[index] == 'D' && data[index + 1] == 'B' && data[index + 2] == '0' && data[index + 3] == '8')
                    return data.Substring(index);
            }
            return (string)null;
        }
        public void ClearPushDataTable()
        {
            if (ListenerDataTable.Rows.Count > 0)
                ListenerDataTable.Rows.Clear();
        }
        public void Dispose()
        {
            ClearPushDataTable();
            server.Server.Dispose();
            LineTrafficSaveEventHandler();
            server = null;
            listenerThread?.Abort();
        }
        #endregion
    }
}
