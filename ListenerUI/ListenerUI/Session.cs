using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestDesktopWFA
{
    public static class Session
    {
        /// <summary>
        /// Hold the user 
        /// </summary>
        public static int userId = 134;
        /// <summary>
        /// Hold the user Name
        /// </summary>
        public static string userName = "";
        /// <summary>
        /// Hold the user Email
        /// </summary>
        public static string userEmail = "";
        /// <summary>
        /// Hold that user is Admin
        /// </summary>
        //public static bool IsAdmin { get; set; }
        public static bool IsAdmin = false;
        /// <summary>
        /// Hold that user is Admin
        /// </summary>
        //public static bool IsAdmin { get; set; }
        public static bool IsActive = true;
        /// <summary>
        /// Hold that user is Admin
        /// </summary>
        //public static bool IsAdmin { get; set; }
        public static string Password = "Admin@123";
    }

    /// <summary>
    /// This will be the connection changes for PowerSource Communication
    /// </summary>
    public static class SessionPowerSourceCommunication
    {
        /// <summary>
        /// Hold the Com - Port for the power Source communication 
        /// </summary>
        public static string comPort = "";
        /// <summary>
        /// Hold the Baud Rate for the power Source communication 
        /// </summary>
        public static int BaudRate = 0;
        /// <summary>
        ///Only Default Calibration FileName May be Set later
        /// </summary>
        public static string CalibrationFileName = null;
        /// <summary>
        /// 
        /// </summary>
        public static double V1Scale;
        /// <summary>
        /// 
        /// </summary>
        public static double V2Scale;
        /// <summary>
        /// 
        /// </summary>
        public static double V3Scale;
        /// <summary>
        /// 
        /// </summary>
        public static double I1Scale;
        /// <summary>
        /// 
        /// </summary>
        public static double I2Scale;
        /// <summary>
        /// 
        /// </summary>
        public static double I3Scale;
        /// <summary>
        /// 
        /// </summary>
        public static double ANGERR1;
        /// <summary>
        /// 
        /// </summary>
        public static double ANGERR2;
        /// <summary>
        /// 
        /// </summary>
        public static double ANGERR3;
        /// <summary>
        /// 
        /// </summary>
        public static double ANGELU1U2;
        /// <summary>
        /// 
        /// </summary>
        public static double ANGELU1U3;
        /// <summary>
        /// 
        /// </summary>
        public static double Frequency;
    }

    /// <summary>
    /// This will be the connection changes for PowerSource Communication
    /// </summary>
    public static class SessionGlobalMeterCommunication
    {
        /// <summary>
        /// Hold the Com - Port for the power Source communication 
        /// </summary>
        public static string comPort = "COM4";
        /// <summary>
        /// Hold the Baud Rate for the power Source communication 
        /// </summary>
        public static int BaudRate = 9600;
        /// <summary>
        ///Only Default Calibration FileName May be Set later
        /// </summary>
        public static string MeterAuthPassword = "1A2B3C4D";
        /// <summary>
        ///Only Default Calibration FileName May be Set later
        /// </summary>
        public static string MeterAuthPasswordWrite = "dlmspassword1234";
        /// <summary>
        /// 
        /// </summary>
        public static string AddressModeText = "";
        /// <summary>
        /// 
        /// </summary>
        public static string LogicalAddress = "1";
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
        /// <summary>
        /// 
        /// </summary>
        public static int AccessMode = 1;
        /// <summary>
        /// 
        /// </summary>
        public static bool IsLNWithCipher = false;
        /// <summary>
        /// 
        /// </summary>
        public static bool IsWithGMAC = false;
        /// <summary>
        /// 
        /// </summary>
        public static bool IsLNwithCipherDedicatedKey = false;
        /// <summary>
        /// 
        /// </summary>
        public static string TxtEK = "";
        /// <summary>
        /// 
        /// </summary>
        public static string TxtAK = "";
        /// <summary>
        /// 
        /// </summary>
        public static string TxtSysT = "";


        //this contains the status of is meter is connected
        public static bool IsMeterConnected = false;

    }

    /// <summary>
    /// This will be the connection changes for PowerSource Communication
    /// </summary>
    public static class SessionGlobalMeterRatings
    {
        /// <summary>
        /// voltageRef
        /// </summary>
        public static string voltageRef = "0";
        /// <summary>
        /// PhaseType
        /// </summary>
        public static string PhaseType = "3P";
        /// <summary>
        ///Type
        /// </summary>
        public static string Type = "CT";
        /// <summary>
        ///CT value
        /// </summary>
        public static string ctValue = "1";
        /// <summary>
        ///Wc value
        /// </summary>
        public static string wcValue = "1";
        /// <summary>
        ///CT value
        /// </summary>
        public static string Class = "1";
        /// <summary>
        ///Ib
        /// </summary>
        public static string Ib = "0";
        /// <summary>
        ///IMax
        /// </summary>
        public static string IMax = "0";
        /// <summary>
        ///Meter Constant
        /// </summary>
        public static int MeterConstant = 0;
    }

    public static class GlobalComponentDetails
    {
        public static string UniqueID = string.Empty;
        public static string TestCaseID = string.Empty;

    }
}
