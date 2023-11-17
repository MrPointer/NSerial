using System;

namespace SerialNET.Connection
{
    /// <summary>
    /// An object representing the event args of an event that's raised when data has been received and read from the RS232 port.
    /// </summary>
    public class DataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="DataReceivedEventArgs"/>.
        /// </summary>
        /// <param name="buffer">Received buffer as an array of bytes.</param>
        public DataReceivedEventArgs(Memory<byte> buffer)
        {
            Buffer = buffer;
        }

        /// <summary>
        /// Received buffer as an array of bytes.
        /// </summary>
        public Memory<byte> Buffer { get; private set; }
    }
}