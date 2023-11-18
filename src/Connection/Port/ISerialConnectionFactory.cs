using System.IO.Ports;
using NSerial.Model;
using Optional;

namespace NSerial.Connection.Port
{
    /// <summary>
    /// Creates a <see cref="ISerialConnection"/> from a <see cref="ConnectionInfo"/>.
    /// </summary>
    public interface ISerialConnectionFactory
    {
        /// <summary>
        /// Creates a <see cref="SerialPort" /> from a <see cref="ConnectionInfo" />.
        /// </summary>
        /// <param name="connectionInfo">The <see cref="ConnectionInfo" /> to create connection from.</param>
        /// <returns>A <see cref="ISerialConnection" /> if created successfully,
        /// <see cref="Option.None{T}" /> otherwise.</returns>
        Option<ISerialConnection> CreateSerialConnection(ConnectionInfo connectionInfo);
    }
}