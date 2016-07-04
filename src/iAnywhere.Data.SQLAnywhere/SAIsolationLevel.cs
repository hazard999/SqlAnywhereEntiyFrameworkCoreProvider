namespace iAnywhere.Data.SQLAnywhere
{
    public enum SAIsolationLevel
    {
        Unspecified = -1,
        Chaos = 16,
        ReadUncommitted = 256,
        ReadCommitted = 4096,
        RepeatableRead = 65536,
        Serializable = 1048576,
        Snapshot = 16777216,
        ReadOnlySnapshot = 16777217,
        StatementSnapshot = 16777218,
    }
}
