using MeterReader.TestHelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeterReader.DLMSNetSerialCommunication
{
    public static class NetworkHelper
    {
        //Logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string errorType = "";
        /// <summary>
        /// Check weather TCP port is open
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <param name="timeoutMilliseconds"></param>
        /// <returns></returns>
        public static bool IsTcpPortOpen(string ipAddress, int port, int timeoutMilliseconds = 3000)
        {
            try
            {
                if (!IPAddress.TryParse(ipAddress, out IPAddress address))
                    return false;

                using (var client = new TcpClient(address.AddressFamily))
                {
                    IAsyncResult result = client.BeginConnect(address, port, null, null);
                    bool success = result.AsyncWaitHandle.WaitOne(timeoutMilliseconds);

                    if (!success)
                        return false;

                    //client.EndConnect(result);
                    client.Close();
                    SetGetFromMeter.Wait(5 * 1000);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Test weather IP is live or in sleep mode.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool ISPingIPSuccess(string ipAddress, int timeoutMilliSeconds = 2000)
        {
            try
            {
                // Validate and parse the IP address
                if (!IPAddress.TryParse(ipAddress, out IPAddress address))
                    return false;

                using (Ping ping = new Ping())
                {
                    PingOptions options = new PingOptions
                    {
                        DontFragment = true,
                    };

                    byte[] buffer = new byte[32]; // 32 bytes of dummy data
                    //int timeoutMilliSeconds = 1000; // 1 second

                    PingReply reply = ping.Send(address, timeoutMilliSeconds, buffer, options);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                log.Error(ex.StackTrace.ToString());
                return false;
            }
        }
        /// <summary>
        /// Check 1. IPv6 Configured, 2. Server is Live, 3. TCP/IP Connection with Server
        /// </summary>
        /// <param name="ipAddress">Ip Address of Server [Meter] or Host Name</param>
        /// <param name="port">Port Number</param>
        /// <param name="timeoutMilliseconds">Time out</param>
        /// <returns></returns>
        public static bool NetworkTest(string ipAddress, int port, int timeoutMilliseconds = 10000)
        {
            bool Status = true;
            errorType = "";
            try
            {
                TestStopWatch testStopWatch = new TestStopWatch();
                testStopWatch.Start();
                while (!IsIPv6Configured())
                {
                    if (testStopWatch.GetElapsedSeconds() > timeoutMilliseconds / 1000)
                    {
                        errorType += "IPv6 not Configured on the System.";
                        Status = false;
                        return Status;
                    }
                }
                testStopWatch.Start();
                while (!ISPingIPSuccess(ipAddress))
                {
                    if (testStopWatch.GetElapsedSeconds() > timeoutMilliseconds / 1000)
                    {
                        errorType += "Server is not Live.";
                        Status = false;
                        return Status;
                    }
                }
                testStopWatch.Start();
                while (!IsTcpPortOpen(ipAddress, port, timeoutMilliseconds))
                {
                    if (testStopWatch.GetElapsedSeconds() > timeoutMilliseconds / 1000)
                    {
                        errorType += "TCP/IP Connection not established.";
                        Status = false;
                        return Status;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                log.Error(ex.StackTrace.ToString());
                return Status;
            }
            return Status;
        }
        /// <summary>
        /// Get status weather the IPv6 is configured.
        /// </summary>
        /// <returns>true if it is configured else false</returns>
        public static bool IsIPv6Configured()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Any(nic => nic.Supports(NetworkInterfaceComponent.IPv6));
        }

        /// <summary>
        /// Provide the configured IPv6 address.
        /// </summary>
        /// <returns>IPv6 address in string form.</returns>
        public static string GetIPv6Address()
        {
            foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (netInterface.OperationalStatus == OperationalStatus.Up &&
                    netInterface.Supports(NetworkInterfaceComponent.IPv6))
                {
                    foreach (UnicastIPAddressInformation ip in netInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6 &&
                            !ip.Address.IsIPv6LinkLocal) // Exclude link-local addresses
                        {
                            return ip.Address.ToString();
                        }
                    }
                }
            }
            return string.Empty;
        }

    }
}
