namespace ISRORCert.Model
{
    public enum ServerCordState : int
    {
        /// <summary>
        /// eSCS_Blind, Gray, Dashed
        /// </summary>
        Blind = 0,

        /// <summary>
        /// eSCS_EstablishingSession, White
        /// </summary>
        Establishing = 1,

        /// <summary>
        /// eSCS_Stable, Blue
        /// </summary>
        Stable = 2,

        /// <summary>
        /// eSCS_Overloading, Red, 2px
        /// </summary>
        Overloading = 3,

        /// <summary>
        /// eSCS_Sleeping, Olive
        /// </summary>
        Sleeping = 4,

        /// <summary>
        /// eSCS_Closing, Black
        /// </summary>
        Closing = 5,
    }
}