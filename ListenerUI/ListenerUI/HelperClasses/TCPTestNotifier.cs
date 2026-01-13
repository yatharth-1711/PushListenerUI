using AutoTest.FrameWork.Converts;
using Gurux.Common;
using Gurux.DLMS;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Secure;
using Indali.Common;
using ListenerUI.HelperClasses;
using log4net;
using MeterComm;
using MeterComm.DLMS;
using meterReader.AesGcmParameter;
using MeterReader.TestHelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;


namespace MeterReader.DLMSNetSerialCommunication
{
    public class TCPTestNotifier : IDisposable
    {
        #region Event Handlers
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public delegate void LineTrafficControl(string updatedText, string status);
        public static event LineTrafficControl LineTrafficControlEventHandler = delegate { };
        public delegate void LogControl(string updatedText, Color color);
        public static event LogControl LogControlEventHandler = delegate { };
        public delegate void AppendColoredTextControlWithBox(string message, Color color, bool isBold);
        public static event AppendColoredTextControlWithBox AppendColoredTextControlNotifier = delegate { };
        public delegate void LineTrafficSave();
        public static event LineTrafficSave LineTrafficSaveEventHandler = delegate { };
        DLMSParser parse = new DLMSParser();
        #endregion

        #region Objects and Global Variables
        TcpListener server;
        private Thread listenerThread;
        GXDLMSSecureClient dlmsClient;
        public static bool isListening = false;
        public static bool isUIHandler = false;
        public static bool showXMLData = false;
        public static bool showCipheredHexData = false;
        public static bool showDecryptedHexData = false;
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

