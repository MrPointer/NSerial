using System.Threading.Tasks;

namespace NSerial.Connection.Port.Lookup
{
    /// <summary>
    /// Executes a query on a <see cref="ISerialConnection" /> to determine if the expected device is connected
    /// to the serial port.
    /// </summary>
    public interface IDeviceQuery
    {
        /// <summary>
        /// Executes the query on the <see cref="ISerialConnection" />.
        /// </summary>
        /// <param name="serialConnection">The <see cref="ISerialConnection" /> to execute the query on.</param>
        /// <returns>A <see cref="Task" /> containing the result of the query.
        /// Query returns <c>true</c> if the expected device is connected to the serial port, <c>false</c> otherwise.
        /// </returns>
        Task<bool> Execute(ISerialConnection serialConnection);
    }
}