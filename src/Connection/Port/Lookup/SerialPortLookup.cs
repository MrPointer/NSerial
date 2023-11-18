using System;
using System.Linq;
using System.Threading.Tasks;
using NSerial.Model;
using Optional.Unsafe;

namespace NSerial.Connection.Port.Lookup;

/// <inheritdoc />
public class SerialPortLookup : ISerialPortLookup
{
    /// <summary>
    /// Creates a new <see cref="SerialPortLookup" />.
    /// </summary>
    /// <param name="portsEnumerator">The <see cref="ISerialPortsEnumerator" /> to use.</param>
    /// <param name="serialConnectionFactory">The <see cref="ISerialConnectionFactory" /> to use.</param>
    public SerialPortLookup(ISerialPortsEnumerator portsEnumerator,
        ISerialConnectionFactory serialConnectionFactory)
    {
        PortsEnumerator = portsEnumerator;
        SerialConnectionFactory = serialConnectionFactory;
    }

    /// <inheritdoc />
    public ISerialConnection? FindPort(ConnectionInfo portConnectionInfo, IDeviceQuery deviceQuery)
    {
        return (from portName in PortsEnumerator.GetAvailablePortNames()
            select new ConnectionInfo(portConnectionInfo) { PortName = portName }
            into currentConnectionInfo
            select SerialConnectionFactory.CreateSerialConnection(currentConnectionInfo)
            into createConnectionResult
            let matchingDevice = createConnectionResult.Map(async connection =>
            {
                try
                {
                    await connection.Open();

                    bool queryResult = await deviceQuery.Execute(connection);
                    if (queryResult)
                    {
                        return true;
                    }

                    await connection.Close();
                    return false;
                }
                catch (Exception)
                {
                    await connection.Close();
                    return false;
                }
            })
            where matchingDevice.ValueOr(Task.FromResult(false)).Result
            select createConnectionResult.ValueOrFailure("Failed to retrieve connection")).FirstOrDefault();
    }

    /// <inheritdoc />
    public ISerialPortsEnumerator PortsEnumerator { get; }

    /// <inheritdoc />
    public ISerialConnectionFactory SerialConnectionFactory { get; }
}