using AutoTestDesktopWFA;
using log4net;
using MeterComm.DLMS;
using MeterReader.DLMSNetSerialCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MeterReader.TestHelperClasses
{
    [Serializable]
    public class TestConfiguration
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #region Main Window Settings
        /// <summary>
        /// Hold the Com - Port for the Meter communication 
        /// </summary>
        public string comPort { get; set; } = "COM4";
        /// <summary>
        /// Hold the Baud Rate for the Meter communication 
        /// </summary>
        public int BaudRate { get; set; } = 9600;
        /// <summary>
        /// Access Mode 
        /// 0=PC
        /// 1=MR
        /// 2=US
        /// 3=PUSH
        /// </summary>
        public int AccessMode { get; set; } = 1;
        /// <summary>
        ///Read Password
        /// </summary>
        public string MeterAuthPassword { get; set; } = "1A2B3C4D";
        /// <summary>
        ///Write Password
        /// </summary>
        public string MeterAuthPasswordWrite { get; set; } = "RsEbHlSugjV97abc";
        /// <summary>
        ///Write Password FW
        /// </summary>
        public string FWMeterAuthPasswordWrite { get; set; } = "RsEbHlSfgjV97abc";
        /// <summary>
        /// Addressing Mode => One Byte / Four Byte
        /// </summary>
        public string AddressModeText { get; set; } = "One Byte";
        /// <summary>
        ///Logical Addrees 
        /// </summary>
        public string LogicalAddress { get; set; } = "1";
        /// <summary>
        ///Logical Addrees 
        /// </summary>
        public string PhysicalAddress { get; set; } = "256";
        /// <summary>
        ///CmBDirect in smart Automation index based 
        ///0=1
        ///1=16
        /// </summary>
        public int CmbDirect { get; set; } = 1;
        public string Parity { get; set; } = "None";
        public int DataBit { get; set; } = 8;
        public string StopBit { get; set; } = "One";
        #endregion

        #region SNRM Information
        /// <summary>
        ///  Window Size Tx
        /// </summary>
        public string WSTx { get; set; } = "";
        /// <summary>
        ///  Window Size Rx
        /// </summary>
        public string WSRx { get; set; } = "";
        /// <summary>
        ///  Information Field Tx
        ///  Default
        ///  112
        ///  128
        ///  256
        ///  512
        ///  1024
        ///  2048
        /// </summary>
        public string IFTx { get; set; } = "Default";
        /// <summary>
        ///  Information Field Rx
        /// </summary>
        public string IFRx { get; set; } = "Default";
        #endregion

        #region AARQ Information
        /// <summary>
        /// LN with Cipher
        /// </summary>
        public bool IsLNWithCipher { get; set; } = false;
        /// <summary>
        /// with GMAC
        /// </summary>
        public bool IsWithGMAC { get; set; } = false;
        /// <summary>
        /// LN with Cipher Dedicated Key
        /// </summary>
        public bool IsLNWithCipherDedicatedKey { get; set; } = false;
        /// <summary>
        /// with Invocation Counter
        /// </summary>
        public bool IsWithInvocationCounter { get; set; } = false;
        /// <summary>
        /// Encryption Key
        /// </summary>
        public string TxtEK { get; set; } = "RsEbEkAkgjV97abc";
        /// <summary>
        /// Authentication Key
        /// </summary>
        public string TxtAK { get; set; } = "RsEbEkAkgjV97abc";
        /// <summary>
        /// System Title
        /// </summary>
        public string TxtSysT { get; set; } = "GOE00000";
        /// <summary>
        /// Master Key
        /// </summary>
        public string MasterKey { get; set; } = "GeNuSmAsteRkEy25";
        /// <summary>
        /// Conformance Block
        /// </summary>
        public string ConformanceBlock { get; set; } = "FFFFF";
        #endregion

        #region Extra
        /// <summary>
        /// 
        /// </summary>
        public byte NWait = 0;
        /// <summary>
        /// 
        /// </summary>
        public byte NTryCount = 3;
        /// <summary>
        /// 
        /// </summary>
        public byte NTimeOut = 3;
        #endregion

        #region CTI File Parameters
        public long InactivityTimeout = 120000;
        public long InterFrameTimeout = 1000;
        public long ResponseTimeout = 2000;
        public long DISCToNDMTimeout = 2500;
        #endregion

        #region Network Info
        public int clientAddress { get; set; } = 16;
        public int serverAddress { get; set; } = 1;
        public string hostName { get; set; } = "2401:4900:983a:aad4::2";
        public int port { get; set; } = 4059;
        public string ModuleType { get; set; } = "";
        #endregion

        #region Reference Project Info
        /// <summary>
        ///Read Password
        /// </summary>
        public string RefMeterAuthPassword { get; set; } = "1A2B3C4D";
        /// <summary>
        ///Write Password
        /// </summary>
        public string RefMeterAuthPasswordWrite { get; set; } = "dlmspassword1234";
        /// <summary>
        ///Write Password FW
        /// </summary>
        public string RefFWMeterAuthPasswordWrite { get; set; } = "dlmspassword1234";
        /// <summary>
        /// Encryption Key
        /// </summary>
        public string RefTxtEK { get; set; } = "NbPdClHlsuNb04ab";
        /// <summary>
        /// Authentication Key
        /// </summary>
        public string RefTxtAK { get; set; } = "NbPdClHlsuNb04ab";
        /// <summary>
        /// Master Key
        /// </summary>
        public string RefMasterKey { get; set; } = "NbPdClHlsuNb04ab";

        #endregion

        // Method to load configuration
        public static TestConfiguration Load()
        {
            // In real-world, this would read from JSON/XML config
            return new TestConfiguration
            {
                #region Serial Info
                comPort = "COM4",
                BaudRate = 9600,
                AccessMode = 2,
                MeterAuthPassword = "1A2B3C4D",
                MeterAuthPasswordWrite = "dlmspassword1234",
                FWMeterAuthPasswordWrite = "dlmspassword1234",
                AddressModeText = "One Byte",
                LogicalAddress = "1",
                PhysicalAddress = "256",
                CmbDirect = DLMSInfo.CmbDirect,
                Parity = "None",
                DataBit = 8,
                StopBit = "One",

                WSTx = "1",
                WSRx = "1",
                IFTx = "Default",
                IFRx = "Default",

                IsLNWithCipher = false,
                IsWithGMAC = false,
                IsLNWithCipherDedicatedKey = false,
                IsWithInvocationCounter = false,
                TxtEK = "NbPdClHlsuNb04ab",
                TxtAK = "NbPdClHlsuNb04ab",
                TxtSysT = "GOE00000",
                MasterKey = "NbPdClHlsuNb04ab",
                ConformanceBlock = "FFFFFF",

                NWait = 0,
                NTryCount = 3,
                NTimeOut = 3,
                #endregion

                clientAddress = 16,
                serverAddress = 1,
                hostName = "2401:4900:983a:aad4::2",
                port = 4059,
                ModuleType = "Genus",

                RefMeterAuthPassword = "1A2B3C4D",
                RefMeterAuthPasswordWrite = "dlmspassword1234",
                RefFWMeterAuthPasswordWrite = "dlmspassword1234",
                RefTxtEK = "NbPdClHlsuNb04ab",
                RefTxtAK = "NbPdClHlsuNb04ab",
                RefMasterKey = "NbPdClHlsuNb04ab",
            };
        }
        public static TestConfiguration CreateDefault()
        {
            return new TestConfiguration
            {
                // Initialize with values from DLMSInfo
                comPort = DLMSInfo.comPort,
                BaudRate = DLMSInfo.BaudRate,
                AccessMode = DLMSInfo.AccessMode,
                MeterAuthPassword = DLMSInfo.MeterAuthPassword,
                MeterAuthPasswordWrite = DLMSInfo.MeterAuthPasswordWrite,
                FWMeterAuthPasswordWrite = DLMSInfo.MeterAuthPasswordWrite,
                AddressModeText = DLMSInfo.AddressModeText,
                LogicalAddress = DLMSInfo.LogicalAddress,
                PhysicalAddress = DLMSInfo.PhysicalAddress,
                CmbDirect = DLMSInfo.CmbDirect,
                Parity = DLMSInfo.Parity,
                DataBit = DLMSInfo.DataBit,
                StopBit = DLMSInfo.StopBit,

                // SNRM values
                WSTx = DLMSInfo.WSTx,
                WSRx = DLMSInfo.WSRx,
                IFTx = DLMSInfo.IFTx,
                IFRx = DLMSInfo.IFRx,

                // AARQ values
                IsLNWithCipher = DLMSInfo.IsLNWithCipher,
                IsWithGMAC = DLMSInfo.IsWithGMAC,
                IsLNWithCipherDedicatedKey = DLMSInfo.IsLNWithCipherDedicatedKey,
                IsWithInvocationCounter = DLMSInfo.IsWithInvocationCounter,

                TxtEK = DLMSInfo.TxtEK,
                TxtAK = DLMSInfo.TxtAK,
                TxtSysT = DLMSInfo.TxtSysT,
                MasterKey = DLMSInfo.MasterKey,
                ConformanceBlock = DLMSInfo.ConformanceBlock,

                // Extra values
                NWait = DLMSInfo.NWait,
                NTryCount = DLMSInfo.NTryCount,
                NTimeOut = DLMSInfo.NTimeOut,

                //WRAPPER
                clientAddress = WrapperInfo.clientAddress,
                serverAddress = WrapperInfo.serverAddress,
                hostName = WrapperInfo.hostName,
                port = WrapperInfo.port,
                ModuleType = WrapperInfo.ModuleType,

                //Reference Project Info
                RefMeterAuthPassword = DLMSInfo.MeterAuthPassword,
                RefMeterAuthPasswordWrite = DLMSInfo.MeterAuthPasswordWrite,
                RefFWMeterAuthPasswordWrite = DLMSInfo.MeterAuthPasswordWrite,
                RefTxtEK = DLMSInfo.TxtEK,
                RefTxtAK = DLMSInfo.TxtAK,
                RefMasterKey = "NbPdClHlsuNb04ab",

                InactivityTimeout = 120000,
                InterFrameTimeout = 1000,
                ResponseTimeout = 2000,
                DISCToNDMTimeout = 2000,
            };
        }

        public void ApplyTestConfiguration()
        {
            // Initialize with values from DLMSInfo
            SessionGlobalMeterCommunication.comPort = DLMSInfo.comPort = comPort;
            SessionGlobalMeterCommunication.BaudRate = DLMSInfo.BaudRate = BaudRate;
            SessionGlobalMeterCommunication.AccessMode = DLMSInfo.AccessMode = AccessMode;
            SessionGlobalMeterCommunication.MeterAuthPassword = DLMSInfo.MeterAuthPassword = MeterAuthPassword;
            SessionGlobalMeterCommunication.AccessMode = DLMSInfo.AccessMode;
            if (DLMSInfo.AccessMode == 1)
                SessionGlobalMeterCommunication.MeterAuthPasswordWrite = DLMSInfo.MeterAuthPasswordWrite = MeterAuthPassword;
            else if (DLMSInfo.AccessMode == 4)
                SessionGlobalMeterCommunication.MeterAuthPasswordWrite = DLMSInfo.MeterAuthPasswordWrite = FWMeterAuthPasswordWrite;
            else
                SessionGlobalMeterCommunication.MeterAuthPasswordWrite = DLMSInfo.MeterAuthPasswordWrite = MeterAuthPasswordWrite;
            SessionGlobalMeterCommunication.AddressModeText = DLMSInfo.AddressModeText = AddressModeText;
            SessionGlobalMeterCommunication.LogicalAddress = DLMSInfo.LogicalAddress = LogicalAddress;
            DLMSInfo.PhysicalAddress = PhysicalAddress;
            DLMSInfo.CmbDirect = CmbDirect;
            DLMSInfo.Parity = Parity;
            DLMSInfo.DataBit = DataBit;
            DLMSInfo.StopBit = StopBit;

            // SNRM values
            DLMSInfo.WSTx = WSTx;
            DLMSInfo.WSRx = WSRx;
            DLMSInfo.IFTx = IFTx;
            DLMSInfo.IFRx = IFRx;

            // AARQ values
            DLMSInfo.IsLNWithCipher = IsLNWithCipher;
            DLMSInfo.IsWithGMAC = IsWithGMAC;
            DLMSInfo.IsLNWithCipherDedicatedKey = IsLNWithCipherDedicatedKey;
            DLMSInfo.IsWithInvocationCounter = IsWithInvocationCounter;

            DLMSInfo.TxtEK = TxtEK;
            DLMSInfo.TxtAK = TxtAK;
            DLMSInfo.TxtSysT = TxtSysT;
            DLMSInfo.MasterKey = MasterKey;

            DLMSInfo.ConformanceBlock = ConformanceBlock;

            // Extra values
            DLMSInfo.NWait = NWait;
            DLMSInfo.NTryCount = NTryCount;
            DLMSInfo.NTimeOut = NTimeOut;

            WrapperInfo.clientAddress = clientAddress;
            WrapperInfo.serverAddress = serverAddress;
            WrapperInfo.hostName = hostName;
            WrapperInfo.port = port;
            WrapperInfo.ModuleType = ModuleType;
        }

        public TestConfiguration Clone()
        {
            TestConfiguration clone = (TestConfiguration)MemberwiseClone();
            return clone;
        }

    }
}
