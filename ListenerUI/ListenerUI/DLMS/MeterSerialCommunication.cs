using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
//using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;

namespace AP_Source
{
    public class MeterSerialCommunication : IDisposable
    {
        #region Meter Serial Communication Properties and Methods
        public enum WriteMode
        {
            AppendLineFeed,
            None
        }

        private SerialPort _oSP = null;

        private bool _bAlwaysOpenPort = true;

        private byte[] nRcvByte = new byte[5000];

        private int nCounter = 0;

        //private int _nWait = 3;// old
        private int _nWait = 1; // new millisecond

        private bool _bGRemove = false;

        private bool _bLog = false;

        private StreamWriter _oSW = null;

        private string _sFilePath = null;

        private DateTime dtLastRcv;

        public string ComPort
        {
            set
            {
                if (!_oSP.IsOpen)
                {
                    _oSP.PortName = value;
                    return;
                }

                ClosePort();
                _oSP.PortName = value;
            }
        }

        public int BaudRate
        {
            set
            {
                if (!_oSP.IsOpen)
                {
                    _oSP.BaudRate = value;
                    return;
                }

                ClosePort();
                _oSP.BaudRate = value;
            }
        }

        public bool RTCEnable
        {
            set
            {
                if (!_oSP.IsOpen)
                {
                    _oSP.RtsEnable = value;
                    return;
                }

                ClosePort();
                _oSP.RtsEnable = value;
            }
        }

        public bool DTREnable
        {
            set
            {
                if (!_oSP.IsOpen)
                {
                    _oSP.DtrEnable = value;
                    return;
                }

                ClosePort();
                _oSP.DtrEnable = value;
            }
        }

        public StopBits stopBits
        {
            set
            {
                if (!_oSP.IsOpen)
                {
                    _oSP.StopBits = value;
                    return;
                }

                ClosePort();
                _oSP.StopBits = value;
            }
        }

        public int DataBits
        {
            set
            {
                if (!_oSP.IsOpen)
                {
                    _oSP.DataBits = value;
                    return;
                }

                ClosePort();
                _oSP.DataBits = value;
            }
        }

        public Parity parity
        {
            set
            {
                if (!_oSP.IsOpen)
                {
                    _oSP.Parity = value;
                    return;
                }

                ClosePort();
                _oSP.Parity = value;
            }
        }

        public bool AlwaysOpenPort
        {
            get
            {
                return _bAlwaysOpenPort;
            }
            set
            {
                _bAlwaysOpenPort = value;
            }
        }

        public int WaitForRespose
        {
            set
            {
                _nWait = value;
            }
        }

        public bool GRemove
        {
            get
            {
                return _bGRemove;
            }
            set
            {
                _bGRemove = value;
            }
        }

        public bool SerialMonitor
        {
            set
            {
                _bLog = value;
            }
        }

        public string LogFilePath
        {
            set
            {
                _sFilePath = value;
                Directory.CreateDirectory(Path.GetDirectoryName(_sFilePath));
                _oSW = new StreamWriter(_sFilePath, append: true);
                _oSW.AutoFlush = true;
            }
        }

        public MeterSerialCommunication()
        {
            _oSP = new SerialPort();
            _oSP.RtsEnable = true;
            _oSP.ReceivedBytesThreshold = 1;
            _oSP.StopBits = StopBits.One;
            _oSP.DataBits = 8;
            _oSP.DtrEnable = true;
            _oSP.Parity = Parity.None;
            _oSP.ReadBufferSize = 2000;
            _oSP.WriteBufferSize = 2000;
            _oSP.DataReceived += _oSP_DataReceived;
            _oSP.WriteTimeout = 2000;
        }

