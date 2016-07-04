namespace iAnywhere.Data.SQLAnywhere
{
    internal class ConnectionOptions
    {
        public string LongName;
        public string ShortName;
        public string AlternateName;
        public object DefaultValue;
        public ConnectionOptionType Type;

        public ConnectionOptions(string lname, string sname, string altName, ConnectionOptionType type, object defaultValue)
        {
            LongName = lname;
            ShortName = sname;
            AlternateName = altName;
            Type = type;
            DefaultValue = defaultValue;
        }
    }
}
