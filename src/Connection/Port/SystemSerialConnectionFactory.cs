using System.IO;
using System.IO.Ports;
using NSerial.Model;
using Optional;

namespace NSerial.Connection.Port
{
    /// <summary>
    /// Creates a <see cref="ISerialConnection" /> from a <see cref="ConnectionInfo" />.
    /// </summary>
    public class SystemSerialConnectionFactory : ISerialConnectionFactory
    {
        /// <inheritdoc />
        public Option<ISerialConnection> CreateSerialConnection(ConnectionInfo connectionInfo)
        {
            try
            {
                var systemPort = new SerialPort(connectionInfo.PortName, connectionInfo.BaudRate);

                if (connectionInfo.Parity.HasValue)
                {
                    systemPort.Parity = connectionInfo.Parity.Value;
                }

                if (connectionInfo.DataBits.HasValue)
                {
                    systemPort.DataBits = connectionInfo.DataBits.Value;
                }

                if (connectionInfo.StopBits.HasValue)
                {
                    systemPort.StopBits = connectionInfo.StopBits.Value;
                }

                if (connectionInfo.Handshake.HasValue)
                {
                    systemPort.Handshake = connectionInfo.Handshake.Value;
                }

                return Option.Some<ISerialConnection>(new SerialConnection(systemPort, connectionInfo));
            }
            catch (IOException)
            {
                return Option.None<ISerialConnection>();
            }
        }
    }
}