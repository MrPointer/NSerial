namespace SerialNET.Model
{
    /// <summary>
    /// An enum listing all of the ping on a serial DE9 port that are used for signaling.
    /// </summary>
    public enum ControlPin
    {
        /// <summary>
        /// Serial DE9's DTR pin represented as its actual pin number - 4.
        /// </summary>
        DTR = 4,

        /// <summary>
        /// Serial DE9's RTS pin represented as its actual pin number - 7.
        /// </summary>
        RTS = 7
    }
}