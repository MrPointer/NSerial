using NSerial.Model;
using OneOf;

namespace NSerial.Connection
{
    public enum ConnectionPoolError
    {
        ConnectionAlreadyExists,
        FailedToCreateConnection,
        ConnectionDoesNotExist
    }

    public class ConnectionPoolResult : OneOfBase<ISerialConnection, ConnectionPoolError>
    {
        public ConnectionPoolResult(OneOf<ISerialConnection, ConnectionPoolError> val) : base(val)
        {
        }

        public static ConnectionPoolResult FromISerialConnection(ISerialConnection _)
        {
            return new ConnectionPoolResult(OneOf<ISerialConnection, ConnectionPoolError>.FromT0(_));
        }


        public static implicit operator ConnectionPoolResult(ConnectionPoolError _)
        {
            return new ConnectionPoolResult(OneOf<ISerialConnection, ConnectionPoolError>.FromT1(_));
        }
    }

    public interface IConnectionPool
    {
        ConnectionPoolResult CreateConnection(ConnectionInfo connectionInfo);

        ConnectionPoolResult GetConnection(string portName);

        bool ContainsConnection(ConnectionInfo connectionInfo);

        bool ContainsConnection(string portName);

        void RemoveConnection(ISerialConnection connection);

        void RemoveConnection(string portName);
    }
}