        public MeterSerialCommunication(string ComPort, int BaudRate)
        {
            _oSP = new SerialPort(ComPort, BaudRate);
            _oSP.RtsEnable = true;
            _oSP.ReceivedBytesThreshold = 1;
            _oSP.StopBits = StopBits.One;
            _oSP.DataBits = 8;
            _oSP.DtrEnable = true;
            _oSP.Parity = Parity.None;
            _oSP.ReadBufferSize = 2000;
            _oSP.WriteBufferSize = 2000;
            _oSP.DataReceived += _oSP_DataReceived;
            _oSP.WriteTimeout = 2000;
        }

        public void OpenPort()
        {
            if (_oSP != null && _oSP.IsOpen)
            {
                _oSP.Close();
            }

            _oSP.Open();
            if (_bLog)
            {
                _oSW.WriteLine("Port Opened at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            }
        }

        public void ClosePort()
        {
            if (_oSP != null && _oSP.IsOpen)
            {
                _oSP.Close();
            }

            if (_bLog)
            {
                _oSW.WriteLine("Port Closed at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            }
        }

        public string Write(string Command, WriteMode writeMode)
        {
            string result = null;
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (!_oSP.IsOpen)
                {
                    OpenPort();
                }

                _oSP.DiscardInBuffer();
                _oSP.DiscardOutBuffer();
                Array.Clear(nRcvByte, 0, nCounter);
                nCounter = 0;
                Thread.Sleep(100);
                DateTime now = DateTime.Now;
                if (writeMode == WriteMode.AppendLineFeed)
                {
                    _oSP.WriteLine(Command);
                }
                else
                {
                    _oSP.Write(Command);
                }

                while (true)
                {
                    Application.DoEvents();
                    if (DateTime.Now.Subtract(now).TotalSeconds > (double)_nWait)
                    {
                        break;
                    }

                    bool flag = true;
                }

                stringBuilder.Length = 0;
                Thread.Sleep(100);
                if (nCounter > 0)
                {
                    for (int i = 0; i < nCounter; i++)
                    {
                        stringBuilder.Append(Convert.ToChar(nRcvByte[i]));
                    }
                }

                result = stringBuilder.ToString();
            }
            catch (Exception)
            {
            }
            finally
            {
                if (!_bAlwaysOpenPort && _oSP.IsOpen)
                {
                    ClosePort();
                }
            }

            return result;
        }

        public string Write(string Command, WriteMode writeMode, bool DiscardBuffer)
        {
            string result = null;
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (!_oSP.IsOpen)
                {
                    OpenPort();
                }

                if (DiscardBuffer)
                {
                    _oSP.DiscardInBuffer();
                    _oSP.DiscardOutBuffer();
                    Array.Clear(nRcvByte, 0, nCounter);
                    nCounter = 0;
                }

                Thread.Sleep(100);
                DateTime now = DateTime.Now;
                if (writeMode == WriteMode.AppendLineFeed)
                {
                    _oSP.WriteLine(Command);
                }
                else
                {
                    _oSP.Write(Command);
                }

                while (true)
                {
                    Application.DoEvents();
                    if (DateTime.Now.Subtract(now).TotalSeconds > (double)_nWait)
                    {
                        break;
                    }

                    bool flag = true;
                }

                stringBuilder.Length = 0;
                Thread.Sleep(100);
                if (nCounter > 0)
                {
                    for (int i = 0; i < nCounter; i++)
                    {
                        stringBuilder.Append(Convert.ToChar(nRcvByte[i]));
                    }
                }

                result = stringBuilder.ToString();
            }
            catch (Exception)
            {
            }
            finally
            {
                if (!_bAlwaysOpenPort && _oSP.IsOpen)
                {
                    ClosePort();
                }
            }

            return result;
        }

        public string Write(string Command)
        {
            string result = null;
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (!_oSP.IsOpen)
                {
                    OpenPort();
                }

                _oSP.DiscardInBuffer();
                _oSP.DiscardOutBuffer();
                Array.Clear(nRcvByte, 0, nCounter);
                nCounter = 0;
                Thread.Sleep(100);
                DateTime now = DateTime.Now;
                _oSP.WriteLine(Command);
                while (true)
                {
                    Application.DoEvents();
                    if (DateTime.Now.Subtract(now).TotalSeconds > (double)_nWait)
                    {
                        break;
                    }

                    bool flag = true;
                }

                stringBuilder.Length = 0;
                //Thread.Sleep(100);//old
                Thread.Sleep(100);//new
                if (nCounter > 0)
                {
                    for (int i = 0; i < nCounter; i++)
                    {
                        stringBuilder.Append(Convert.ToChar(nRcvByte[i]));
                    }
                }

                result = stringBuilder.ToString();
            }
            catch (Exception)
            {
            }
            finally
            {
                if (!_bAlwaysOpenPort && _oSP.IsOpen)
                {
                    ClosePort();
                }
            }

            return result;
        }

        public string Write(string Command, int Wait)//milliseconds
        {
            string result = null;
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (!_oSP.IsOpen)
                {
                    OpenPort();
                }

                _oSP.DiscardInBuffer();
                _oSP.DiscardOutBuffer();
                Array.Clear(nRcvByte, 0, nCounter);
                nCounter = 0;
                Thread.Sleep(50);
                DateTime now = DateTime.Now;
                _oSP.WriteLine(Command);
                while (true)
                {
                    Application.DoEvents();
                    if (DateTime.Now.Subtract(now).TotalMilliseconds > (double)Wait)
                    {
                        break;
                    }

                    bool flag = true;
                }

                stringBuilder.Length = 0;
                Thread.Sleep(50);
                if (nCounter > 0)
                {
                    for (int i = 0; i < nCounter; i++)
                    {
                        stringBuilder.Append(Convert.ToChar(nRcvByte[i]));
                    }
                }

                result = stringBuilder.ToString();
            }
            catch (Exception)
            {
                //MessageBox.Show("Error Connecting Kindly Check PORT Number" );
            }
            finally
            {
                if (!_bAlwaysOpenPort && _oSP.IsOpen)
                {
                    ClosePort();
                }
            }

            return result;
        }

