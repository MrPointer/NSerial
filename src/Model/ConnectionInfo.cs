using System.IO.Ports;

namespace NSerial.Model
{
    /// <summary>
    /// Represents the information needed to create a serial connection.
    /// </summary>
    public struct ConnectionInfo
    {
        /// <summary>
        /// Creates a new connection info by copying the values from the given connection info object. 
        /// </summary>
        /// <param name="other">The connection info to copy from.</param>
        public ConnectionInfo(ConnectionInfo other)
        {
            PortName = other.PortName;
            BaudRate = other.BaudRate;
            DataBits = other.DataBits;
            Parity = other.Parity;
            StopBits = other.StopBits;
            Handshake = other.Handshake;
        }

        /// <summary>
        /// Checks if this connection info is equal to another connection info.
        /// </summary>
        /// <param name="other">The other connection info to check.</param>
        /// <returns><c>true</c> if the connection infos are equal, <c>false</c> otherwise.</returns>
        public bool Equals(ConnectionInfo other)
        {
            return PortName == other.PortName && BaudRate == other.BaudRate && DataBits == other.DataBits &&
                   Parity == other.Parity && StopBits == other.StopBits && Handshake == other.Handshake;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is ConnectionInfo other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = PortName != null ? PortName.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ BaudRate;
                hashCode = (hashCode * 397) ^ DataBits.GetHashCode();
                hashCode = (hashCode * 397) ^ Parity.GetHashCode();
                hashCode = (hashCode * 397) ^ StopBits.GetHashCode();
                hashCode = (hashCode * 397) ^ Handshake.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Checks if two connection infos are equal.
        /// </summary>
        /// <param name="left">The left connection info to check.</param>
        /// <param name="right">The right connection info to check.</param>
        /// <returns><c>true</c> if the connection infos are equal, <c>false</c> otherwise.</returns>
        public static bool operator ==(ConnectionInfo left, ConnectionInfo right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Checks if two connection infos are not equal.
        /// </summary>
        /// <param name="left">The left connection info to check.</param>
        /// <param name="right">The right connection info to check.</param>
        /// <returns><c>true</c> if the connection infos are not equal, <c>false</c> otherwise.</returns>
        public static bool operator !=(ConnectionInfo left, ConnectionInfo right)
        {
            return !(left == right);
        }

        /// <summary>
        /// The name of the port to connect to.
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        /// The baud rate (speed) of the connection.
        /// </summary>
        public int BaudRate { get; set; }

        /// <summary>
        /// The number of data bits in each byte, if any.
        /// </summary>
        public int? DataBits { get; set; }

        /// <summary>
        /// The number of stop bits in each byte, if any.
        /// </summary>
        public StopBits? StopBits { get; set; }

        /// <summary>
        /// The parity configuration of the connection, if any.
        /// </summary>
        public Parity? Parity { get; set; }

        /// <summary>
        /// The handshake configuration of the connection, if any.
        /// </summary>
        public Handshake? Handshake { get; set; }
    }
}