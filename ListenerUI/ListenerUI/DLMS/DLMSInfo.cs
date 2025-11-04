using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterComm.DLMS
{
    public static class DLMSInfo
    {
        #region Main Window Settings
        /// <summary>
        /// Hold the Com - Port for the Meter communication 
        /// </summary>
        public static string comPort = "COM3";
        /// <summary>
        /// Hold the Baud Rate for the Meter communication 
        /// </summary>
        public static int BaudRate = 9600;
        /// <summary>
        /// Access Mode 
        /// 0=PC
        /// 1=MR
        /// 2=US
        /// 3=PUSH
        /// </summary>
        public static int AccessMode = 2;
        /// <summary>
        ///Read Password
        /// </summary>
        public static string MeterAuthPassword = "1A2B3C4D";
        /// <summary>
        ///Write Password
        /// </summary>
        public static string MeterAuthPasswordWrite = "CsPdClHlsuG1946a";
        /// <summary>
        /// Addressing Mode => One Byte / Four Byte
        /// </summary>
        public static string AddressModeText = "One Byte";
        /// <summary>
        ///Logical Addrees 
        /// </summary>
        public static string LogicalAddress = "1";
        /// <summary>
        ///Logical Addrees 
        /// </summary>
        public static string PhysicalAddress = "";
        /// <summary>
        ///CmBDirect in smart Automation index based 
        ///0=1
        ///1=16
        /// </summary>
        public static int CmbDirect = 1;
        /// <summary>
        /// Parity Bit for COM Connection
        /// </summary>
        public static string Parity = "None";
        /// <summary>
        /// Data Bit for COM Connection
        /// </summary>
        public static int DataBit = 8;
        /// <summary>
        /// Stop Bit for COM Connection
        /// </summary>
        public static string StopBit = "None";
        #endregion

        #region SNRM Information
        /// <summary>
        ///  Window Size Tx
        /// </summary>
        public static string WSTx = "1";
        /// <summary>
        ///  Window Size Rx
        /// </summary>
        public static string WSRx = "1";
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
        public static string IFTx = "Default";
        /// <summary>
        ///  Information Field Rx
        /// </summary>
        public static string IFRx = "Default";
        #endregion

        #region AARQ Information
        /// <summary>
        /// LN with Cipher
        /// </summary>
        public static bool IsLNWithCipher = true;
        /// <summary>
        /// with GMAC
        /// </summary>
        public static bool IsWithGMAC = false;
        /// <summary>
        /// LN with Cipher Dedicated Key
        /// </summary>
        public static bool IsLNWithCipherDedicatedKey = false;
        /// <summary>
        /// with Invocation Counter
        /// </summary>
        public static bool IsWithInvocationCounter = false;
        /// <summary>
        /// Encryption Key
        /// </summary>
        public static string TxtEK = "CsPdClEkaKG1946a";
        /// <summary>
        /// Authentication Key
        /// </summary>
        public static string TxtAK = "CsPdClEkaKG1946a";
        /// <summary>
        /// System Title
        /// </summary>
        public static string TxtSysT = "GOE00000";

        /// <summary>
        /// Default Propose all Services "62FEDF"
        /// </summary>
        public static string ConformanceBlock = "62FEDF";

        /// <summary>
        /// Master Key for Global Key Change
        /// </summary>
        public static string MasterKey = "";
        #endregion

        #region Extra
        /// <summary>
        /// 
        /// </summary>
        public static byte NWait = 0;
        /// <summary>
        /// 
        /// </summary>
        public static byte NTryCount = 5;
        /// <summary>
        /// 
        /// </summary>
        public static byte NTimeOut = 10;
        #endregion

        #region Net Serial Communication
        public static bool Connectionsucceed = false;
        public static bool MediaConnected = false;
        public static bool IsGXNetclicked = false;
        public static bool TransactionVerificationstatus = false;
        public static bool TransactionVerificationProcess = false;
        public static bool TransactionVerificationcancelled = false;
        public static bool IsModuleFotaToBeSet = false;
        public static bool IsNICFOTAToBeSet = false;
        #endregion
    }
}