        public string Write(string Command, int Wait, int TotalReceivedChar)
        {
            string result = null;
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (!_oSP.IsOpen)
                {
                    OpenPort();
                }

                _oSP.DiscardInBuffer();
                _oSP.DiscardOutBuffer();
                Array.Clear(nRcvByte, 0, nCounter);
                nCounter = 0;
                Thread.Sleep(100);
                DateTime now = DateTime.Now;
                _oSP.WriteLine(Command);
                while (true)
                {
                    Application.DoEvents();
                    if (DateTime.Now.Subtract(now).TotalSeconds > (double)Wait || nCounter >= TotalReceivedChar)
                    {
                        break;
                    }

                    bool flag = true;
                }

                stringBuilder.Length = 0;
                Thread.Sleep(100);
                if (nCounter > 0)
                {
                    for (int i = 0; i < nCounter; i++)
                    {
                        stringBuilder.Append(Convert.ToChar(nRcvByte[i]));
                    }
                }

                result = stringBuilder.ToString();
            }
            catch (Exception)
            {
            }
            finally
            {
                if (!_bAlwaysOpenPort && _oSP.IsOpen)
                {
                    ClosePort();
                }
            }

            return result;
        }

        public byte[] Write(byte[] Command)
        {
            byte[] array = null;
            try
            {
                if (!_oSP.IsOpen)
                {
                    OpenPort();
                }

                _oSP.DiscardInBuffer();
                _oSP.DiscardOutBuffer();
                Array.Clear(nRcvByte, 0, nCounter);
                nCounter = 0;
                Thread.Sleep(100);
                DateTime now = DateTime.Now;
                if (_bLog)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Length = 0;
                    for (int i = 0; i < Command.Length; i++)
                    {
                        stringBuilder.Append(Command[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }

                    _oSW.WriteLine("Request Send at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _oSW.WriteLine(stringBuilder.ToString());
                    stringBuilder = null;
                }

                _oSP.Write(Command, 0, Command.Length);
                while (true)
                {
                    Application.DoEvents();
                    if (DateTime.Now.Subtract(now).TotalSeconds > (double)_nWait)
                    {
                        break;
                    }

                    bool flag = true;
                }

                Thread.Sleep(100);
                if (nCounter > 0)
                {
                    array = new byte[nCounter];
                    Array.Copy(nRcvByte, array, nCounter);
                    Thread.Sleep(10);
                }

                if (_bLog)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Length = 0;
                    for (int i = 0; i < nCounter; i++)
                    {
                        stringBuilder.Append(array[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }

                    _oSW.WriteLine("Response Received at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _oSW.WriteLine(stringBuilder.ToString());
                    stringBuilder = null;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (!_bAlwaysOpenPort && _oSP.IsOpen)
                {
                    ClosePort();
                }
            }

            return array;
        }

        public byte[] Write(byte[] Command, int StartIndex, int Length)
        {
            byte[] array = null;
            try
            {
                if (!_oSP.IsOpen)
                {
                    OpenPort();
                }

                _oSP.DiscardInBuffer();
                _oSP.DiscardOutBuffer();
                Array.Clear(nRcvByte, 0, nCounter);
                nCounter = 0;
                //Thread.Sleep(100);//old
                //Thread.Sleep(50);// new
                DateTime now = DateTime.Now;
                if (_bLog)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Length = 0;
                    for (int i = StartIndex; i < Length; i++)
                    {
                        stringBuilder.Append(Command[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }

                    _oSW.WriteLine("Request Send at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _oSW.WriteLine(stringBuilder.ToString());
                    stringBuilder = null;
                }

                _oSP.Write(Command, StartIndex, Length);
                while (true)
                {
                    Application.DoEvents();
                    //if (DateTime.Now.Subtract(now).TotalSeconds > (double)_nWait)//old
                    if (DateTime.Now.Subtract(now).TotalMilliseconds > (double)_nWait)//new
                    {
                        break;
                    }

                    bool flag = true;
                }

                //Thread.Sleep(100);// old
                Thread.Sleep(30);// new
                if (nCounter > 0)
                {
                    array = new byte[nCounter];
                    Array.Copy(nRcvByte, array, nCounter);
                    //Thread.Sleep(1);
                }

                if (_bLog)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Length = 0;
                    for (int i = 0; i < nCounter; i++)
                    {
                        stringBuilder.Append(array[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }

                    _oSW.WriteLine("Response Received at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _oSW.WriteLine(stringBuilder.ToString());
                    stringBuilder = null;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (!_bAlwaysOpenPort && _oSP.IsOpen)
                {
                    ClosePort();
                }
            }

            return array;
        }

        public byte[] Write(byte[] Command, int StartIndex, int Length, int Wait)
        {
            byte[] array = null;
            try
            {
                if (!_oSP.IsOpen)
                {
                    OpenPort();
                }

                _oSP.DiscardInBuffer();
                _oSP.DiscardOutBuffer();
                Array.Clear(nRcvByte, 0, nCounter);
                nCounter = 0;
                Thread.Sleep(100);
                DateTime now = DateTime.Now;
                if (_bLog)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Length = 0;
                    for (int i = StartIndex; i < Length; i++)
                    {
                        stringBuilder.Append(Command[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }

                    _oSW.WriteLine("Request Send at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _oSW.WriteLine(stringBuilder.ToString());
                    stringBuilder = null;
                }

                _oSP.Write(Command, StartIndex, Length);
                while (true)
                {
                    Application.DoEvents();
                    if (DateTime.Now.Subtract(now).TotalSeconds > (double)Wait)
                    {
                        break;
                    }

                    bool flag = true;
                }

                Thread.Sleep(100);
                if (nCounter > 0)
                {
                    array = new byte[nCounter];
                    Array.Copy(nRcvByte, array, nCounter);
                    Thread.Sleep(10);
                }

                if (_bLog)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Length = 0;
                    for (int i = 0; i < nCounter; i++)
                    {
                        stringBuilder.Append(array[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }

                    _oSW.WriteLine("Response Received at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _oSW.WriteLine(stringBuilder.ToString());
                    stringBuilder = null;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (!_bAlwaysOpenPort && _oSP.IsOpen)
                {
                    ClosePort();
                }
            }

            return array;
        }

        public byte[] Write(byte[] Command, int StartIndex, int Length, int Wait, int TotalReceived)
        {
            byte[] array = null;
            try
            {
                if (!_oSP.IsOpen)
                {
                    OpenPort();
                }

                _oSP.DiscardInBuffer();
                _oSP.DiscardOutBuffer();
                Array.Clear(nRcvByte, 0, nCounter);
                nCounter = 0;
                if (_bLog)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Length = 0;
                    for (int i = StartIndex; i < Length; i++)
                    {
                        stringBuilder.Append(Command[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }

                    _oSW.WriteLine("Request Send at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _oSW.WriteLine(stringBuilder.ToString());
                    stringBuilder = null;
                }

                _oSP.Write(Command, StartIndex, Length);
                DateTime now = DateTime.Now;
                while (true)
                {
                    Application.DoEvents();
                    if (nCounter >= TotalReceived || DateTime.Now.Subtract(now).TotalSeconds > (double)Wait)
                    {
                        break;
                    }

                    bool flag = true;
                }

                Thread.Sleep(100);
                if (nCounter > 0)
                {
                    array = new byte[nCounter];
                    Array.Copy(nRcvByte, array, nCounter);
                    Thread.Sleep(10);
                }

                if (_bLog)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Length = 0;
                    for (int i = 0; i < nCounter; i++)
                    {
                        stringBuilder.Append(array[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }

                    _oSW.WriteLine("Response Received at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _oSW.WriteLine(stringBuilder.ToString());
                    stringBuilder = null;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (!_bAlwaysOpenPort && _oSP.IsOpen)
                {
                    ClosePort();
                }
            }

            return array;
        }

        public byte[] WriteData(byte[] Command, int StartIndex, int Length, int Wait, int TotalReceived)
        {
            byte[] array = null;
            try
            {
                if (!_oSP.IsOpen)
                {
                    OpenPort();
                }

                _oSP.DiscardInBuffer();
                _oSP.DiscardOutBuffer();
                Array.Clear(nRcvByte, 0, nCounter);
                nCounter = 0;
                if (_bLog)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Length = 0;
                    for (int i = StartIndex; i < Length; i++)
                    {
                        stringBuilder.Append(Command[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }

                    _oSW.WriteLine("Request Send at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _oSW.WriteLine(stringBuilder.ToString());
                    stringBuilder = null;
                }

                _oSP.Write(Command, StartIndex, Length);
                dtLastRcv = DateTime.Now;
                while (nCounter < TotalReceived && !(DateTime.Now.Subtract(dtLastRcv).TotalSeconds > (double)Wait))
                {
                    bool flag = true;
                }

                if (nCounter > 0)
                {
                    array = new byte[nCounter];
                    Array.Copy(nRcvByte, array, nCounter);
                    Thread.Sleep(10);
                }

                if (_bLog)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Length = 0;
                    for (int i = 0; i < nCounter; i++)
                    {
                        stringBuilder.Append(array[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }

                    _oSW.WriteLine("Response Received at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _oSW.WriteLine(stringBuilder.ToString());
                    stringBuilder = null;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (!_bAlwaysOpenPort && _oSP.IsOpen)
                {
                    ClosePort();
                }
            }

            return array;
        }

        public byte[] Write(byte[] Command, int StartIndex, int Length, int Wait, int TotalReceived, int MinRcvBytes, int nMinRcvWait)
        {
            byte[] array = null;
            try
            {
                if (!_oSP.IsOpen)
                {
                    OpenPort();
                }

                _oSP.DiscardInBuffer();
                _oSP.DiscardOutBuffer();
                Array.Clear(nRcvByte, 0, nCounter);
                nCounter = 0;
                DateTime now = DateTime.Now;
                if (_bLog)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Length = 0;
                    for (int i = StartIndex; i < Length; i++)
                    {
                        stringBuilder.Append(Command[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }

                    _oSW.WriteLine("Request Send at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _oSW.WriteLine(stringBuilder.ToString());
                    stringBuilder = null;
                }

                _oSP.Write(Command, StartIndex, Length);
                while (true)
                {
                    Application.DoEvents();
                    if ((DateTime.Now.Subtract(now).TotalSeconds > (double)nMinRcvWait && nCounter < MinRcvBytes) || nCounter >= TotalReceived || DateTime.Now.Subtract(now).TotalSeconds > (double)Wait)
                    {
                        break;
                    }

                    bool flag = true;
                }

                Thread.Sleep(100);
                if (nCounter > 0)
                {
                    array = new byte[nCounter];
                    Array.Copy(nRcvByte, array, nCounter);
                    Thread.Sleep(10);
                }

                if (_bLog)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Length = 0;
                    for (int i = 0; i < nCounter; i++)
                    {
                        stringBuilder.Append(array[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }

                    _oSW.WriteLine("Response Received at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _oSW.WriteLine(stringBuilder.ToString());
                    stringBuilder = null;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (!_bAlwaysOpenPort && _oSP.IsOpen)
                {
                    ClosePort();
                }
            }

            return array;
        }

        public byte[] Write(int TotRcvByteIndex, byte[] Command, int StartIndex, int Length, int Wait)
        {
            byte[] array = null;
            try
            {
                if (!_oSP.IsOpen)
                {
                    OpenPort();
                }

                _oSP.DiscardInBuffer();
                _oSP.DiscardOutBuffer();
                Array.Clear(nRcvByte, 0, nCounter);
                nCounter = 0;
                Thread.Sleep(100);
                DateTime now = DateTime.Now;
                if (_bLog)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Length = 0;
                    for (int i = StartIndex; i < Length; i++)
                    {
                        stringBuilder.Append(Command[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }

                    _oSW.WriteLine("Request Send at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _oSW.WriteLine(stringBuilder.ToString());
                    stringBuilder = null;
                }

                _oSP.Write(Command, StartIndex, Length);
                while (true)
                {
                    Application.DoEvents();
                    if ((nCounter > TotRcvByteIndex && nCounter >= nRcvByte[TotRcvByteIndex]) || DateTime.Now.Subtract(now).TotalSeconds > (double)Wait)
                    {
                        break;
                    }

                    bool flag = true;
                }

                Thread.Sleep(100);
                if (nCounter > 0)
                {
                    array = new byte[nCounter];
                    Array.Copy(nRcvByte, array, nCounter);
                    Thread.Sleep(10);
                }

                if (_bLog)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Length = 0;
                    for (int i = 0; i < nCounter; i++)
                    {
                        stringBuilder.Append(array[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }

                    _oSW.WriteLine("Response Received at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _oSW.WriteLine(stringBuilder.ToString());
                    stringBuilder = null;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (!_bAlwaysOpenPort && _oSP.IsOpen)
                {
                    ClosePort();
                }
            }

            return array;
        }

        public byte[] Write(int TotRcvByteIndex, byte[] Command, int StartIndex, int Length, int Wait, int ExtraBytes)
        {
            byte[] array = null;
            try
            {
                if (!_oSP.IsOpen)
                {
                    OpenPort();
                }

                _oSP.DiscardInBuffer();
                _oSP.DiscardOutBuffer();
                Array.Clear(nRcvByte, 0, nCounter);
                nCounter = 0;
                DateTime now = DateTime.Now;
                if (_bLog)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Length = 0;
                    for (int i = StartIndex; i < Length; i++)
                    {
                        stringBuilder.Append(Command[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }

                    _oSW.WriteLine("Request Send at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _oSW.WriteLine(stringBuilder.ToString());
                    stringBuilder = null;
                }

                _oSP.Write(Command, StartIndex, Length);
                while (true)
                {
                    Application.DoEvents();
                    if ((nCounter > TotRcvByteIndex && nCounter >= nRcvByte[TotRcvByteIndex] + ExtraBytes) || DateTime.Now.Subtract(now).TotalSeconds > (double)Wait)
                    {
                        break;
                    }

                    bool flag = true;
                }

                if (nCounter > 0)
                {
                    array = new byte[nCounter];
                    Array.Copy(nRcvByte, array, nCounter);
                    Thread.Sleep(10);
                }

                if (_bLog)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Length = 0;
                    for (int i = 0; i < nCounter; i++)
                    {
                        stringBuilder.Append(array[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }

                    _oSW.WriteLine("Response Received at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _oSW.WriteLine(stringBuilder.ToString());
                    stringBuilder = null;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (!_bAlwaysOpenPort && _oSP.IsOpen)
                {
                    ClosePort();
                }
            }

            return array;
        }

        public byte[] Write(int TotRcvByteStartIndex, int TotRcvByteLen, byte[] Command, int StartIndex, int Length, int Wait)
        {
            byte[] array = null;
            string empty = string.Empty;
            try
            {
                if (!_oSP.IsOpen)
                {
                    OpenPort();
                }

                _oSP.DiscardInBuffer();
                _oSP.DiscardOutBuffer();
                Array.Clear(nRcvByte, 0, nCounter);
                nCounter = 0;
                DateTime now = DateTime.Now;
                if (_bLog)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Length = 0;
                    for (int i = StartIndex; i < Length; i++)
                    {
                        stringBuilder.Append(Command[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }

                    _oSW.WriteLine("Request Send at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _oSW.WriteLine(stringBuilder.ToString());
                    stringBuilder = null;
                }

                _oSP.Write(Command, StartIndex, Length);
                while (true)
                {
                    Thread.Sleep(50);
                    Application.DoEvents();
                    if (nCounter > TotRcvByteStartIndex + TotRcvByteLen)
                    {
                        empty = string.Empty;
                        for (int i = TotRcvByteStartIndex; i < TotRcvByteStartIndex + TotRcvByteLen; i++)
                        {
                            empty += nRcvByte[i].ToString("X2");
                        }

                        if (!string.IsNullOrEmpty(empty) && nCounter >= Convert.ToInt32(empty, 16))
                        {
                            break;
                        }
                    }

                    if (DateTime.Now.Subtract(now).TotalSeconds > (double)Wait)
                    {
                        break;
                    }

                    bool flag = true;
                }

                Thread.Sleep(100);
                if (nCounter > 0)
                {
                    array = new byte[nCounter];
                    Array.Copy(nRcvByte, array, nCounter);
                    Thread.Sleep(10);
                }

                if (_bLog)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Length = 0;
                    for (int i = 0; i < nCounter; i++)
                    {
                        stringBuilder.Append(array[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }

                    _oSW.WriteLine("Response Received at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _oSW.WriteLine(stringBuilder.ToString());
                    stringBuilder = null;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (!_bAlwaysOpenPort && _oSP.IsOpen)
                {
                    ClosePort();
                }
            }

            return array;
        }

        public byte[] Read(int Wait)
        {
            byte[] array = null;
            try
            {
                if (!_oSP.IsOpen)
                {
                    OpenPort();
                }

                _oSP.DiscardInBuffer();
                _oSP.DiscardOutBuffer();
                Array.Clear(nRcvByte, 0, nCounter);
                nCounter = 0;
                Thread.Sleep(100);
                DateTime now = DateTime.Now;
                while (true)
                {
                    Application.DoEvents();
                    if (DateTime.Now.Subtract(now).TotalSeconds > (double)Wait)
                    {
                        break;
                    }

                    bool flag = true;
                }

                Thread.Sleep(100);
                if (nCounter > 0)
                {
                    array = new byte[nCounter];
                    Array.Copy(nRcvByte, array, nCounter);
                    Thread.Sleep(10);
                }

                if (_bLog)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Length = 0;
                    for (int i = 0; i < nCounter; i++)
                    {
                        stringBuilder.Append(array[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }

                    _oSW.WriteLine("Response Received at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _oSW.WriteLine(stringBuilder.ToString());
                    stringBuilder = null;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (!_bAlwaysOpenPort && _oSP.IsOpen)
                {
                    ClosePort();
                }
            }

            return array;
        }

        public byte[] Read(int Wait, int TotalReceived)
        {
            byte[] array = null;
            try
            {
                if (!_oSP.IsOpen)
                {
                    OpenPort();
                }

                _oSP.DiscardInBuffer();
                _oSP.DiscardOutBuffer();
                Array.Clear(nRcvByte, 0, nCounter);
                nCounter = 0;
                Thread.Sleep(100);
                DateTime now = DateTime.Now;
                while (true)
                {
                    Application.DoEvents();
                    if (DateTime.Now.Subtract(now).TotalSeconds > (double)Wait || nCounter >= TotalReceived)
                    {
                        break;
                    }

                    bool flag = true;
                }

                Thread.Sleep(100);
                if (nCounter > 0)
                {
                    array = new byte[nCounter];
                    Array.Copy(nRcvByte, array, nCounter);
                    Thread.Sleep(10);
                }

                if (_bLog)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Length = 0;
                    for (int i = 0; i < nCounter; i++)
                    {
                        stringBuilder.Append(array[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }

                    _oSW.WriteLine("Response Received at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _oSW.WriteLine(stringBuilder.ToString());
                    stringBuilder = null;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (!_bAlwaysOpenPort && _oSP.IsOpen)
                {
                    ClosePort();
                }
            }

            return array;
        }

        public void ClearPreviousData()
        {
            Array.Clear(nRcvByte, 0, nCounter);
            nCounter = 0;
        }

        public string GetBufferData()
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Length = 0;
                Thread.Sleep(100);
                if (nCounter > 0)
                {
                    for (int i = 0; i < nCounter; i++)
                    {
                        stringBuilder.Append(Convert.ToChar(nRcvByte[i]));
                    }
                }

                return stringBuilder.ToString();
            }
            catch (Exception)
            {
            }

            return null;
        }

        public byte[] GetBufferDataArray()
        {
            byte[] array = null;
            try
            {
                array = new byte[nCounter];
                Array.Copy(nRcvByte, array, nCounter);
            }
            catch (Exception)
            {
            }

            return array;
        }

        public void Dispose()
        {
            if (_oSP != null && _oSP.IsOpen)
            {
                _oSP.Close();
            }

            _oSP.Dispose();
            if (_oSW != null)
            {
                _oSW.Close();
            }

            _oSW = null;
        }

        private void _oSP_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int num = 0;
                num = _oSP.BytesToRead;
                for (int i = 0; i < num; i++)
                {
                    try
                    {
                        if (nCounter >= nRcvByte.Length)
                        {
                            nCounter = 0;
                        }

                        nRcvByte[nCounter++] = (byte)_oSP.ReadByte();
                        if (_bGRemove && nCounter == 1 && nRcvByte[0] == 71)
                        {
                            nCounter--;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                dtLastRcv = DateTime.Now;
            }
            catch (Exception)
            {
            }
            finally
            {
            }
        }

        public void DiscardInputOutputBuffer()
        {
            try
            {
                if (!_oSP.IsOpen)
                {
                    OpenPort();
                }
                _oSP.DiscardInBuffer();
                _oSP.DiscardOutBuffer();
            }
            catch (Exception)
            {
            }
        }

        #endregion


    }
}
