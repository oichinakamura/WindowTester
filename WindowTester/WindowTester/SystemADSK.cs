namespace HIMTools
{
    using HIMTools.AppSystem;
    using HIMTools.Xml;
    using System.Xml.Linq;

    public class SystemADSK : XeDocument
    {
        protected string FilePath = "System.Config";

        public override IMetaElement CreateElement(XName name)
        {
            return this.CreateElement(name.LocalName);
        }
        public override IMetaElement CreateElement(XName name, bool IsRoot)
        {
            return this.CreateElement(name.LocalName);
        }
        public override IMetaElement CreateElement(string name)
        {
            switch (name)
            {
                case "SystemConfig": return new SystemConfigRoot();
                case "UsedFiles": return new UsedFileCollection();
                case "File": return new UsedFileElement();
            }
            return base.CreateElement(name);
        }

        public void Save()
        {
            Save(FilePath);
        }
    }
}
