namespace iAnywhere.Data.SQLAnywhere
{
    /// <summary>Summary description for SATime.</summary>
    internal struct SATime
    {
        public ushort Hour;
        public ushort Minute;
        public ushort Second;
        public uint Microsecond;

        public SATime(ushort hour, ushort minute, ushort second, uint microsecond)
        {
            Hour = hour;
            Minute = minute;
            Second = second;
            Microsecond = microsecond;
        }
    }
}
