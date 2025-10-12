using Gurux.Common;
using Gurux.DLMS.Enums;
using Gurux.DLMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterReader.DLMSNetSerialCommunication
{
    public static class GXCommonNotDefinedMethods
    {
        #region GXCommon not defined methods
        ///<summary>
        ///Convert object to DLMS bytes.
        ///</summary>
        ///<param name="settings">DLMS settings.</param>
        ///<param name="buff">Byte buffer where data is write.</param>
        ///<param name="dataType">Data type.</param>
        ///<param name="value">Added Value.</param>
        public static void SetData(GXDLMSSettings settings, GXByteBuffer buff, DataType type, object value)
        {
            if ((type == DataType.Array || type == DataType.Structure) && value is byte[])
            {
                // If byte array is added do not add type.
                buff.Set((byte[])value);
                return;
            }
            buff.SetUInt8((byte)type);
            switch (type)
            {
                case DataType.None:
                    break;
                case DataType.Boolean:
                    if (Convert.ToBoolean(value))
                    {
                        buff.SetUInt8(1);
                    }
                    else
                    {
                        buff.SetUInt8(0);
                    }
                    break;
                case DataType.Int8:
                    buff.SetUInt8((byte)Convert.ToSByte(value));
                    break;
                case DataType.UInt8:
                case DataType.Enum:
                    buff.SetUInt8(Convert.ToByte(value));
                    break;
                case DataType.Int16:
                    if (value is UInt16)
                    {
                        buff.SetUInt16((UInt16)value);
                    }
                    else
                    {
                        int v = Convert.ToInt32(value);
                        if (v == 0x8000)
                        {
                            buff.SetUInt16(0x8000);
                        }
                        else
                        {
                            buff.SetInt16((short)v);
                        }
                    }
                    break;
                case DataType.UInt16:
                    buff.SetUInt16(Convert.ToUInt16(value));
                    break;
                case DataType.Int32:
                    if (value is DateTime)
                    {
                        buff.SetUInt32((UInt32)GXDateTime.ToUnixTime((DateTime)value));
                    }
                    else if (value is GXDateTime)
                    {
                        buff.SetUInt32((UInt32)GXDateTime.ToUnixTime(((GXDateTime)value).Value.DateTime));
                    }
                    else
                    {
                        buff.SetInt32(Convert.ToInt32(value));
                    }
                    break;
                case DataType.UInt32:
                    if (value is DateTime)
                    {
                        buff.SetUInt32((UInt32)GXDateTime.ToUnixTime((DateTime)value));
                    }
                    else if (value is GXDateTime)
                    {
                        buff.SetUInt32((UInt32)GXDateTime.ToUnixTime(((GXDateTime)value).Value.DateTime));
                    }
                    else
                    {
                        buff.SetUInt32(Convert.ToUInt32(value));
                    }
                    break;
                case DataType.Int64:
                    buff.SetUInt64(Convert.ToUInt64(value));
                    break;
                case DataType.UInt64:
                    buff.SetUInt64(Convert.ToUInt64(value));
                    break;
                case DataType.Float32:
                    buff.SetFloat(Convert.ToSingle(value));
                    break;
                case DataType.Float64:
                    buff.SetDouble(Convert.ToDouble(value));
                    break;
                case DataType.BitString:
                    SetBitString(buff, value, true);
                    break;
                case DataType.String:
                    SetString(buff, value);
                    break;
                case DataType.StringUTF8:
                    SetUtcString(buff, value);
                    break;
                case DataType.OctetString:
                    if (value is GXDate)
                    {
                        //Add size
                        buff.SetUInt8(5);
                        SetDate(settings, buff, value);
                    }
                    else if (value is GXTime)
                    {
                        //Add size
                        buff.SetUInt8(4);
                        SetTime(settings, buff, value);
                    }
                    else if (value is GXDateTime || value is DateTime)
                    {
                        //Add size
                        buff.SetUInt8(12);
                        SetDateTime(settings, buff, value);
                    }
                    else
                    {
                        SetOctetString(buff, value);
                    }
                    break;
                case DataType.Array:
                case DataType.Structure:
                    SetArray(settings, buff, value);
                    break;
                case DataType.Bcd:
                    SetBcd(buff, value);
                    break;
                case DataType.CompactArray:
                    throw new Exception("Invalid data type.");
                case DataType.DateTime:
                    SetDateTime(settings, buff, value);
                    break;
                case DataType.Date:
                    SetDate(settings, buff, value);
                    break;
                case DataType.Time:
                    SetTime(settings, buff, value);
                    break;
                default:
                    throw new Exception("Invalid data type.");
            }
        }
        ///<summary>
        ///Convert time to DLMS bytes.
        ///</summary>
        ///<param name="buff">
        ///Byte buffer where data is write.
        ///</param>
        ///<param name="value">
        ///Added value.
        ///</param>
        private static void SetTime(GXDLMSSettings settings, GXByteBuffer buff, object value)
        {
            GXDateTime dt;
            if (value is GXDateTime)
            {
                dt = (GXDateTime)value;
            }
            else if (value is DateTime)
            {
                dt = new GXDateTime((DateTime)value);
            }
            else if (value is DateTimeOffset)
            {
                dt = new GXDateTime((DateTimeOffset)value);
            }
            else if (value is string)
            {
                dt = DateTime.Parse((string)value);
            }
            else
            {
                throw new Exception("Invalid date format.");
            }
            //Add additional date time skips.
            if (settings != null && settings.DateTimeSkips != DateTimeSkips.None)
            {
                dt.Skip |= settings.DateTimeSkips;
            }
            //Add time.
            if ((dt.Skip & DateTimeSkips.Hour) != 0)
            {
                buff.SetUInt8(0xFF);
            }
            else
            {
                buff.SetUInt8((byte)dt.Value.Hour);
            }

            if ((dt.Skip & DateTimeSkips.Minute) != 0)
            {
                buff.SetUInt8(0xFF);
            }
            else
            {
                buff.SetUInt8((byte)dt.Value.Minute);
            }

            if ((dt.Skip & DateTimeSkips.Second) != 0)
            {
                buff.SetUInt8(0xFF);
            }
            else
            {
                buff.SetUInt8((byte)dt.Value.Second);
            }

            //Hundredths of second is not used.
            if ((dt.Skip & DateTimeSkips.Ms) != 0)
            {
                buff.SetUInt8(0xFF);
            }
            else
            {
                buff.SetUInt8((byte)(dt.Value.Millisecond / 10));
            }
        }

        ///<summary>
        ///Convert date to DLMS bytes.
        ///</summary>
        ///<param name="buff">
        ///Byte buffer where data is write.
        ///</param>
        ///<param name="value">
        ///Added value.
        ///</param>
        private static void SetDate(GXDLMSSettings settings, GXByteBuffer buff, object value)
        {
            GXDateTime dt;
            if (value is GXDateTime)
            {
                dt = (GXDateTime)value;
            }
            else if (value is DateTime)
            {
                dt = new GXDateTime((DateTime)value);
            }
            else if (value is DateTimeOffset)
            {
                dt = new GXDateTime((DateTimeOffset)value);
            }
            else if (value is string)
            {
                dt = DateTime.Parse((string)value);
            }
            else
            {
                throw new Exception("Invalid date format.");
            }
            //Add additional date time skips.
            if (settings != null && settings.DateTimeSkips != DateTimeSkips.None)
            {
                dt.Skip |= settings.DateTimeSkips;
            }
            // Add year.
            if ((dt.Skip & DateTimeSkips.Year) != 0)
            {
                buff.SetUInt16(0xFFFF);
            }
            else
            {
                buff.SetUInt16((UInt16)dt.Value.Year);
            }
            // Add month
            if ((dt.Skip & DateTimeSkips.Month) != 0)
            {
                buff.SetUInt8(0xFF);
            }
            else
            {
                if ((dt.Extra & DateTimeExtraInfo.DstBegin) != 0)
                {
                    buff.SetUInt8(0xFE);
                }
                else if ((dt.Extra & DateTimeExtraInfo.DstEnd) != 0)
                {
                    buff.SetUInt8(0xFD);
                }
                else
                {
                    buff.SetUInt8((byte)dt.Value.Month);
                }
            }
            // Add day
            if ((dt.Skip & DateTimeSkips.Day) != 0)
            {
                buff.SetUInt8(0xFF);
            }
            else
            {
                if ((dt.Extra & DateTimeExtraInfo.LastDay) != 0)
                {
                    buff.SetUInt8(0xFE);
                }
                else if ((dt.Extra & DateTimeExtraInfo.LastDay2) != 0)
                {
                    buff.SetUInt8(0xFD);
                }
                else
                {
                    buff.SetUInt8((byte)dt.Value.Day);
                }
            }
            // Add week day
            if ((dt.Skip & DateTimeSkips.DayOfWeek) != 0)
            {
                buff.SetUInt8(0xFF);
            }
            else
            {
                if (dt.Value.DayOfWeek == DayOfWeek.Sunday)
                {
                    buff.SetUInt8(7);
                }
                else
                {
                    buff.SetUInt8((byte)(dt.Value.DayOfWeek));
                }
            }
        }

        ///<summary>
        ///Convert date time to DLMS bytes.
        ///</summary>
        ///<param name="buff">
        ///Byte buffer where data is write.
        ///</param>
        ///<param name="value">
        ///Added value.
        ///</param>
        private static void SetDateTime(GXDLMSSettings settings, GXByteBuffer buff, object value)
        {
            GXDateTime dt;
            if (value is GXDateTime)
            {
                dt = (GXDateTime)value;
            }
            else if (value is DateTime)
            {
                dt = new GXDateTime((DateTime)value);
                dt.Skip = dt.Skip | DateTimeSkips.Ms;
            }
            else if (value is string)
            {
                dt = new GXDateTime(DateTime.Parse((string)value));
                dt.Skip = dt.Skip | DateTimeSkips.Ms;
            }
            else
            {
                throw new Exception("Invalid date format.");
            }
            //Add additional date time skips.
            if (settings != null && settings.DateTimeSkips != DateTimeSkips.None)
            {
                dt.Skip |= settings.DateTimeSkips;
            }
            if (dt.Value.UtcDateTime == DateTime.MinValue)
            {
                dt.Value = DateTime.SpecifyKind(new DateTime(2000, 1, 1).Date, DateTimeKind.Utc);
            }
            else if (dt.Value.UtcDateTime == DateTime.MaxValue)
            {
                dt.Value = DateTime.SpecifyKind(DateTime.Now.AddYears(1).Date, DateTimeKind.Utc);
            }
            DateTimeOffset tm = dt.Value;
            if ((dt.Skip & DateTimeSkips.Year) == 0)
            {
                buff.SetUInt16((ushort)tm.Year);
            }
            else
            {
                buff.SetUInt16(0xFFFF);
            }
            if ((dt.Skip & DateTimeSkips.Month) == 0)
            {
                if ((dt.Extra & DateTimeExtraInfo.DstBegin) != 0)
                {
                    buff.SetUInt8(0xFE);
                }
                else if ((dt.Extra & DateTimeExtraInfo.DstEnd) != 0)
                {
                    buff.SetUInt8(0xFD);
                }
                else
                {
                    buff.SetUInt8((byte)tm.Month);
                }
            }
            else
            {
                buff.SetUInt8(0xFF);
            }
            if ((dt.Skip & DateTimeSkips.Day) == 0)
            {
                if ((dt.Extra & DateTimeExtraInfo.LastDay) != 0)
                {
                    buff.SetUInt8(0xFE);
                }
                else if ((dt.Extra & DateTimeExtraInfo.LastDay2) != 0)
                {
                    buff.SetUInt8(0xFD);
                }
                else
                {
                    buff.SetUInt8((byte)tm.Day);
                }
            }
            else
            {
                buff.SetUInt8(0xFF);
            }
            // Add week day
            if ((dt.Skip & DateTimeSkips.DayOfWeek) != 0)
            {
                //Skip.
                buff.SetUInt8(0xFF);
            }
            else
            {
                if (dt.DayOfWeek == 0)
                {
                    byte d = (byte)dt.Value.DayOfWeek;
                    //If Sunday.
                    if (d == 0)
                    {
                        d = 7;
                    }
                    buff.SetUInt8(d);
                }
                else
                {
                    buff.SetUInt8((byte)(dt.DayOfWeek));
                }
            }
            //Add time.
            if ((dt.Skip & DateTimeSkips.Hour) == 0)
            {
                buff.SetUInt8((byte)tm.Hour);
            }
            else
            {
                buff.SetUInt8(0xFF);
            }

            if ((dt.Skip & DateTimeSkips.Minute) == 0)
            {
                buff.SetUInt8((byte)tm.Minute);
            }
            else
            {
                buff.SetUInt8(0xFF);
            }
            if ((dt.Skip & DateTimeSkips.Second) == 0)
            {
                buff.SetUInt8((byte)tm.Second);
            }
            else
            {
                buff.SetUInt8(0xFF);
            }

            if ((dt.Skip & DateTimeSkips.Ms) == 0)
            {
                buff.SetUInt8((byte)(tm.Millisecond / 10));
            }
            else
            {
                buff.SetUInt8((byte)0xFF); //Hundredths of second is not used.
            }
            //Add deviation.
            if ((dt.Skip & DateTimeSkips.Deviation) == 0)
            {
                Int16 deviation = (Int16)dt.Value.Offset.TotalMinutes;
                if (settings != null && settings.UseUtc2NormalTime)
                {
                    buff.SetInt16(deviation);
                }
                else
                {
                    buff.SetInt16((Int16)(-deviation));
                }
            }
            else //deviation not used  .
            {
                buff.SetUInt16(0x8000);
            }
            //Add clock_status
            if ((dt.Skip & DateTimeSkips.Status) == 0)
            {
                buff.SetUInt8((byte)dt.Status);
            }
            else //Status is not used.
            {
                buff.SetUInt8((byte)0xFF);
            }
        }

        ///<summary>
        ///Convert BCD to DLMS bytes.
        ///</summary>
        ///<param name="buff">
        ///Byte buffer where data is write.
        ///</param>
        ///<param name="value">
        ///Added value.
        ///</param>
        private static void SetBcd(GXByteBuffer buff, object value)
        {
            //Standard supports only size of byte.
            buff.SetUInt8(Convert.ToByte(value));
        }

        ///<summary>
        ///Convert Array to DLMS bytes.
        ///</summary>
        ///<param name="buff">
        ///Byte buffer where data is write.
        ///</param>
        ///<param name="value">
        ///Added value.
        ///</param>
        private static void SetArray(GXDLMSSettings settings, GXByteBuffer buff, object value)
        {
            if (value != null)
            {
                List<object> tmp;
                if (value is List<object>)
                {
                    tmp = (List<object>)value;
                }
                else
                {
                    tmp = new List<object>();
                    tmp.AddRange((object[])value);
                }
                SetObjectCount(tmp.Count, buff);
                foreach (object it in tmp)
                {
                    DataType dt = GXDLMSConverter.GetDLMSDataType(it);
                    SetData(settings, buff, dt, it);
                }
            }
            else
            {
                SetObjectCount(0, buff);
            }
        }

        ///<summary>
        ///Convert Octet string to DLMS bytes.
        ///</summary>
        ///<param name="buff">
        ///Byte buffer where data is write.
        ///</param>
        ///<param name="value">
        ///Added value.
        ///</param>
        private static void SetOctetString(GXByteBuffer buff, object value)
        {
            if (value is string)
            {
                byte[] tmp = GXCommon.HexToBytes((string)value);
                SetObjectCount(tmp.Length, buff);
                buff.Set(tmp);
            }
            else if (value is byte[] || value is sbyte[])
            {
                SetObjectCount(((byte[])value).Length, buff);
                buff.Set((byte[])value);
            }
            else if (value == null)
            {
                SetObjectCount(0, buff);
            }
            else
            {
                throw new Exception("Invalid data type.");
            }
        }

        ///<summary>
        ///Convert UTC string to DLMS bytes.
        ///</summary>
        ///<param name="buff">
        ///Byte buffer where data is write.
        ///</param>
        ///<param name="value">
        ///Added value.
        ///</param>
        private static void SetUtcString(GXByteBuffer buff, object value)
        {
            if (value != null)
            {
                byte[] tmp = ASCIIEncoding.UTF8.GetBytes(Convert.ToString(value));
                SetObjectCount(tmp.Length, buff);
                buff.Set(tmp);
            }
            else
            {
                buff.SetUInt8(0);
            }
        }

        ///<summary>
        ///Convert ASCII string to DLMS bytes.
        ///</summary>
        ///<param name="buff">
        ///Byte buffer where data is write.
        ///</param>
        ///<param name="value">
        ///Added value.
        ///</param>
        private static void SetString(GXByteBuffer buff, object value)
        {
            if (value is byte[])
            {
                byte[] tmp = (byte[])value;
                SetObjectCount(tmp.Length, buff);
                buff.Set(tmp);
            }
            if (value != null)
            {
                string str = Convert.ToString(value);
                SetObjectCount(str.Length, buff);
                buff.Set(ASCIIEncoding.ASCII.GetBytes(str));
            }
            else
            {
                buff.SetUInt8(0);
            }
        }

        ///<summary>
        ///Convert Bit string to DLMS bytes.
        ///</summary>
        ///<param name="buff">
        ///Byte buffer where data is write.
        ///</param>
        ///<param name="value">
        ///Added value.
        ///</param>
        internal static void SetBitString(GXByteBuffer buff, object value, bool addCount)
        {
            if (value is GXBitString)
            {
                value = (value as GXBitString).Value;
            }
            if (value is string)
            {
                byte val = 0;
                String str = (String)value;
                if (addCount)
                {
                    SetObjectCount(str.Length, buff);
                }
                int index = 7;
                for (int pos = 0; pos != str.Length; ++pos)
                {
                    char it = str[pos];
                    if (it == '1')
                    {
                        val |= (byte)(1 << index);
                    }
                    else if (it != '0')
                    {
                        throw new ArgumentException("Not a bit string.");
                    }
                    --index;
                    if (index == -1)
                    {
                        index = 7;
                        buff.SetUInt8(val);
                        val = 0;
                    }
                }
                if (index != 7)
                {
                    buff.SetUInt8(val);
                }
            }
            else if (value is byte[])
            {
                byte[] arr = (byte[])value;
                SetObjectCount(8 * arr.Length, buff);
                buff.Set(arr);
            }
            else if (value == null)
            {
                buff.SetUInt8(0);
            }
            else if (value is byte)
            {
                SetObjectCount(8, buff);
                buff.SetUInt8((byte)value);
            }
            else
            {
                throw new Exception("BitString must give as string.");
            }
        }
        /// <summary>
        /// Set item count.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="buff"></param>
        internal static void SetObjectCount(int count, GXByteBuffer buff)
        {
            if (count < 0x80)
            {
                buff.SetUInt8((byte)count);
            }
            else if (count < 0x100)
            {
                buff.SetUInt8(0x81);
                buff.SetUInt8((byte)count);
            }
            else if (count < 0x10000)
            {
                buff.SetUInt8(0x82);
                buff.SetUInt16((UInt16)count);
            }
            else
            {
                buff.SetUInt8(0x84);
                buff.SetUInt32((UInt32)count);
            }
        }

        #endregion

    }
}
