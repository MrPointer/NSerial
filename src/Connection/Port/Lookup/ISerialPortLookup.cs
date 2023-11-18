using NSerial.Model;

namespace NSerial.Connection.Port.Lookup
{
    /// <summary>
    /// Finds the serial port associated with a <see cref="ConnectionInfo" />
    /// and optionally a <see cref="IDeviceQuery" />.
    /// Creates a <see cref="ISerialConnection" /> in-place. 
    /// </summary>
    public interface ISerialPortLookup
    {
        /// <summary>
        /// Finds the serial port associated with a <see cref="ConnectionInfo" /> which successfully responds
        /// to the <see cref="IDeviceQuery" />.
        /// </summary>
        /// <param name="portConnectionInfo">Connection information for the serial port.</param>
        /// <param name="deviceQuery">Query to execute on the serial port to verify it is the correct device.</param>
        /// <returns>A <see cref="ISerialConnection" /> if a matching port is found, null otherwise.</returns>
        ISerialConnection? FindPort(ConnectionInfo portConnectionInfo, IDeviceQuery deviceQuery);

        /// <summary>
        /// An enumerator that returns the available serial ports.
        /// </summary>
        ISerialPortsEnumerator PortsEnumerator { get; }

        /// <summary>
        /// A factory that creates <see cref="ISerialConnection" />s from <see cref="ConnectionInfo" />s.
        /// </summary>
        ISerialConnectionFactory SerialConnectionFactory { get; }
    }
}