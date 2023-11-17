using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using NSerial.Model;

namespace NSerial.Connection
{
    public class ConnectionPool : IConnectionPool
    {
        public ConnectionPoolResult CreateConnection(ConnectionInfo connectionInfo)
        {
            if (m_connections.ContainsKey(connectionInfo))
            {
                return ConnectionPoolError.ConnectionAlreadyExists;
            }

            var connection = new SerialConnection(new SerialPort(connectionInfo.PortName, connectionInfo.BaudRate));
            m_connections.Add(connectionInfo, connection);
            return ConnectionPoolResult.FromISerialConnection(connection);
        }

        public ConnectionPoolResult GetConnection(ConnectionInfo connectionInfo)
        {
            return !m_connections.ContainsKey(connectionInfo)
                ? ConnectionPoolError.ConnectionDoesNotExist
                : ConnectionPoolResult.FromISerialConnection(m_connections[connectionInfo]);
        }

        public bool ContainsConnection(ConnectionInfo connectionInfo)
        {
            return m_connections.ContainsKey(connectionInfo);
        }

        public void RemoveConnection(ISerialConnection connection)
        {
            foreach (var connectionInfo in m_connections.Keys.Where(connectionInfo =>
                         m_connections[connectionInfo] == connection))
            {
                m_connections.Remove(connectionInfo);
                break;
            }
        }

        public void RemoveConnection(ConnectionInfo connectionInfo)
        {
            if (m_connections.ContainsKey(connectionInfo))
            {
                m_connections.Remove(connectionInfo);
            }
        }

        public static IConnectionPool LocalConnectionPool => srm_LocalConnectionPool.Value;

        private Dictionary<ConnectionInfo, ISerialConnection> m_connections =
            new Dictionary<ConnectionInfo, ISerialConnection>();

        private static readonly Lazy<ConnectionPool> srm_LocalConnectionPool =
            new Lazy<ConnectionPool>(() => new ConnectionPool());
    }
}