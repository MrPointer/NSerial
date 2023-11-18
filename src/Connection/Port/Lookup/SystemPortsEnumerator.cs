using System.Collections.Generic;

namespace NSerial.Connection.Port.Lookup;

/// <summary>
/// Enumerates available serial ports using the System.IO.Ports.SerialPort class.
/// </summary>
public class SystemPortsEnumerator : ISerialPortsEnumerator
{
    /// <inheritdoc />
    public IEnumerable<string> GetAvailablePortNames()
    {
        return System.IO.Ports.SerialPort.GetPortNames();
    }
}