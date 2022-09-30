namespace ISRORCert.Model
{
    public enum ServerBodyState : int
    {
        Blind = 0,
        Startup = 1,
        Loading = 2,
        Certifying = 3,
        ServicePending = 4,
        ServiceRunning = 5,
        Overloading = 6,
        Exiting = 7,
        Exited = 8,
    }
}