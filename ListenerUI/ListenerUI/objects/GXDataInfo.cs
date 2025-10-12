
//#nullable disable
namespace Indali.Security.Enum
{
    public class GXDataInfo
    {
        public int Index { get; internal set; }

        public bool AppendAA { get; internal set; }

        public int Count { get; internal set; }

        public DataType Type { get; internal set; }

        public bool Complete { get; internal set; }

        public GXDLMSTranslatorStructure xml { get; internal set; }

        public virtual void Clear()
        {
            this.Index = 0;
            this.Count = 0;
            this.Type = DataType.None;
            this.Complete = true;
        }
    }
}
