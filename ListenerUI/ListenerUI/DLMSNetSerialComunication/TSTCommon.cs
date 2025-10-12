using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Gurux.DLMS;
using Gurux.DLMS.Enums;
using Indali.Security.Enum;
using Indali.Common;

namespace Indali.Common
{
    public class TSTCommon
    {
        public static string Title = "";
        public static Control Owner = (Control)null;
        internal static readonly byte[] LLCSendBytes = new byte[3]
        {
      (byte) 230,
      (byte) 230,
      (byte) 0
        };
        internal static readonly byte[] LLCReplyBytes = new byte[3]
        {
      (byte) 230,
      (byte) 231,
      (byte) 0
        };
        internal const byte HDLCFrameStartEnd = 126;


        [DebuggerStepThrough]
        public static string ToHex(byte[] bytes, bool addSpace)
        {
            return TSTCommon.ToHex(bytes, addSpace, 0, bytes == null ? 0 : bytes.Length);
        }

        [DebuggerStepThrough]
        public static string ToHex(byte[] bytes, bool addSpace, int index, int count)
        {
            if (bytes == null || bytes.Length == 0 || count == 0)
                return string.Empty;
            char[] chArray = new char[count * (addSpace ? 3 : 2)];
            int length = 0;
            for (int index1 = 0; index1 != count; ++index1)
            {
                int num1 = (int)bytes[index + index1] >> 4;
                chArray[length] = num1 > 9 ? (char)(num1 + 55) : (char)(num1 + 48);
                int index2 = length + 1;
                int num2 = (int)bytes[index + index1] & 15;
                chArray[index2] = num2 > 9 ? (char)(num2 + 55) : (char)(num2 + 48);
                length = index2 + 1;
                if (addSpace)
                {
                    chArray[length] = ' ';
                    ++length;
                }
            }
            if (addSpace)
                --length;
            return new string(chArray, 0, length);
        }

