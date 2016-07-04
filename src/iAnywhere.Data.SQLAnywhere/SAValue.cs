namespace iAnywhere.Data.SQLAnywhere
{
    internal struct SAValue
    {
        public int Ordinal;
        public SADataItem Value;

        public SAValue(int ordinal, SADataItem value)
        {
            Ordinal = ordinal;
            Value = value;
        }
    }
}
