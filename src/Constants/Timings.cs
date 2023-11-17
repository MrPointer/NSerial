using System;

namespace NSerial.Constants
{
    /// <summary>
    /// Contains timing constants.
    /// </summary>
    public static class Timings
    {
        /// <summary>
        /// The minimum required time to hold a signal for it to be considered a signal.
        /// </summary>
        public static readonly TimeSpan MinimumSignalSwitchTime = TimeSpan.FromMilliseconds(200);
    }
}