        public static string SystemTitleToString(string standard, byte[] st)
        {
            if (standard == "Italy" || !char.IsLetter((char)st[0]) || !char.IsLetter((char)st[1]) || !char.IsLetter((char)st[2]))
                return TSTCommon.UNISystemTitleToString(st);
            return !char.IsNumber((char)st[3]) || !char.IsNumber((char)st[4]) || !char.IsNumber((char)st[5]) || !char.IsNumber((char)st[6]) || !char.IsNumber((char)st[7]) ? TSTCommon.IdisSystemTitleToString(st) : TSTCommon.DlmsSystemTitleToString(st);
        }
        private static string IdisSystemTitleToString(byte[] st)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("IDIS system title:");
            stringBuilder.Append("Manufacturer Code: ");
            stringBuilder.AppendLine(new string(new char[3]
            {
        (char) st[0],
        (char) st[1],
        (char) st[2]
            }));
            stringBuilder.Append("Function type: ");
            int num1 = (int)st[4] >> 4;
            bool flag = false;
            if ((num1 & 1) != 0)
            {
                stringBuilder.Append("Disconnector extension");
                flag = true;
            }
            if ((num1 & 2) != 0)
            {
                if (flag)
                    stringBuilder.Append(", ");
                flag = true;
                stringBuilder.Append("Load Management extension");
            }
            if ((num1 & 4) != 0)
            {
                if (flag)
                    stringBuilder.Append(", ");
                stringBuilder.Append("Multi Utility extension");
            }
            int num2 = ((int)st[4] & 15) << 24 | (int)st[5] << 16 | (int)st[6] << 8 | (int)st[7];
            stringBuilder.AppendLine("");
            stringBuilder.Append("Serial number: ");
            stringBuilder.AppendLine(num2.ToString());
            return stringBuilder.ToString();
        }
        private static string UNISystemTitleToString(byte[] st)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("UNI/TS system title:");
            stringBuilder.Append("Manufacturer: ");
            ushort num = (ushort)((uint)st[0] << 8 | (uint)st[1]);
            stringBuilder.AppendLine(TSTCommon.DecryptManufacturer(num));
            stringBuilder.Append("Serial number: ");
            stringBuilder.AppendLine(TSTCommon.ToHex(new byte[6]
            {
        st[7],
        st[6],
        st[5],
        st[4],
        st[3],
        st[2]
            }, false));
            return stringBuilder.ToString();
        }
        private static string DlmsSystemTitleToString(byte[] st)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("IDIS system title:");
            stringBuilder.Append("Manufacturer Code: ");
            stringBuilder.AppendLine(new string(new char[3]
            {
        (char) st[0],
        (char) st[1],
        (char) st[2]
            }));
            stringBuilder.Append("Serial number: ");
            stringBuilder.AppendLine(new string(new char[5]
            {
        (char) st[3],
        (char) st[4],
        (char) st[5],
        (char) st[6],
        (char) st[7]
            }));
            return stringBuilder.ToString();
        }
        public static string DecryptManufacturer(ushort value)
        {
            ushort num1 = (ushort)((int)value >> 8 | (int)value << 8);
            char ch1 = (char)(((int)num1 & 31) + 64);
            ushort num2 = (ushort)((uint)num1 >> 5);
            char ch2 = (char)(((int)num2 & 31) + 64);
            return new string(new char[3]
            {
        (char) (((int) (ushort) ((uint) num2 >> 5) & 31) + 64),
        ch2,
        ch1
            });
        }
        private static byte GetValue(byte c)
        {
            byte num = byte.MaxValue;
            if (c > (byte)47 && c < (byte)58)
                num = (byte)((uint)c - 48U);
            else if (c > (byte)64 && c < (byte)71)
                num = (byte)((int)c - 65 + 10);
            else if (c > (byte)96 && c < (byte)103)
                num = (byte)((int)c - 97 + 10);
            return num;
        }

        private static bool IsHex(byte c) => TSTCommon.GetValue(c) != byte.MaxValue;

        public static byte[] HexToBytes(string value)
        {
            if (value == null || value.Length == 0)
                return new byte[0];
            int length = value.Length / 2;
            if (value.Length % 2 != 0)
                ++length;
            byte[] src = new byte[length];
            int num = -1;
            int count = 0;
            foreach (byte c in value)
            {
                if (TSTCommon.IsHex(c))
                {
                    if (num == -1)
                        num = (int)TSTCommon.GetValue(c);
                    else if (num != -1)
                    {
                        src[count] = (byte)((uint)(num << 4) | (uint)TSTCommon.GetValue(c));
                        num = -1;
                        ++count;
                    }
                }
                else if (num != -1 && c == (byte)32)
                {
                    src[count] = TSTCommon.GetValue(c);
                    num = -1;
                    ++count;
                }
                else
                    num = -1;
            }
            if (num != -1)
            {
                src[count] = (byte)num;
                ++count;
            }
            if (src.Length == count)
                return src;
            byte[] dst = new byte[count];
            Buffer.BlockCopy((Array)src, 0, (Array)dst, 0, count);
            return dst;
        }

        public static int IndexOf(byte[] input, byte[] pattern, int index, int count)
        {
            if (count - index < pattern.Length)
                return -1;
            byte num1 = pattern[0];
            int num2;
            if ((num2 = Array.IndexOf<byte>(input, num1, index, count - index)) >= 0)
            {
                if (count - num2 < pattern.Length)
                {
                    num2 = -1;
                }
                else
                {
                    for (int index1 = 0; index1 < pattern.Length; ++index1)
                    {
                        if (num2 + index1 >= input.Length || (int)pattern[index1] != (int)input[num2 + index1])
                            return TSTCommon.IndexOf(input, pattern, num2 + 1, count);
                    }
                }
            }
            return num2;
        }

        public static DialogResult ShowQuestion(IWin32Window parent, string title, string str)
        {
            try
            {
                if (!Environment.UserInteractive)
                    return DialogResult.Yes;
                if (!(parent is Control control))
                    return MessageBox.Show(str, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (!control.InvokeRequired)
                    return MessageBox.Show(parent, str, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (!control.IsHandleCreated)
                    return MessageBox.Show(str, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                return (DialogResult)control.Invoke((Delegate)new TSTCommon.ShowDialogEventHandler(TSTCommon.ShowQuestion), (object)parent, (object)title, (object)str);
            }
            catch
            {
                return DialogResult.Abort;
            }
        }

        public static DialogResult ShowExclamation(IWin32Window parent, string title, string str)
        {
            try
            {
                if (!Environment.UserInteractive)
                    return DialogResult.OK;
                if (!(parent is Control control))
                    return MessageBox.Show(str, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (!control.InvokeRequired)
                    return MessageBox.Show(parent, str, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (!control.IsHandleCreated)
                    return MessageBox.Show(str, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                return (DialogResult)control.Invoke((Delegate)new TSTCommon.ShowDialogEventHandler(TSTCommon.ShowExclamation), (object)parent, (object)title, (object)str);
            }
            catch
            {
                return DialogResult.Abort;
            }
        }
        /*
        internal static string ToLogicalName(object value)
        {
            if (!(value is byte[] numArray))
                return Convert.ToString(value);
            if (numArray.Length == 0)
                numArray = new byte[6];
            if (numArray.Length != 6)
                throw new ArgumentException(Resources.InvalidLogicalName);
            return ((int)numArray[0] & (int)byte.MaxValue).ToString() + "." + ((int)numArray[1] & (int)byte.MaxValue).ToString() + "." + ((int)numArray[2] & (int)byte.MaxValue).ToString() + "." + ((int)numArray[3] & (int)byte.MaxValue).ToString() + "." + ((int)numArray[4] & (int)byte.MaxValue).ToString() + "." + ((int)numArray[5] & (int)byte.MaxValue).ToString();
        }
        */
        internal static void SetObjectCount(int count, GXByteBuffer buff)
        {
            if (count < 128)
                buff.SetUInt8((byte)count);
            else if (count < 256)
            {
                buff.SetUInt8((byte)129);
                buff.SetUInt8((byte)count);
            }
            else if (count < 65536)
            {
                buff.SetUInt8((byte)130);
                buff.SetUInt16((ushort)count);
            }
            else
            {
                buff.SetUInt8((byte)132);
                buff.SetUInt32((uint)count);
            }
        }

        internal static void SetBitString(GXByteBuffer buff, object value, bool addCount)
        {
            if (value is GXBitString)
                value = (object)(value as GXBitString).Value;
            if (value is string)
            {
                byte num1 = 0;
                string str = (string)value;
                if (addCount)
                    TSTCommon.SetObjectCount(str.Length, buff);
                int num2 = 7;
                for (int index = 0; index != str.Length; ++index)
                {
                    switch (str[index])
                    {
                        case '0':
                            --num2;
                            if (num2 == -1)
                            {
                                num2 = 7;
                                buff.SetUInt8(num1);
                                num1 = (byte)0;
                                continue;
                            }
                            continue;
                        case '1':
                            num1 |= (byte)(1 << num2);
                            goto case '0';
                        default:
                            throw new ArgumentException("Not a bit string.");
                    }
                }
                if (num2 == 7)
                    return;
                buff.SetUInt8(num1);
            }
            else if (value is byte[] numArray)
            {
                TSTCommon.SetObjectCount(8 * numArray.Length, buff);
                buff.Set(numArray);
            }
            else if (value == null)
            {
                buff.SetUInt8((byte)0);
            }
            else
            {
                if (!(value is byte num))
                    throw new Exception("BitString must give as string.");
                TSTCommon.SetObjectCount(8, buff);
                buff.SetUInt8(num);
            }
        }

        private static void SetString(GXByteBuffer buff, object value)
        {
            if (value is byte[] numArray)
            {
                TSTCommon.SetObjectCount(numArray.Length, buff);
                buff.Set(numArray);
            }
            if (value != null)
            {
                string s = Convert.ToString(value);
                TSTCommon.SetObjectCount(s.Length, buff);
                buff.Set(Encoding.ASCII.GetBytes(s));
            }
            else
                buff.SetUInt8((byte)0);
        }

        private static void SetUtcString(GXByteBuffer buff, object value)
        {
            if (value != null)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(Convert.ToString(value));
                TSTCommon.SetObjectCount(bytes.Length, buff);
                buff.Set(bytes);
            }
            else
                buff.SetUInt8((byte)0);
        }

        private static void SetDate(GXByteBuffer buff, object value)
        {
            GXDateTime gxDateTime;
            switch (value)
            {
                case GXDateTime _:
                    gxDateTime = (GXDateTime)value;
                    break;
                case DateTime dateTime:
                    gxDateTime = new GXDateTime(dateTime);
                    break;
                case DateTimeOffset dateTimeOffset1:
                    gxDateTime = new GXDateTime(dateTimeOffset1);
                    break;
                case string _:
                    gxDateTime = (GXDateTime)DateTime.Parse((string)value);
                    break;
                default:
                    throw new Exception("Invalid date format.");
            }
            DateTimeOffset dateTimeOffset2;
            if ((gxDateTime.Skip & DateTimeSkips.Year) != 0)
            {
                buff.SetUInt16(ushort.MaxValue);
            }
            else
            {
                GXByteBuffer gxByteBuffer = buff;
                dateTimeOffset2 = gxDateTime.Value;
                int year = (int)(ushort)dateTimeOffset2.Year;
                gxByteBuffer.SetUInt16((ushort)year);
            }
            if ((gxDateTime.Skip & DateTimeSkips.Month) != 0)
                buff.SetUInt8(byte.MaxValue);
            else if ((gxDateTime.Extra & DateTimeExtraInfo.DstBegin) != 0)
                buff.SetUInt8((byte)254);
            else if ((gxDateTime.Extra & DateTimeExtraInfo.DstEnd) != 0)
            {
                buff.SetUInt8((byte)253);
            }
            else
            {
                GXByteBuffer gxByteBuffer = buff;
                dateTimeOffset2 = gxDateTime.Value;
                int month = (int)(byte)dateTimeOffset2.Month;
                gxByteBuffer.SetUInt8((byte)month);
            }
            if ((gxDateTime.Skip & DateTimeSkips.Day) != 0)
            {
                buff.SetUInt8(byte.MaxValue);
            }
            else
            {
                GXByteBuffer gxByteBuffer = buff;
                dateTimeOffset2 = gxDateTime.Value;
                int day = (int)(byte)dateTimeOffset2.Day;
                gxByteBuffer.SetUInt8((byte)day);
            }
            if ((gxDateTime.Skip & DateTimeSkips.DayOfWeek) != 0)
            {
                buff.SetUInt8(byte.MaxValue);
            }
            else
            {
                dateTimeOffset2 = gxDateTime.Value;
                if (dateTimeOffset2.DayOfWeek == DayOfWeek.Sunday)
                {
                    buff.SetUInt8((byte)7);
                }
                else
                {
                    GXByteBuffer gxByteBuffer = buff;
                    dateTimeOffset2 = gxDateTime.Value;
                    int dayOfWeek = (int)(byte)dateTimeOffset2.DayOfWeek;
                    gxByteBuffer.SetUInt8((byte)dayOfWeek);
                }
            }
        }
        private static void SetDateTime(GXDLMSSettings settings, GXByteBuffer buff, object value)
        {
            GXDateTime gxDateTime;
            switch (value)
            {
                case GXDateTime _:
                    gxDateTime = (GXDateTime)value;
                    break;
                case DateTime dateTime:
                    gxDateTime = new GXDateTime(dateTime);
                    gxDateTime.Skip |= DateTimeSkips.Ms;
                    break;
                case string _:
                    gxDateTime = new GXDateTime(DateTime.Parse((string)value));
                    gxDateTime.Skip |= DateTimeSkips.Ms;
                    break;
                default:
                    throw new Exception("Invalid date format.");
            }
            if (gxDateTime.Value.UtcDateTime == DateTime.MinValue)
                gxDateTime.Value = (DateTimeOffset)DateTime.SpecifyKind(new DateTime(2000, 1, 1).Date, DateTimeKind.Utc);
            else if (gxDateTime.Value.UtcDateTime == DateTime.MaxValue)
                gxDateTime.Value = (DateTimeOffset)DateTime.SpecifyKind(DateTime.Now.AddYears(1).Date, DateTimeKind.Utc);
            DateTimeOffset dateTimeOffset = gxDateTime.Value;
            if ((gxDateTime.Skip & DateTimeSkips.Year) == DateTimeSkips.None)
                buff.SetUInt16((ushort)dateTimeOffset.Year);
            else
                buff.SetUInt16(ushort.MaxValue);
            if ((gxDateTime.Skip & DateTimeSkips.Month) == DateTimeSkips.None)
            {
                if ((gxDateTime.Extra & DateTimeExtraInfo.DstBegin) != 0)
                    buff.SetUInt8((byte)254);
                else if ((gxDateTime.Extra & DateTimeExtraInfo.DstEnd) != 0)
                    buff.SetUInt8((byte)253);
                else
                    buff.SetUInt8((byte)dateTimeOffset.Month);
            }
            else
                buff.SetUInt8(byte.MaxValue);
            if ((gxDateTime.Skip & DateTimeSkips.Day) == DateTimeSkips.None)
                buff.SetUInt8((byte)dateTimeOffset.Day);
            else
                buff.SetUInt8(byte.MaxValue);
            if ((gxDateTime.Skip & DateTimeSkips.DayOfWeek) != 0)
                buff.SetUInt8(byte.MaxValue);
            else if (gxDateTime.DayOfWeek == 0)
            {
                byte num = (byte)gxDateTime.Value.DayOfWeek;
                if (num == (byte)0)
                    num = (byte)7;
                buff.SetUInt8(num);
            }
            else
                buff.SetUInt8((byte)gxDateTime.DayOfWeek);
            if ((gxDateTime.Skip & DateTimeSkips.Hour) == DateTimeSkips.None)
                buff.SetUInt8((byte)dateTimeOffset.Hour);
            else
                buff.SetUInt8(byte.MaxValue);
            if ((gxDateTime.Skip & DateTimeSkips.Minute) == DateTimeSkips.None)
                buff.SetUInt8((byte)dateTimeOffset.Minute);
            else
                buff.SetUInt8(byte.MaxValue);
            if ((gxDateTime.Skip & DateTimeSkips.Second) == DateTimeSkips.None)
                buff.SetUInt8((byte)dateTimeOffset.Second);
            else
                buff.SetUInt8(byte.MaxValue);
            if ((gxDateTime.Skip & DateTimeSkips.Ms) == DateTimeSkips.None)
                buff.SetUInt8((byte)(dateTimeOffset.Millisecond / 10));
            else
                buff.SetUInt8(byte.MaxValue);
            if ((gxDateTime.Skip & DateTimeSkips.Deviation) == DateTimeSkips.None)
            {
                short totalMinutes = (short)gxDateTime.Value.Offset.TotalMinutes;
                if (settings != null && settings.UseUtc2NormalTime)
                    buff.SetInt16(totalMinutes);
                else
                    buff.SetInt16((short)-totalMinutes);
            }
            else
                buff.SetUInt16((ushort)32768);
            if ((gxDateTime.Skip & DateTimeSkips.Status) == DateTimeSkips.None)
                buff.SetUInt8((byte)gxDateTime.Status);
            else
                buff.SetUInt8(byte.MaxValue);
        }
        private static void SetTime(GXByteBuffer buff, object value)
        {
            GXDateTime gxDateTime;
            switch (value)
            {
                case GXDateTime _:
                    gxDateTime = (GXDateTime)value;
                    break;
                case DateTime dateTime:
                    gxDateTime = new GXDateTime(dateTime);
                    break;
                case DateTimeOffset dateTimeOffset1:
                    gxDateTime = new GXDateTime(dateTimeOffset1);
                    break;
                case string _:
                    gxDateTime = (GXDateTime)DateTime.Parse((string)value);
                    break;
                default:
                    throw new Exception("Invalid date format.");
            }
            DateTimeOffset dateTimeOffset2;
            if ((gxDateTime.Skip & DateTimeSkips.Hour) != 0)
            {
                buff.SetUInt8(byte.MaxValue);
            }
            else
            {
                GXByteBuffer gxByteBuffer = buff;
                dateTimeOffset2 = gxDateTime.Value;
                int hour = (int)(byte)dateTimeOffset2.Hour;
                gxByteBuffer.SetUInt8((byte)hour);
            }
            if ((gxDateTime.Skip & DateTimeSkips.Minute) != 0)
            {
                buff.SetUInt8(byte.MaxValue);
            }
            else
            {
                GXByteBuffer gxByteBuffer = buff;
                dateTimeOffset2 = gxDateTime.Value;
                int minute = (int)(byte)dateTimeOffset2.Minute;
                gxByteBuffer.SetUInt8((byte)minute);
            }
            if ((gxDateTime.Skip & DateTimeSkips.Second) != 0)
            {
                buff.SetUInt8(byte.MaxValue);
            }
            else
            {
                GXByteBuffer gxByteBuffer = buff;
                dateTimeOffset2 = gxDateTime.Value;
                int second = (int)(byte)dateTimeOffset2.Second;
                gxByteBuffer.SetUInt8((byte)second);
            }
            if ((gxDateTime.Skip & DateTimeSkips.Ms) != 0)
            {
                buff.SetUInt8(byte.MaxValue);
            }
            else
            {
                GXByteBuffer gxByteBuffer = buff;
                dateTimeOffset2 = gxDateTime.Value;
                int num = (int)(byte)(dateTimeOffset2.Millisecond / 10);
                gxByteBuffer.SetUInt8((byte)num);
            }
        }
        private static void SetBcd(GXByteBuffer buff, object value)
        {
            buff.SetUInt8(Convert.ToByte(value));
        }
        private static void SetArray(GXDLMSSettings settings, GXByteBuffer buff, object value)
        {
            if (value != null)
            {
                List<object> objectList;
                if (value is List<object>)
                {
                    objectList = (List<object>)value;
                }
                else
                {
                    objectList = new List<object>();
                    objectList.AddRange((IEnumerable<object>)(object[])value);
                }
                TSTCommon.SetObjectCount(objectList.Count, buff);
                foreach (object obj in objectList)
                {
                    DataType type = GXDLMSConverter.GetDLMSDataType(obj);
                    if (type == DataType.Array)
                        type = DataType.Structure;
                    TSTCommon.SetData(settings, buff, type, obj);
                }
            }
            else
                TSTCommon.SetObjectCount(0, buff);
        }
        private static void SetOctetString(GXByteBuffer buff, object value)
        {
            switch (value)
            {
                case string _:
                    byte[] bytes = TSTCommon.HexToBytes((string)value);
                    TSTCommon.SetObjectCount(bytes.Length, buff);
                    buff.Set(bytes);
                    break;
                case sbyte[] _:
                    TSTCommon.SetObjectCount(((byte[])value).Length, buff);
                    buff.Set((byte[])value);
                    break;
                case null:
                    TSTCommon.SetObjectCount(0, buff);
                    break;
                default:
                    throw new Exception("Invalid data type.");
            }
        }
        public static void SetData(
          GXDLMSSettings settings,
          GXByteBuffer buff,
          DataType type,
          object value)
        {
            if ((type == DataType.Array || type == DataType.Structure) && value is byte[])
            {
                buff.Set((byte[])value);
            }
            else
            {
                buff.SetUInt8((byte)type);
                switch (type)
                {
                    case DataType.None:
                        break;
                    case DataType.Array:
                    case DataType.Structure:
                        TSTCommon.SetArray(settings, buff, value);
                        break;
                    case DataType.Boolean:
                        if (Convert.ToBoolean(value))
                        {
                            buff.SetUInt8((byte)1);
                            break;
                        }
                        buff.SetUInt8((byte)0);
                        break;
                    case DataType.BitString:
                        TSTCommon.SetBitString(buff, value, true);
                        break;
                    case DataType.Int32:
                        switch (value)
                        {
                            case DateTime date:
                                buff.SetUInt32((uint)GXDateTime.ToUnixTime(date));
                                return;
                            case GXDateTime _:
                                buff.SetUInt32((uint)GXDateTime.ToUnixTime(((GXDateTime)value).Value.DateTime));
                                return;
                            default:
                                buff.SetUInt32((uint)Convert.ToInt32(value));
                                return;
                        }
                    case DataType.UInt32:
                        buff.SetUInt32(Convert.ToUInt32(value));
                        break;
                    case DataType.OctetString:
                        int num1;
                        switch (value)
                        {
                            case GXDate _:
                                buff.SetUInt8((byte)5);
                                TSTCommon.SetDate(buff, value);
                                return;
                            case GXTime _:
                                buff.SetUInt8((byte)4);
                                TSTCommon.SetTime(buff, value);
                                return;
                            case GXDateTime _:
                                num1 = 1;
                                break;
                            default:
                                num1 = value is DateTime ? 1 : 0;
                                break;
                        }
                        if (num1 != 0)
                        {
                            buff.SetUInt8((byte)12);
                            TSTCommon.SetDateTime(settings, buff, value);
                            break;
                        }
                        TSTCommon.SetOctetString(buff, value);
                        break;
                    case DataType.String:
                        TSTCommon.SetString(buff, value);
                        break;
                    case DataType.StringUTF8:
                        TSTCommon.SetUtcString(buff, value);
                        break;
                    case DataType.Bcd:
                        TSTCommon.SetBcd(buff, value);
                        break;
                    case DataType.Int8:
                        buff.SetUInt8((byte)Convert.ToSByte(value));
                        break;
                    case DataType.Int16:
                        if (value is ushort num2)
                        {
                            buff.SetUInt16(num2);
                            break;
                        }
                        int int32 = Convert.ToInt32(value);
                        if (int32 == 32768)
                            buff.SetUInt16((ushort)32768);
                        else
                            buff.SetInt16((short)int32);
                        break;
                    case DataType.UInt8:
                    case DataType.Enum:
                        buff.SetUInt8(Convert.ToByte(value));
                        break;
                    case DataType.UInt16:
                        buff.SetUInt16(Convert.ToUInt16(value));
                        break;
                    case DataType.CompactArray:
                        throw new Exception("Invalid data type.");
                    case DataType.Int64:
                        buff.SetUInt64((ulong)Convert.ToInt64(value));
                        break;
                    case DataType.UInt64:
                        buff.SetUInt64(Convert.ToUInt64(value));
                        break;
                    case DataType.Float32:
                        buff.SetFloat((float)value);
                        break;
                    case DataType.Float64:
                        buff.SetDouble((double)value);
                        break;
                    case DataType.DateTime:
                        TSTCommon.SetDateTime(settings, buff, value);
                        break;
                    case DataType.Date:
                        TSTCommon.SetDate(buff, value);
                        break;
                    case DataType.Time:
                        TSTCommon.SetTime(buff, value);
                        break;
                    default:
                        throw new Exception("Invalid data type.");
                }
            }
        }

        internal static void ToBitString(StringBuilder sb, byte value, int count)
        {
            if (count <= 0)
                return;
            if (count > 8)
                count = 8;
            for (int index = 7; index != 8 - count - 1; --index)
            {
                if (((uint)value & (uint)(1 << index)) > 0U)
                    sb.Append('1');
                else
                    sb.Append('0');
            }
        }

        public static DataType GetDLMSDataType(System.Type type)
        {
            if (type == (System.Type)null)
                return DataType.None;
            if (type == typeof(int))
                return DataType.Int32;
            if (type == typeof(uint))
                return DataType.UInt32;
            if (type == typeof(string))
                return DataType.String;
            if (type == typeof(byte))
                return DataType.UInt8;
            if (type == typeof(sbyte))
                return DataType.Int8;
            if (type == typeof(short))
                return DataType.Int16;
            if (type == typeof(ushort))
                return DataType.UInt16;
            if (type == typeof(long))
                return DataType.Int64;
            if (type == typeof(ulong))
                return DataType.UInt64;
            if (type == typeof(float))
                return DataType.Float32;
            if (type == typeof(double))
                return DataType.Float64;
            if (type == typeof(DateTime) || type == typeof(GXDateTime))
                return DataType.DateTime;
            if (type == typeof(GXDate))
                return DataType.Date;
            if (type == typeof(GXTime))
                return DataType.Time;
            if (type == typeof(bool))
                return DataType.Boolean;
            if (type == typeof(byte[]))
                return DataType.OctetString;
            if (type == typeof(GXStructure))
                return DataType.Structure;
            if (type == typeof(GXArray) || type == typeof(object[]))
                return DataType.Array;
            if (type == typeof(GXEnum))
                return DataType.Enum;
            if (type == typeof(GXBitString))
                return DataType.BitString;
            throw new Exception("Failed to convert data type to Gurux.DLMS data type. Unknown data type.");
        }

        public static int GetObjectCount(GXByteBuffer data)
        {
            int uint8 = (int)data.GetUInt8();
            if (uint8 <= 128)
                return uint8;
            switch (uint8)
            {
                case 129:
                    return (int)data.GetUInt8();
                case 130:
                    return (int)data.GetUInt16();
                case 131:
                    int uint24 = data.GetUInt24(data.Position);
                    data.Position += 3;
                    return uint24;
                case 132:
                    return (int)data.GetUInt32();
                default:
                    throw new ArgumentException("Invalid count.");
            }
        }

        /*
        public static object GetData(GXDLMSSettings settings, GXByteBuffer data, GXDataInfo info)
        {
            object data1 = (object)null;
            int position = data.Position;
            if (data.Position == data.Size)
            {
                info.Complete = false;
                return (object)null;
            }
            info.Complete = true;
            bool knownType = info.Type != 0;
            if (!knownType)
                info.Type = (DataType)data.GetUInt8();
            if (info.Type == DataType.None)
            {
                if (info.xml != null)
                    info.xml.AppendLine("<" + info.xml.GetDataType(info.Type) + " />");
                return data1;
            }
            if (data.Position == data.Size)
            {
                info.Complete = false;
                return (object)null;
            }
            object data2;
            switch (info.Type)
            {
                case DataType.Array:
                case DataType.Structure:
                    data2 = TSTCommon.GetArray(settings, data, info, position);
                    break;
                case DataType.Boolean:
                    data2 = TSTCommon.GetBoolean(data, info);
                    break;
                case DataType.BitString:
                    data2 = (object)TSTCommon.GetBitString(data, info);
                    break;
                case DataType.Int32:
                    data2 = TSTCommon.GetInt32(data, info);
                    break;
                case DataType.UInt32:
                    data2 = TSTCommon.GetUInt32(data, info);
                    break;
                case DataType.OctetString:
                    data2 = TSTCommon.GetOctetString(settings, data, info, knownType);
                    break;
                case DataType.String:
                    data2 = TSTCommon.GetString(data, info, knownType);
                    break;
                case DataType.StringUTF8:
                    data2 = TSTCommon.GetUtfString(data, info, knownType);
                    break;
                case DataType.Bcd:
                    data2 = TSTCommon.GetBcd(data, info);
                    break;
                case DataType.Int8:
                    data2 = TSTCommon.GetInt8(data, info);
                    break;
                case DataType.Int16:
                    data2 = TSTCommon.GetInt16(data, info);
                    break;
                case DataType.UInt8:
                    data2 = TSTCommon.GetUInt8(data, info);
                    break;
                case DataType.UInt16:
                    data2 = TSTCommon.GetUInt16(data, info);
                    break;
                case DataType.CompactArray:
                    data2 = TSTCommon.GetCompactArray(data, info, false);
                    break;
                case DataType.Int64:
                    data2 = TSTCommon.GetInt64(data, info);
                    break;
                case DataType.UInt64:
                    data2 = TSTCommon.GetUInt64(data, info);
                    break;
                case DataType.Enum:
                    data2 = TSTCommon.GetEnum(data, info);
                    break;
                case DataType.Float32:
                    data2 = TSTCommon.Getfloat(data, info);
                    break;
                case DataType.Float64:
                    data2 = TSTCommon.GetDouble(data, info);
                    break;
                case DataType.DateTime:
                    data2 = TSTCommon.GetDateTime(settings, data, info);
                    break;
                case DataType.Date:
                    data2 = TSTCommon.GetDate(data, info);
                    break;
                case DataType.Time:
                    data2 = TSTCommon.GetTime(data, info);
                    break;
                default:
                    throw new Exception("Invalid data type.");
            }
            return data2;
        }

        private static object GetArray(
          GXDLMSSettings settings,
          GXByteBuffer buff,
          GXDataInfo info,
          int index)
        {
            if (info.Count == 0)
                info.Count = TSTCommon.GetObjectCount(buff);
            if (info.xml != null)
                info.xml.AppendStartTag((int)(DataType.None | info.Type), "Qty", info.xml.IntegerToHex((long)info.Count, 2));
            int num1 = buff.Size - buff.Position;
            if (info.Count != 0 && num1 < 1)
            {
                info.Complete = false;
                return (object)null;
            }
            int num2 = index;
            List<object> array = info.Type != DataType.Array ? (List<object>)new GXStructure() : (List<object>)new GXArray();
            int index1;
            for (index1 = info.Index; index1 != info.Count; ++index1)
            {
                GXDataInfo info1 = new GXDataInfo()
                {
                    xml = info.xml
                };
                object data = TSTCommon.GetData(settings, buff, info1);
                if (!info1.Complete)
                {
                    buff.Position = num2;
                    info.Complete = false;
                    break;
                }
                if (info1.Count == info1.Index)
                {
                    num2 = buff.Position;
                    array.Add(data);
                }
            }
            if (info.xml != null)
                info.xml.AppendEndTag((int)((byte)0 + info.Type));
            info.Index = index1;
            return (object)array;
        }

        private static object GetTime(GXByteBuffer buff, GXDataInfo info)
        {
            GXTime time = (GXTime)null;
            if (buff.Size - buff.Position < 4)
            {
                info.Complete = false;
                return (object)null;
            }
            string str = (string)null;
            if (info.xml != null)
                str = TSTCommon.ToHex(buff.Data, false, buff.Position, 4);
            try
            {
                int uint8_1 = (int)buff.GetUInt8();
                int uint8_2 = (int)buff.GetUInt8();
                int uint8_3 = (int)buff.GetUInt8();
                int uint8_4 = (int)buff.GetUInt8();
                int millisecond = uint8_4 == (int)byte.MaxValue ? -1 : uint8_4 * 10;
                time = new GXTime(uint8_1, uint8_2, uint8_3, millisecond);
            }
            catch
            {
                if (info.xml == null)
                    throw;
            }
            if (info.xml != null)
            {
                if (time != null)
                    info.xml.AppendComment(time.ToFormatString());
                info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", (object)str);
            }
            return (object)time;
        }

        private static object GetDate(GXByteBuffer buff, GXDataInfo info)
        {
            GXDate date = (GXDate)null;
            if (buff.Size - buff.Position < 5)
            {
                info.Complete = false;
                return (object)null;
            }
            string str = (string)null;
            if (info.xml != null)
                str = TSTCommon.ToHex(buff.Data, false, buff.Position, 5);
            try
            {
                date = new GXDate((int)buff.GetUInt16(), (int)buff.GetUInt8(), (int)buff.GetUInt8());
                if (buff.GetUInt8() == byte.MaxValue)
                    date.Skip |= DateTimeSkips.DayOfWeek;
            }
            catch (Exception ex)
            {
                if (info.xml == null)
                    throw;
            }
            if (info.xml != null)
            {
                if (date != null)
                    info.xml.AppendComment(date.ToFormatString());
                info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", (object)str);
            }
            return (object)date;
        }

        private static object GetDateTime(GXDLMSSettings settings, GXByteBuffer buff, GXDataInfo info)
        {
            if (buff.Size - buff.Position < 12)
            {
                if (buff.Size - buff.Position < 5)
                    return TSTCommon.GetTime(buff, info);
                if (buff.Size - buff.Position < 6)
                    return TSTCommon.GetDate(buff, info);
                info.Complete = false;
                return (object)null;
            }
            string str = (string)null;
            if (info.xml != null)
                str = TSTCommon.ToHex(buff.Data, false, buff.Position, 12);
            GXDateTime dateTime1 = new GXDateTime();
            try
            {
                int year = (int)buff.GetUInt16();
                if (year == (int)ushort.MaxValue || year == 0)
                {
                    year = DateTime.Now.Year;
                    dateTime1.Skip |= DateTimeSkips.Year;
                }
                int month = (int)buff.GetUInt8();
                if (month == 0 || month == (int)byte.MaxValue)
                {
                    month = 1;
                    dateTime1.Skip |= DateTimeSkips.Month;
                }
                else
                {
                    switch (month)
                    {
                        case 253:
                            month = 1;
                            dateTime1.Skip |= DateTimeSkips.Month;
                            dateTime1.Extra |= DateTimeExtraInfo.DstEnd;
                            break;
                        case 254:
                            month = 1;
                            dateTime1.Skip |= DateTimeSkips.Month;
                            dateTime1.Extra |= DateTimeExtraInfo.DstBegin;
                            break;
                    }
                }
                int day = (int)buff.GetUInt8();
                if (day < 1 || day == (int)byte.MaxValue)
                {
                    day = 1;
                    dateTime1.Skip |= DateTimeSkips.Day;
                }
                else
                {
                    switch (day)
                    {
                        case 253:
                            day = 1;
                            dateTime1.Skip |= DateTimeSkips.Day;
                            break;
                        case 254:
                            day = 1;
                            dateTime1.Skip |= DateTimeSkips.Day;
                            break;
                    }
                }
                byte uint8_1 = buff.GetUInt8();
                if (uint8_1 == byte.MaxValue)
                    dateTime1.Skip |= DateTimeSkips.DayOfWeek;
                else
                    dateTime1.DayOfWeek = (int)uint8_1;
                int hour = (int)buff.GetUInt8();
                if (hour == (int)byte.MaxValue)
                {
                    hour = 0;
                    dateTime1.Skip |= DateTimeSkips.Hour;
                }
                int minute = (int)buff.GetUInt8();
                if (minute == (int)byte.MaxValue)
                {
                    minute = 0;
                    dateTime1.Skip |= DateTimeSkips.Minute;
                }
                int second = (int)buff.GetUInt8();
                if (second == (int)byte.MaxValue)
                {
                    second = 0;
                    dateTime1.Skip |= DateTimeSkips.Second;
                }
                int uint8_2 = (int)buff.GetUInt8();
                int millisecond;
                if (uint8_2 != (int)byte.MaxValue)
                {
                    millisecond = uint8_2 * 10;
                }
                else
                {
                    millisecond = 0;
                    dateTime1.Skip |= DateTimeSkips.Ms;
                }
                int num = (int)buff.GetInt16();
                dateTime1.Status = (ClockStatus)buff.GetUInt8();
                if (settings != null && settings.UseUtc2NormalTime && num != (int)short.MinValue)
                    num = -num;
                if (num != -1 && num != (int)short.MinValue && year != 1 && (dateTime1.Skip & DateTimeSkips.Year) == DateTimeSkips.None)
                {
                    dateTime1.Value = new DateTimeOffset(new DateTime(year, month, day, hour, minute, second, millisecond), new TimeSpan(0, -num, 0));
                }
                else
                {
                    dateTime1.Skip |= DateTimeSkips.Deviation;
                    DateTime dateTime2 = new DateTime(year, month, day, hour, minute, second, millisecond, DateTimeKind.Local);
                    dateTime1.Value = new DateTimeOffset(dateTime2);
                }
            }
            catch
            {
                if (info.xml == null)
                    throw;
                else
                    dateTime1 = (GXDateTime)null;
            }
            if (info.xml != null)
            {
                if (dateTime1 != null)
                    info.xml.AppendComment(dateTime1.ToFormatString());
                info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", (object)str);
            }
            return (object)dateTime1;
        }

        private static object GetDouble(GXByteBuffer buff, GXDataInfo info)
        {
            if (buff.Size - buff.Position < 8)
            {
                info.Complete = false;
                return (object)null;
            }
            double num = buff.GetDouble();
            if (info.xml != null)
            {
                if (info.xml.Comments)
                    info.xml.AppendComment(num.ToString());
                GXByteBuffer buff1 = new GXByteBuffer();
                TSTCommon.SetData((GXDLMSSettings)null, buff1, DataType.Float64, (object)num);
                info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", (object)TSTCommon.ToHex(buff1.Data, false, 1, buff1.Size - 1));
            }
            return (object)num;
        }

        private static object Getfloat(GXByteBuffer buff, GXDataInfo info)
        {
            if (buff.Size - buff.Position < 4)
            {
                info.Complete = false;
                return (object)null;
            }
            float num = buff.GetFloat();
            if (info.xml != null)
            {
                if (info.xml.Comments)
                    info.xml.AppendComment(num.ToString());
                GXByteBuffer buff1 = new GXByteBuffer();
                TSTCommon.SetData((GXDLMSSettings)null, buff1, DataType.Float32, (object)num);
                info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", (object)TSTCommon.ToHex(buff1.Data, false, 1, buff1.Size - 1));
            }
            return (object)num;
        }

        private static object GetEnum(GXByteBuffer buff, GXDataInfo info)
        {
            if (buff.Size - buff.Position < 1)
            {
                info.Complete = false;
                return (object)null;
            }
            byte uint8 = buff.GetUInt8();
            if (info.xml != null)
                info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", (object)info.xml.IntegerToHex((long)uint8, 2));
            return (object)new GXEnum(uint8);
        }

        private static object GetUInt64(GXByteBuffer buff, GXDataInfo info)
        {
            if (buff.Size - buff.Position < 8)
            {
                info.Complete = false;
                return (object)null;
            }
            ulong uint64 = buff.GetUInt64();
            if (info.xml != null)
                info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", (object)info.xml.IntegerToHex(uint64));
            return (object)uint64;
        }

        private static object GetInt64(GXByteBuffer buff, GXDataInfo info)
        {
            if (buff.Size - buff.Position < 8)
            {
                info.Complete = false;
                return (object)null;
            }
            long int64 = buff.GetInt64();
            if (info.xml != null)
                info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", (object)info.xml.IntegerToHex(int64, 16));
            return (object)int64;
        }

        private static object GetUInt16(GXByteBuffer buff, GXDataInfo info)
        {
            if (buff.Size - buff.Position < 2)
            {
                info.Complete = false;
                return (object)null;
            }
            ushort uint16 = buff.GetUInt16();
            if (info.xml != null)
                info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", (object)info.xml.IntegerToHex((long)uint16, 4));
            return (object)uint16;
        }

        private static void GetCompactArrayItem(
          GXDLMSSettings settings,
          GXByteBuffer buff,
          List<object> dt,
          List<object> list)
        {
            List<object> list1 = new List<object>();
            foreach (object dt1 in dt)
            {
                if (dt1 is DataType dt2)
                    TSTCommon.GetCompactArrayItem(settings, buff, dt2, list1, 1);
                else
                    TSTCommon.GetCompactArrayItem(settings, buff, (List<object>)dt1, list1);
            }
            list.Add((object)list1.ToArray());
        }

        private static void GetCompactArrayItem(
          GXDLMSSettings settings,
          GXByteBuffer buff,
          DataType dt,
          List<object> list,
          int len)
        {
            GXDataInfo info = new GXDataInfo() { Type = dt };
            int position = buff.Position;
            switch (dt)
            {
                case DataType.OctetString:
                    while (buff.Position - position < len)
                    {
                        info.Clear();
                        info.Type = dt;
                        list.Add(TSTCommon.GetOctetString(settings, buff, info, false));
                        if (!info.Complete)
                            break;
                    }
                    break;
                case DataType.String:
                    while (buff.Position - position < len)
                    {
                        info.Clear();
                        info.Type = dt;
                        list.Add(TSTCommon.GetString(buff, info, false));
                        if (!info.Complete)
                            break;
                    }
                    break;
                default:
                    while (buff.Position - position < len)
                    {
                        info.Clear();
                        info.Type = dt;
                        list.Add(TSTCommon.GetData((GXDLMSSettings)null, buff, info));
                        if (!info.Complete)
                            break;
                    }
                    break;
            }
        }

        private static void AppendDataTypeAsXml(List<object> cols, GXDataInfo info)
        {
            foreach (object col in cols)
            {
                switch (col)
                {
                    case DataType type:
                        info.xml.AppendEmptyTag(info.xml.GetDataType(type));
                        break;
                    case GXStructure _:
                        info.xml.AppendStartTag(16711682, (string)null, (string)null);
                        TSTCommon.AppendDataTypeAsXml((List<object>)col, info);
                        info.xml.AppendEndTag(16711682);
                        break;
                    default:
                        info.xml.AppendStartTag(16711681, (string)null, (string)null);
                        TSTCommon.AppendDataTypeAsXml((List<object>)col, info);
                        info.xml.AppendEndTag(16711681);
                        break;
                }
            }
        }

        private static void ToString(object it, StringBuilder sb)
        {
            switch (it)
            {
                case byte[] _:
                    sb.Append(TSTCommon.ToHex((byte[])it, true));
                    break;
                case IEnumerable<object> _:
                    bool flag = true;
                    foreach (object it1 in (IEnumerable<object>)it)
                    {
                        flag = false;
                        TSTCommon.ToString(it1, sb);
                    }
                    if (!flag)
                    {
                        --sb.Length;
                        break;
                    }
                    break;
                default:
                    sb.Append(Convert.ToString(it));
                    break;
            }
            sb.Append(";");
        }

        internal static object GetCompactArray(GXByteBuffer buff, GXDataInfo info, bool onlyDataTypes)
        {
            if (buff.Size - buff.Position < 2)
            {
                info.Complete = false;
                return (object)null;
            }
            DataType uint8 = (DataType)buff.GetUInt8();
            if (uint8 == DataType.Array)
                throw new ArgumentException("Invalid compact array data.");
            int objectCount1 = TSTCommon.GetObjectCount(buff);
            List<object> list1 = new List<object>();
            if (uint8 == DataType.Structure)
            {
                List<object> cols = new List<object>();
                TSTCommon.GetDataTypes(buff, cols, objectCount1);
                if (onlyDataTypes)
                    return (object)cols;
                int objectCount2 = buff.Position != buff.Size ? TSTCommon.GetObjectCount(buff) : 0;
                if (info.xml != null)
                {
                    info.xml.AppendStartTag(16711699, (string)null, (string)null);
                    info.xml.AppendStartTag((System.Enum)TranslatorTags.ContentsDescription);
                    TSTCommon.AppendDataTypeAsXml(cols, info);
                    info.xml.AppendEndTag((System.Enum)TranslatorTags.ContentsDescription);
                    if (info.xml.OutputType == TranslatorOutputType.StandardXml)
                    {
                        info.xml.AppendStartTag(65360, (string)null, (string)null, true);
                        info.xml.Append(buff.RemainingHexString(true));
                        info.xml.AppendEndTag((System.Enum)TranslatorTags.ArrayContents, true);
                        info.xml.AppendEndTag(16711699);
                    }
                    else
                        info.xml.AppendStartTag((System.Enum)TranslatorTags.ArrayContents);
                }
                int position = buff.Position;
                while (buff.Position - position < objectCount2)
                {
                    List<object> list2 = new List<object>();
                    for (int index = 0; index != cols.Count; ++index)
                    {
                        if (cols[index] is GXStructure)
                            TSTCommon.GetCompactArrayItem((GXDLMSSettings)null, buff, (List<object>)cols[index], list2);
                        else if (cols[index] is GXArray)
                        {
                            List<object> list3 = new List<object>();
                            if (info.AppendAA)
                                TSTCommon.GetObjectCount(buff);
                            TSTCommon.GetCompactArrayItem((GXDLMSSettings)null, buff, (List<object>)cols[index], list3);
                            List<object> objectList = new List<object>();
                            objectList.AddRange((IEnumerable<object>)list3[0]);
                            list2.Add((object)objectList);
                        }
                        else
                            TSTCommon.GetCompactArrayItem((GXDLMSSettings)null, buff, (DataType)cols[index], list2, 1);
                        if (buff.Position == buff.Size)
                            break;
                    }
                    if (list2.Count >= cols.Count)
                        list1.Add((object)list2);
                    else
                        break;
                }
                if (info.xml != null && info.xml.OutputType == TranslatorOutputType.SimpleXml)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (List<object> objectList in list1)
                    {
                        foreach (object it in objectList)
                            TSTCommon.ToString(it, sb);
                        if (sb.Length != 0)
                            --sb.Length;
                        info.xml.AppendLine(sb.ToString());
                        sb.Length = 0;
                    }
                }
                if (info.xml != null && info.xml.OutputType == TranslatorOutputType.SimpleXml)
                {
                    info.xml.AppendEndTag((System.Enum)TranslatorTags.ArrayContents);
                    info.xml.AppendEndTag(16711699);
                }
                return (object)list1;
            }
            if (info.xml != null)
            {
                info.xml.AppendStartTag(16711699, (string)null, (string)null);
                info.xml.AppendStartTag((System.Enum)TranslatorTags.ContentsDescription);
                info.xml.AppendEmptyTag((int)((byte)0 + uint8));
                info.xml.AppendEndTag((System.Enum)TranslatorTags.ContentsDescription);
                info.xml.AppendStartTag(65360, (string)null, (string)null, true);
                if (info.xml.OutputType == TranslatorOutputType.StandardXml)
                {
                    info.xml.Append(buff.RemainingHexString(true));
                    info.xml.AppendEndTag((System.Enum)TranslatorTags.ArrayContents, true);
                    info.xml.AppendEndTag(16711699);
                }
            }
            TSTCommon.GetCompactArrayItem((GXDLMSSettings)null, buff, uint8, list1, objectCount1);
            if (info.xml != null && info.xml.OutputType == TranslatorOutputType.SimpleXml)
            {
                foreach (object bytes in list1)
                {
                    if (bytes is byte[])
                        info.xml.Append(TSTCommon.ToHex((byte[])bytes, true));
                    else
                        info.xml.Append(Convert.ToString(bytes));
                    info.xml.Append(";");
                }
                if (list1.Count != 0)
                    info.xml.SetXmlLength(info.xml.GetXmlLength() - 1);
                info.xml.AppendEndTag((System.Enum)TranslatorTags.ArrayContents, true);
                info.xml.AppendEndTag(16711699);
            }
            return (object)list1;
        }

        private static object GetUInt8(GXByteBuffer buff, GXDataInfo info)
        {
            if (buff.Size - buff.Position < 1)
            {
                info.Complete = false;
                return (object)null;
            }
            byte uint8 = buff.GetUInt8();
            if (info.xml != null)
                info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", (object)info.xml.IntegerToHex((long)uint8, 2));
            return (object)uint8;
        }

        private static object GetInt16(GXByteBuffer buff, GXDataInfo info)
        {
            if (buff.Size - buff.Position < 2)
            {
                info.Complete = false;
                return (object)null;
            }
            short int16 = buff.GetInt16();
            if (info.xml != null)
                info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", (object)info.xml.IntegerToHex((long)int16, 4));
            return (object)int16;
        }

        private static object GetInt8(GXByteBuffer buff, GXDataInfo info)
        {
            if (buff.Size - buff.Position < 1)
            {
                info.Complete = false;
                return (object)null;
            }
            sbyte int8 = buff.GetInt8();
            if (info.xml != null)
                info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", (object)int8);
            return (object)int8;
        }

        private static object GetBcd(GXByteBuffer buff, GXDataInfo info)
        {
            if (buff.Size - buff.Position < 1)
            {
                info.Complete = false;
                return (object)null;
            }
            byte uint8 = buff.GetUInt8();
            if (info.xml != null)
                info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", (object)uint8);
            return (object)uint8;
        }

        private static object GetUtfString(GXByteBuffer buff, GXDataInfo info, bool knownType)
        {
            int count;
            if (knownType)
            {
                count = buff.Size;
            }
            else
            {
                count = TSTCommon.GetObjectCount(buff);
                if (buff.Size - buff.Position < count)
                {
                    info.Complete = false;
                    return (object)null;
                }
            }
            object stringUtf8 = count <= 0 ? (object)"" : (object)buff.GetStringUtf8(buff.Position, count);
            if (info.xml != null)
                info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", stringUtf8);
            return stringUtf8;
        }

        private static object GetOctetString(
          GXDLMSSettings settings,
          GXByteBuffer buff,
          GXDataInfo info,
          bool knownType)
        {
            int length;
            if (knownType)
            {
                length = buff.Size;
            }
            else
            {
                length = TSTCommon.GetObjectCount(buff);
                if (buff.Size - buff.Position < length)
                {
                    info.Complete = false;
                    return (object)null;
                }
            }
            byte[] numArray = new byte[length];
            buff.Get(numArray);
            object octetString = (object)numArray;
            if (info.xml != null)
            {
                if (info.xml.Comments && numArray.Length != 0)
                {
                    if (numArray.Length == 6 && numArray[5] == byte.MaxValue)
                    {
                        info.xml.AppendComment(TSTCommon.ToLogicalName((object)numArray));
                    }
                    else
                    {
                        bool flag = true;
                        if (numArray.Length == 12 || numArray.Length == 5 || numArray.Length == 4)
                        {
                            try
                            {
                                DataType type = numArray.Length != 12 ? (numArray.Length != 5 ? DataType.Time : DataType.Date) : DataType.DateTime;
                                GXDateTime gxDateTime = (GXDateTime)GXDLMSClient.ChangeType(numArray, type, settings.UseUtc2NormalTime);
                                if (gxDateTime.Value != (DateTimeOffset)DateTime.MaxValue)
                                {
                                    info.xml.AppendComment(gxDateTime.ToString());
                                    flag = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            foreach (char ch in numArray)
                            {
                                if (ch < ' ' || ch > '~')
                                {
                                    flag = false;
                                    break;
                                }
                            }
                        }
                        if (flag)
                            info.xml.AppendComment(Encoding.ASCII.GetString(numArray));
                    }
                }
                info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", (object)TSTCommon.ToHex(numArray, false));
            }
            return octetString;
        }

        private static object GetBoolean(GXByteBuffer buff, GXDataInfo info)
        {
            if (buff.Size - buff.Position < 1)
            {
                info.Complete = false;
                return (object)null;
            }
            byte uint8 = buff.GetUInt8();
            if (info.xml != null)
                info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", uint8 != (byte)0 ? (object)"true" : (object)"false");
            return (object)(uint8 > (byte)0);
        }

        private static object GetString(GXByteBuffer buff, GXDataInfo info, bool knownType)
        {
            int count;
            if (knownType)
            {
                count = buff.Size;
            }
            else
            {
                count = TSTCommon.GetObjectCount(buff);
                if (buff.Size - buff.Position < count)
                {
                    info.Complete = false;
                    return (object)null;
                }
            }
            string str = count <= 0 ? "" : buff.GetString(count);
            if (info.xml != null)
            {
                if (info.xml.ShowStringAsHex)
                    info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", (object)TSTCommon.ToHex(buff.Data, false, buff.Position - count, count));
                else
                    info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", (object)str.Replace('"', '\''));
            }
            return (object)str;
        }

        private static object GetUInt32(GXByteBuffer buff, GXDataInfo info)
        {
            if (buff.Size - buff.Position < 4)
            {
                info.Complete = false;
                return (object)null;
            }
            uint uint32 = buff.GetUInt32();
            if (info.xml != null)
                info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", (object)uint32);
            return (object)uint32;
        }

        private static object GetInt32(GXByteBuffer buff, GXDataInfo info)
        {
            if (buff.Size - buff.Position < 4)
            {
                info.Complete = false;
                return (object)null;
            }
            int int32 = buff.GetInt32();
            if (info.xml != null)
                info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", (object)int32);
            return (object)int32;
        }

        private static GXBitString GetBitString(GXByteBuffer buff, GXDataInfo info)
        {
            int objectCount = TSTCommon.GetObjectCount(buff);
            double d = (double)objectCount / 8.0;
            if (objectCount % 8 != 0)
                ++d;
            int num = (int)Math.Floor(d);
            if (buff.Size - buff.Position < num)
            {
                info.Complete = false;
                return (GXBitString)null;
            }
            StringBuilder sb = new StringBuilder();
            for (; objectCount > 0; objectCount -= 8)
                TSTCommon.ToBitString(sb, buff.GetUInt8(), objectCount);
            if (info.xml != null)
                info.xml.AppendLine(info.xml.GetDataType(info.Type), "Value", (object)sb.ToString());
            return new GXBitString(sb.ToString());
        }

        private static void GetDataTypes(GXByteBuffer buff, List<object> cols, int len)
        {
            for (int index1 = 0; index1 != len; ++index1)
            {
                DataType uint8 = (DataType)buff.GetUInt8();
                switch (uint8)
                {
                    case DataType.Array:
                        int uint16 = (int)buff.GetUInt16();
                        List<object> objectList1 = new List<object>();
                        List<object> objectList2 = new List<object>();
                        TSTCommon.GetDataTypes(buff, objectList1, 1);
                        for (int index2 = 0; index2 != uint16; ++index2)
                            objectList2.AddRange((IEnumerable<object>)objectList1);
                        cols.Add((object)objectList2);
                        break;
                    case DataType.Structure:
                        List<object> cols1 = new List<object>();
                        TSTCommon.GetDataTypes(buff, cols1, (int)buff.GetUInt8());
                        cols.Add((object)cols1.ToArray());
                        break;
                    default:
                        cols.Add((object)uint8);
                        break;
                }
            }
        }

        public static void DatatoXml(object value, GXDLMSTranslatorStructure xml)
        {
            if (value == null)
                xml.AppendEmptyTag(xml.GetDataType(DataType.None));
            else if (value is GXStructure)
            {
                xml.AppendStartTag(16711682, (string)null, (string)null);
                foreach (object obj in (List<object>)value)
                    TSTCommon.DatatoXml(obj, xml);
                xml.AppendEndTag(16711682);
            }
            else if (value is GXArray)
            {
                xml.AppendStartTag(16711681, (string)null, (string)null);
                foreach (object obj in (List<object>)value)
                    TSTCommon.DatatoXml(obj, xml);
                xml.AppendEndTag(16711681);
            }
            else if (value.GetType().IsArray)
            {
                xml.AppendStartTag(16711681, (string)null, (string)null);
                foreach (object obj in (IEnumerable)value)
                    TSTCommon.DatatoXml(obj, xml);
                xml.AppendEndTag(16711681);
            }
            else if (value is IPAddress)
            {
                xml.AppendLine(16711689, (string)null, (object)((IPAddress)value).GetAddressBytes());
            }
            else
            {
                DataType dlmsDataType = TSTCommon.GetDLMSDataType(value.GetType());
                xml.AppendLine((int)((byte)0 + dlmsDataType), (string)null, value);
            }
        }
        */
        private delegate DialogResult ShowDialogEventHandler(
          IWin32Window parent,
          string title,
          string str);
    }

}
