using System.Collections.Generic;

namespace NSerial.Connection.Port.Lookup
{
    /// <summary>
    /// Enumerates available serial ports.
    /// </summary>
    public interface ISerialPortsEnumerator
    {
        /// <summary>
        /// Gets the available serial port names.
        /// </summary>
        /// <returns>The available serial port names.</returns>
        IEnumerable<string> GetAvailablePortNames();
    }
}