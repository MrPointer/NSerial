using System.IO;
using System.IO.Ports;
using NSerial.Control;
using NSerial.Core;
using NSerial.Model;
using Optional;

namespace NSerial.Connection;

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
            var systemPort = new SerialPortWrapper(new SerialPort(connectionInfo.PortName, connectionInfo.BaudRate));

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

            var dtrPinManager = new SerialControlPinManager(ControlPin.DTR, systemPort, new TaskDelay());
            var rtsPinManager = new SerialControlPinManager(ControlPin.RTS, systemPort, new TaskDelay());

            return Option.Some<ISerialConnection>(new SerialConnection(systemPort, connectionInfo, dtrPinManager,
                rtsPinManager, dtrPinManager, rtsPinManager));
        }
        catch (IOException)
        {
            return Option.None<ISerialConnection>();
        }
    }
}