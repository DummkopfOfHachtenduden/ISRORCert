namespace ISRORCert.Model
{
    [Flags]
    public enum StateNotifyFlag : byte
    {
        None = 0,
        ServerBody = 1,
        ServerCord = 2,
    }
}