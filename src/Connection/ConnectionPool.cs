using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using NSerial.Model;

namespace NSerial.Connection;

/// <inheritdoc />
public class ConnectionPool : IConnectionPool
{
    /// <inheritdoc />
    public ConnectionPoolResult AddConnection(ConnectionInfo connectionInfo)
    {
        if (Connections.ContainsKey(connectionInfo.PortName))
        {
            return ConnectionPoolError.ConnectionAlreadyExists;
        }

        try
        {
            var connection = new SerialConnection(new SerialPort(connectionInfo.PortName, connectionInfo.BaudRate),
                connectionInfo);
            Connections.Add(connectionInfo.PortName, connection);
            return ConnectionPoolResult.FromISerialConnection(connection);
        }
        catch (IOException)
        {
            return ConnectionPoolError.FailedToCreateConnection;
        }
    }

    /// <inheritdoc />
    public ConnectionPoolResult AddConnection(ISerialConnection connection)
    {
        if (Connections.ContainsKey(connection.ConnectionInfo.PortName))
        {
            return ConnectionPoolError.ConnectionAlreadyExists;
        }

        Connections.Add(connection.ConnectionInfo.PortName, connection);
        return ConnectionPoolResult.FromISerialConnection(connection);
    }

    /// <inheritdoc />
    public ConnectionPoolResult GetConnection(string portName)
    {
        return !Connections.ContainsKey(portName)
            ? ConnectionPoolError.ConnectionDoesNotExist
            : ConnectionPoolResult.FromISerialConnection(Connections[portName]);
    }

    /// <inheritdoc />
    public ConnectionPoolResult GetConnection(ConnectionInfo connectionInfo)
    {
        var getResult = GetConnection(connectionInfo.PortName);
        return getResult.Match(
            connection => connection.ConnectionInfo == connectionInfo
                ? ConnectionPoolResult.FromISerialConnection(connection)
                : ConnectionPoolError.ConnectionDoesNotExist,
            error => error);
    }

    /// <inheritdoc />
    public bool ContainsConnection(string portName)
    {
        return Connections.ContainsKey(portName);
    }

    /// <inheritdoc />
    public bool ContainsConnection(ConnectionInfo connectionInfo)
    {
        return Connections.Values.Any(connection => connection.ConnectionInfo == connectionInfo);
    }

    /// <inheritdoc />
    public bool ContainsConnection(ISerialConnection connection)
    {
        return Connections.Values.Contains(connection);
    }

    /// <inheritdoc />
    public void RemoveConnection(string portName)
    {
        if (Connections.ContainsKey(portName))
        {
            Connections.Remove(portName);
        }
    }

    /// <inheritdoc />
    public void RemoveConnection(ConnectionInfo connectionInfo)
    {
        var getResult = GetConnection(connectionInfo);
        getResult.Switch(
            connection => RemoveConnection(connection.ConnectionInfo.PortName),
            _ => { });
    }

    /// <inheritdoc />
    public void RemoveConnection(ISerialConnection connection)
    {
        var getResult = GetConnection(connection.ConnectionInfo);
        getResult.Switch(
            _ => RemoveConnection(connection.ConnectionInfo.PortName),
            _ => { });
    }

    /// <inheritdoc />
    public IDictionary<string, ISerialConnection> Connections { get; } =
        new Dictionary<string, ISerialConnection>();

    /// <summary>
    /// System-wide connection pool, used as a singleton.
    /// This is the recommended way to access the connection pool.
    /// </summary>
    public static IConnectionPool LocalConnectionPool => srm_LocalConnectionPool.Value;

    private static readonly Lazy<ConnectionPool> srm_LocalConnectionPool = new(() => new ConnectionPool());
}