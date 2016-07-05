namespace iAnywhere.Data.SQLAnywhere
{
    internal struct SADecimal
    {
        public uint Lo;
        public uint Mid;
        public uint Hi;
        public byte Scale;
        public byte Sign;

        public SADecimal(uint lo, uint mid, uint hi, byte scale, byte sign)
        {
            Lo = lo;
            Mid = mid;
            Hi = hi;
            Scale = scale;
            Sign = sign;
        }
    }
}
