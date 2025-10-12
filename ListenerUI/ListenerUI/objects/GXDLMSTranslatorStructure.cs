
using Gurux.DLMS.Enums;
using Indali.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Indali.Security.Enum
{
    public class GXDLMSTranslatorStructure
    {
        private int offset;
        internal StringBuilder sb = new StringBuilder();
        private SortedList<int, string> tags;
        private bool showNumericsAsHex;

        public TranslatorOutputType OutputType { get; private set; }

        public bool OmitNameSpace { get; private set; }

        public int Offset
        {
            get => this.offset;
            set => this.offset = value >= 0 ? value : throw new ArgumentException("offset");
        }

        public string GetDataType(DataType type) => this.GetTag((int)((byte)0 + type));

        public bool ShowStringAsHex { get; set; }

        public bool Comments { get; set; }

        public bool IgnoreSpaces { get; set; }

        public GXDLMSTranslatorStructure(
          TranslatorOutputType type,
          bool omitNameSpace,
          bool numericsAshex,
          bool hex,
          bool comments,
          SortedList<int, string> list)
        {
            this.OutputType = type;
            this.OmitNameSpace = omitNameSpace;
            this.showNumericsAsHex = numericsAshex;
            this.ShowStringAsHex = hex;
            this.Comments = comments;
            this.tags = list;
        }

        public override string ToString() => this.sb.ToString();

        private void AppendSpaces()
        {
            if (this.IgnoreSpaces)
                this.sb.Append(' ');
            else
                this.sb.Append(' ', 2 * this.offset);
        }

        public void AppendLine(string str)
        {
            if (this.IgnoreSpaces)
            {
                this.sb.Append(str);
            }
            else
            {
                this.AppendSpaces();
                this.sb.AppendLine(str);
            }
        }

        private string GetTag(int tag)
        {
            return this.OutputType == TranslatorOutputType.SimpleXml || this.OmitNameSpace ? this.tags[tag] : "x:" + this.tags[tag];
        }
        /*
        public void AppendLine(Enum tag, string name, object value)
        {
            this.AppendLine(Convert.ToInt32((object)tag), name, value);
        }
        */
        public void AppendLine(int tag, string name, object value)
        {
            this.AppendLine(this.GetTag(tag), name, value);
        }

        public void AppendLine(string tag, string name, object value)
        {
            this.AppendSpaces();
            this.sb.Append('<');
            this.sb.Append(tag);
            if (this.OutputType == TranslatorOutputType.SimpleXml)
            {
                this.sb.Append(' ');
                if (name == null)
                    this.sb.Append("Value");
                else
                    this.sb.Append(name);
                this.sb.Append("=\"");
            }
            else
                this.sb.Append('>');
            switch (value)
            {
                case byte num1:
                    this.sb.Append(this.IntegerToHex((long)num1, 2));
                    break;
                case sbyte num2:
                    this.sb.Append(this.IntegerToHex((long)num2, 2));
                    break;
                case ushort num3:
                    this.sb.Append(this.IntegerToHex((long)num3, 4));
                    break;
                case short num4:
                    this.sb.Append(this.IntegerToHex((long)num4, 4));
                    break;
                case uint num5:
                    this.sb.Append(this.IntegerToHex((long)num5, 8));
                    break;
                case int num6:
                    this.sb.Append(this.IntegerToHex((long)num6, 8));
                    break;
                case ulong num7:
                    this.sb.Append(this.IntegerToHex(num7));
                    break;
                case long num8:
                    this.sb.Append(this.IntegerToHex(num8, 16));
                    break;
                case byte[] _:
                    this.sb.Append(TSTCommon.ToHex((byte[])value, true));
                    break;
                case sbyte[] _:
                    this.sb.Append(TSTCommon.ToHex((byte[])value, true));
                    break;
                default:
                    this.sb.Append(Convert.ToString(value));
                    break;
            }
            if (this.OutputType == TranslatorOutputType.SimpleXml)
            {
                this.sb.Append("\" />");
            }
            else
            {
                this.sb.Append("</");
                this.sb.Append(tag);
                this.sb.Append('>');
            }
            this.sb.Append('\r');
            this.sb.Append('\n');
        }

        public void StartComment(string comment)
        {
            if (!this.Comments)
                return;
            this.AppendSpaces();
            this.sb.Append("<!--");
            this.sb.Append(comment);
            this.sb.Append('\r');
            this.sb.Append('\n');
            ++this.offset;
        }

        public void EndComment()
        {
            if (!this.Comments)
                return;
            --this.offset;
            this.AppendSpaces();
            this.sb.Append("-->");
            this.sb.Append('\r');
            this.sb.Append('\n');
        }

        public void AppendComment(string comment)
        {
            if (!this.Comments)
                return;
            this.AppendSpaces();
            this.sb.Append("<!--");
            this.sb.Append(comment);
            this.sb.Append("-->");
            this.sb.Append('\r');
            this.sb.Append('\n');
        }

        public void Append(string value) => this.sb.Append(value);

        public void Append(int tag, bool start)
        {
            if (start)
                this.sb.Append('<');
            else
                this.sb.Append("</");
            this.sb.Append(this.GetTag(tag));
            this.sb.Append('>');
        }
        /*
        public void AppendStartTag(Enum tag, string name, string value)
        {
            this.AppendStartTag(Convert.ToInt32((object)tag), name, value);
        }
        */
        public void AppendStartTag(int tag, string name, string value)
        {
            this.AppendStartTag(tag, name, value, false);
        }

        public void AppendStartTag(int tag, string name, string value, bool plain)
        {
            this.AppendSpaces();
            this.sb.Append('<');
            this.sb.Append(this.GetTag(tag));
            if (this.OutputType == TranslatorOutputType.SimpleXml && name != null)
            {
                this.sb.Append(' ');
                this.sb.Append(name);
                this.sb.Append("=\"");
                this.sb.Append(value);
                this.sb.Append("\" >");
            }
            else
                this.sb.Append(">");
            if (!plain)
                this.sb.AppendLine("");
            ++this.offset;
        }
        /*
        public void AppendStartTag(Enum cmd)
        {
            this.AppendSpaces();
            this.sb.Append("<");
            this.sb.Append(this.GetTag(Convert.ToInt32((object)cmd)));
            this.sb.AppendLine(">");
            ++this.offset;
        }
        
        public void AppendStartTag(Command cmd, Enum type)
        {
            this.AppendSpaces();
            this.sb.Append("<");
            this.sb.Append(this.GetTag((int)cmd << 8 | (int)Convert.ToByte((object)type)));
            this.sb.AppendLine(">");
            ++this.offset;
        }
        
        public void AppendEndTag(Enum cmd) => this.AppendEndTag(Convert.ToInt32((object)cmd));
        
        public void AppendEndTag(Enum cmd, bool plain)
        {
            this.AppendEndTag(Convert.ToInt32((object)cmd), plain);
        }
        
        public void AppendEndTag(Command cmd, Enum type)
        {
            this.AppendEndTag((int)cmd << 8 | (int)Convert.ToByte((object)type));
        }
        */
        public void AppendEndTag(int tag) => this.AppendEndTag(tag, false);

        public void AppendEndTag(int tag, bool plain)
        {
            --this.Offset;
            if (!plain)
                this.AppendSpaces();
            this.sb.Append("</");
            this.sb.Append(this.GetTag(tag));
            this.sb.Append(">");
            this.AppendLine("");
        }

        public void AppendEmptyTag(int tag) => this.AppendEmptyTag(this.tags[tag]);

        public void AppendEmptyTag(string tag)
        {
            this.AppendSpaces();
            this.sb.Append("<");
            this.sb.Append(tag);
            this.sb.AppendLine("/>");
        }

        public void Trim() => this.sb.Length -= 2;

        public int GetXmlLength() => this.sb.Length;

        public void SetXmlLength(int value) => this.sb.Length = value;

        public string IntegerToHex(long value, int desimals)
        {
            return this.IntegerToHex(value, desimals, false);
        }

        public string IntegerToHex(long value, int desimals, bool forceHex)
        {
            return this.showNumericsAsHex && this.OutputType == TranslatorOutputType.SimpleXml ? value.ToString("X" + desimals.ToString()) : value.ToString();
        }

        public string IntegerToHex(ulong value, int desimals, bool forceHex)
        {
            return this.showNumericsAsHex && this.OutputType == TranslatorOutputType.SimpleXml ? value.ToString("X" + desimals.ToString()) : value.ToString();
        }

        public string IntegerToHex(ulong value)
        {
            return this.showNumericsAsHex && this.OutputType == TranslatorOutputType.SimpleXml ? value.ToString("X16") : value.ToString();
        }
    }
}
