using System.IO.Ports;

namespace NSerial.Model
{
    public struct ConnectionInfo
    {
        public ConnectionInfo(ConnectionInfo other)
        {
            PortName = other.PortName;
            BaudRate = other.BaudRate;
            DataBits = other.DataBits;
            Parity = other.Parity;
            StopBits = other.StopBits;
            Handshake = other.Handshake;
        }

        public bool Equals(ConnectionInfo other)
        {
            return PortName == other.PortName && BaudRate == other.BaudRate && DataBits == other.DataBits &&
                   Parity == other.Parity && StopBits == other.StopBits && Handshake == other.Handshake;
        }

        public override bool Equals(object obj)
        {
            return obj is ConnectionInfo other && Equals(other);
        }

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

        public static bool operator ==(ConnectionInfo left, ConnectionInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ConnectionInfo left, ConnectionInfo right)
        {
            return !(left == right);
        }

        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public int? DataBits { get; set; }
        public Parity? Parity { get; set; }
        public StopBits? StopBits { get; set; }
        public Handshake? Handshake { get; set; }
    }
}