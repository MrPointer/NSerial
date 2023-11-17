using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using NSerial.Model;

namespace NSerial.Connection
{
    public class ManagedConnection
    {
        public ManagedConnection(ConnectionInfo connectionInfo, ISerialConnection connection)
        {
            ConnectionInfo = connectionInfo;
            Connection = connection;
        }

        public ConnectionInfo ConnectionInfo { get; }

        public ISerialConnection Connection { get; set; }
    }

    public class ConnectionPool : IConnectionPool
    {
        public ConnectionPoolResult CreateConnection(ConnectionInfo connectionInfo)
        {
            if (m_connections.ContainsKey(connectionInfo.PortName))
            {
                return ConnectionPoolError.ConnectionAlreadyExists;
            }

            try
            {
                var connection = new SerialConnection(new SerialPort(connectionInfo.PortName, connectionInfo.BaudRate));
                m_connections.Add(connectionInfo.PortName, new ManagedConnection(connectionInfo, connection));
                return ConnectionPoolResult.FromISerialConnection(connection);
            }
            catch (IOException)
            {
                return ConnectionPoolError.FailedToCreateConnection;
            }
        }

        public ConnectionPoolResult GetConnection(string portName)
        {
            return !m_connections.ContainsKey(portName)
                ? ConnectionPoolError.ConnectionDoesNotExist
                : ConnectionPoolResult.FromISerialConnection(m_connections[portName].Connection);
        }

        public bool ContainsConnection(ConnectionInfo connectionInfo)
        {
            return m_connections.FirstOrDefault(pair => pair.Value.ConnectionInfo == connectionInfo).Key != default;
        }

        public bool ContainsConnection(string portName)
        {
            return m_connections.ContainsKey(portName);
        }

        public void RemoveConnection(ISerialConnection connection)
        {
            var matchingConnectionPair = m_connections.FirstOrDefault(pair => pair.Value.Connection == connection);
            if (matchingConnectionPair.Key != default)
            {
                m_connections.Remove(matchingConnectionPair.Key);
            }
        }

        public void RemoveConnection(string portName)
        {
            if (m_connections.ContainsKey(portName))
            {
                m_connections.Remove(portName);
            }
        }

        private Dictionary<string, ManagedConnection> m_connections =
            new Dictionary<string, ManagedConnection>();

        public static IConnectionPool LocalConnectionPool => srm_LocalConnectionPool.Value;

        private static readonly Lazy<ConnectionPool> srm_LocalConnectionPool =
            new Lazy<ConnectionPool>(() => new ConnectionPool());
    }
}