        #region Separate Notification Global variables
        public static string notify_EK = string.Empty;
        public static string notify_AK = string.Empty;
        public static string notify_SysT = string.Empty;
        public static bool useSeparateCredentials = false;
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
            dlmsClient.Ciphering.Security = Gurux.DLMS.Enums.Security.AuthenticationEncryption;
            if (useSeparateCredentials)
            {
                dlmsClient.Ciphering.SystemTitle = GXCommon.GetAsByteArray(notify_SysT);
                dlmsClient.Ciphering.BlockCipherKey = GXCommon.GetAsByteArray(notify_EK);
                dlmsClient.Ciphering.AuthenticationKey = GXCommon.GetAsByteArray(notify_AK);
            }
            else
            {
                dlmsClient.Ciphering.SystemTitle = GXCommon.GetAsByteArray(DLMSInfo.TxtSysT);
                dlmsClient.Ciphering.BlockCipherKey = GXCommon.GetAsByteArray(DLMSInfo.TxtEK); ;
                dlmsClient.Ciphering.AuthenticationKey = GXCommon.GetAsByteArray(DLMSInfo.TxtAK);
            }
            listenerThread = new Thread(StartServer);
            listenerThread.IsBackground = true;
            listenerThread.Start();
        }
        private void StartServer()
        {
            isListening = true;
            try
            {
                if (useSeparateCredentials)
                {
                    this.EK = Encoding.ASCII.GetBytes(notify_EK);
                    this.AK = Encoding.ASCII.GetBytes(notify_AK);
                    this.ST = Encoding.ASCII.GetBytes(notify_SysT);
                }
                else
                {
                    this.EK = Encoding.ASCII.GetBytes(DLMSInfo.TxtEK);
                    this.AK = Encoding.ASCII.GetBytes(DLMSInfo.TxtAK);
                    this.ST = Encoding.ASCII.GetBytes(DLMSInfo.TxtSysT);
                }
                IPAddress ipv6Address = new IPAddress(IPAddress.Parse(NetworkHelper.GetIPv6Address()).GetAddressBytes());
                server = new TcpListener(ipv6Address, WrapperInfo.port);
                server.Start();

                while (isListening)
                {
                    #region AAC
                    TcpClient client = null;
                    try
                    {
                        client = server.AcceptTcpClient(); // Blocking call
                    }
                    catch (SocketException)
                    {
                        break; // Triggered by server.Stop()
                    }
                    if (!isListening)
                        break;
                    using (client)
                    #endregion
                    //using (TcpClient client = server.AcceptTcpClient())
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
                            // Decryption Logic
                            this.GXSecurity = Security.AuthenticationEncryption;
                            DateTime timeStamp = DateTime.Now;
                            DateTime packetSendingTime = DateTime.Now; DateTime packetGenerateTime = DateTime.Now;
                            string DecryptedData = null; string DeviceID = string.Empty; string decodedHex = string.Empty;

                            string recEncryptedHex = BitConverter.ToString(receivedBytes.ToArray()).Replace("-", " ");
                            input_packet = recEncryptedHex.Replace(" ", "").ToUpper();
                            Enc_packets = input_packet.Split(new string[1] { "00010001" }, StringSplitOptions.None);
                            for (int index = 1; index < Enc_packets.Length; ++index)
                            {
                                Enc_packets[index] = "00010001" + Enc_packets[index];
                                cipherText = TSTCommon.HexToBytes(Find_DB08(Enc_packets[index]));
                                DecryptedData += DecryptData(true, EK, AK, ST, GXSecurity, IC, cipherText) + "\n";
                                //Console.WriteLine($"Decrypted value ==> {DecryptedData}");
                            }
                            string decodedXml = DecodePushDataToXML(receivedBytes.ToArray(), receivedBytes.Count);
                            decodedHex = DecodePushData(receivedBytes.ToArray(), receivedBytes.Count);
                            if (!showCipheredHexData && !showDecryptedHexData && !showXMLData)
                            {
                                // Push Type Identification
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
                                packetSendingTime = DateTime.ParseExact(parse.Getdate(sData.Substring(12, 24), 0, false), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                counter = 36;
                                while (sData.Substring(counter, 2) != "02")
                                {
                                    counter += 4;
                                }
                                counter += 4;//40  
                                int lengthOfSerialNumber = Convert.ToInt32(sData.Substring(counter + 2, 2).ToString().Trim(), 16) * 2;
                                DeviceID = parse.GetProfileValueString(sData.Substring(counter, lengthOfSerialNumber + 4).ToString()).ToString();
                                counter += lengthOfSerialNumber + 4;//66
                                int OBISLength = Convert.ToInt32(sData.Substring(counter + 2, 2).ToString().Trim(), 16) * 2;
                                counter += OBISLength + 4;
                                packetGenerateTime = DateTime.ParseExact(parse.Getdate(sData.Substring(counter + 4, 24), 0, false), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                PrintPushHeader(DeviceID, typeOfData, packetGenerateTime, packetSendingTime, timeStamp.ToString("dd/MM/yyyy hh:mm:ss tt"), isUIHandler);
                            }
                            else
                            {
                                PrintMessage(isUIHandler, $"-----------------------------------------------------------------------------------------------------------", Color.Black, true);
                                PrintMessage(isUIHandler, $"Client Connected: {timeStamp.ToString(Constants.timeStamp12Hours)}", Color.Black, true);
                                if (showCipheredHexData)
                                {
                                    PrintMessage(isUIHandler, $"Encrypted Packet:", Color.Blue, true);
                                    PrintMessage(isUIHandler, $"{recEncryptedHex}" + Environment.NewLine, Color.Blue);
                                }
                                if (showDecryptedHexData)
                                {
                                    PrintMessage(isUIHandler, $"Decrypted Packet:", Color.Green, true);
                                    PrintMessage(isUIHandler, $"{DecryptedData}", Color.Green);
                                }
                                if (showXMLData)
                                {
                                    PrintMessage(isUIHandler, $"Decoded XML:", Color.Black, true);
                                    PrintMessage(isUIHandler, $"{decodedXml}" + Environment.NewLine, Color.Black);
                                }
                            }
                            ListenerDataTable.Rows.Add($"{timeStamp.ToString("dd/MM/yyyy hh:mm:ss tt")}-{typeOfData}-{DeviceID}", packetSendingTime.ToString("dd/MM/yyyy hh:mm:ss tt"), recEncryptedHex, decodedHex, DecryptedData, decodedXml);
                            ListenerDataTable.AcceptChanges();
                            //Line Traffic
                            LineTrafficControlEventHandler($"\n     Push Packet Received:  {typeOfData} {timeStamp}", "Send");
                            LineTrafficControlEventHandler($"     CIPHERED DATA", "Send");
                            LineTrafficControlEventHandler($"     {recEncryptedHex} {timeStamp}", "Receive");
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
        private void PrintMessage(bool isUIHandler, string message, Color color, bool isBold = false)
        {
            if (isUIHandler)
                AppendColoredTextControlNotifier(message, color, isBold);
            else
                LogControlEventHandler(message, color);
        }
        public void PrintPushHeader(string DeviceID, string typeOfData, DateTime packetGenerateTime, DateTime packetSendingTime, string timeStamp, bool isUIHandler)
        {
            bool isMatched = PushPacketManager.DeviceID == DeviceID;
            Color headerColor = Color.Green;
            Color detailColor = isMatched ? Color.Green : Color.Red;
            PrintMessage(isUIHandler, "------------------------------------------------------------------------------------------------------------------", headerColor);
            PrintMessage(isUIHandler, $"\t{string.Format("{0,-28}{1,-27}{2,-60}", $"Device ID: {DeviceID}", $"{timeStamp}", $"{typeOfData} Received")}", detailColor, true);
            string formattedMessage = $"\t{string.Format("{0,-28}{1,-27}{2,-23}{3,-37}", $"Packet Generated in meter:", $"{packetGenerateTime.ToString(Constants.timeStamp12Hours)}", $"Packet Sent by meter:", $"{packetGenerateTime.ToString(Constants.timeStamp12Hours)}")}";
            PrintMessage(isUIHandler, formattedMessage, isMatched ? Color.Blue : Color.Red, true);
        }
        public void StopServer()
        {
            try
            {
                isListening = false;

                if (server != null)
                {
                    server.Stop();  // This will force AcceptTcpClient() to return
                }

                if (listenerThread != null && listenerThread.IsAlive)
                {
                    listenerThread.Join(1000); // wait for thread to exit
                }
            }
            catch (Exception ex)
            {
                log.Error("Error during shutdown: " + ex.Message);
            }

            /*
            if (server != null)
            {
                isListening = false;
                server.Stop();
                listenerThread?.Abort();
                //_logService.LogMessage(_logBox, $"Server stopped.", Color.Black);
            }*/
        }
        private string GetDecryptData(string XmlData)
        {
            string decryptData = "";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<root>" + $@"{XmlData}" + "</root>"); // Wrapping in a root node for valid XML

            XmlNode blockDataNode = xmlDoc.SelectSingleNode("//BlockData[@Value]");
            if (blockDataNode != null)
            {
                decryptData = blockDataNode.Attributes["Value"].Value.Trim();
                return decryptData;
            }
            foreach (XmlNode node in xmlDoc.DocumentElement.SelectNodes("//comment()"))
            {
                if (node.NodeType == XmlNodeType.Comment)
                {
                    string commentText = node.Value;
                    Match match = null;
                    if (XmlData.Contains("<GeneralBlockTransfer>"))
                    {
                        match = Regex.Match(commentText, @"BlockData Value=\""([^\""]+)\""", RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
                    }
                    else
                        match = Regex.Match(commentText, @"Decrypt data:\s*([\dA-F ]+)", RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        decryptData = match.Groups[1].Value.Trim();
                        break;
                    }
                }
            }
            return decryptData;

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
        public string DecodePushDataToXML(byte[] data, int length)
        {
            string dataXML = "";
            try
            {
                Gurux.DLMS.GXReplyData reply = new Gurux.DLMS.GXReplyData();
                // Decrypt and parse push message
                //dlmsClient.GetData(data, reply);
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
                //dlmsClient.GetData(data, reply);
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