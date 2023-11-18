using System.Collections.Generic;
using NSerial.Model;
using OneOf;

namespace NSerial.Connection;

/// <summary>
/// Represents errors that can occur when using a connection pool.
/// </summary>
public enum ConnectionPoolError
{
    /// <summary>
    /// The connection already exists in the pool.
    /// </summary>
    ConnectionAlreadyExists,

    /// <summary>
    /// The connection does not exist in the pool.
    /// </summary>
    ConnectionDoesNotExist,

    /// <summary>
    /// Internal error caused the creation of the connection to fail.
    /// </summary>
    FailedToCreateConnection
}

/// <summary>
/// Represents the result of a connection pool operation.
/// </summary>
public class ConnectionPoolResult : OneOfBase<ISerialConnection, ConnectionPoolError>
{
    /// <summary>
    /// Creates a new connection pool result.
    /// </summary>
    /// <param name="_"></param>
    public ConnectionPoolResult(OneOf<ISerialConnection, ConnectionPoolError> _) : base(_)
    {
    }

    /// <summary>
    /// Creates a new connection pool result from an <see cref="ISerialConnection"/>.
    /// </summary>
    /// <param name="_"></param>
    /// <returns>A <see cref="ConnectionPoolResult"/> containing the <see cref="ISerialConnection"/>.</returns>
    public static ConnectionPoolResult FromISerialConnection(ISerialConnection _)
    {
        return new ConnectionPoolResult(OneOf<ISerialConnection, ConnectionPoolError>.FromT0(_));
    }

    /// <summary>
    /// Creates a new connection pool result from a <see cref="ConnectionPoolError"/>.
    /// </summary>
    /// <param name="_"></param>
    /// <returns>A <see cref="ConnectionPoolResult"/> containing the <see cref="ConnectionPoolError"/>.</returns>
    public static implicit operator ConnectionPoolResult(ConnectionPoolError _)
    {
        return new ConnectionPoolResult(OneOf<ISerialConnection, ConnectionPoolError>.FromT1(_));
    }
}

/// <summary>
/// Manages a pool of connections, where each connection is identified by a port name.
/// </summary>
public interface IConnectionPool
{
    /// <summary>
    /// Adds a new connection to the pool if it does not already exist.
    /// The connection is created from the given connection info.
    /// </summary>
    /// <param name="connectionInfo">The connection info to create the connection from.</param>
    /// <returns>A <see cref="ConnectionPoolResult"/> containing the <see cref="ISerialConnection"/> if created,
    /// <see cref="ConnectionPoolError"/> otherwise.</returns>
    ConnectionPoolResult AddConnection(ConnectionInfo connectionInfo);

    /// <summary>
    /// Adds a connection to the pool if it does not already exist.
    /// </summary>
    /// <param name="connection">The connection to add.</param>
    /// <returns>A <see cref="ConnectionPoolResult"/> containing the <see cref="ISerialConnection"/> if added,
    /// <see cref="ConnectionPoolError"/> otherwise.</returns>
    ConnectionPoolResult AddConnection(ISerialConnection connection);

    /// <summary>
    /// Attempts to gets a connection from the pool using the port name, if it exists.
    /// </summary>
    /// <param name="portName">The port name of the connection to get.</param>
    /// <returns>A <see cref="ConnectionPoolResult"/> containing the <see cref="ISerialConnection"/> if it exists,
    /// <see cref="ConnectionPoolError"/> otherwise.</returns>
    ConnectionPoolResult GetConnection(string portName);

    /// <summary>
    /// Attempts to gets a connection from the pool using the connection info, if it exists.
    /// </summary>
    /// <param name="connectionInfo">The connection info of the connection to get.</param>
    /// <returns>A <see cref="ConnectionPoolResult"/> containing the <see cref="ISerialConnection"/> if it exists,
    /// <see cref="ConnectionPoolError"/> otherwise.</returns>
    ConnectionPoolResult GetConnection(ConnectionInfo connectionInfo);

    /// <summary>
    /// Checks if the pool contains a connection with the given port name.
    /// </summary>
    /// <param name="portName">The port name of the connection to check.</param>
    /// <returns><c>true</c> if the pool contains a connection with the given port name, <c>false</c> otherwise.</returns>
    bool ContainsConnection(string portName);

    /// <summary>
    /// Checks if the pool contains a connection with the given connection info.
    /// </summary>
    /// <param name="connectionInfo">The connection info of the connection to check.</param>
    /// <returns><c>true</c> if the pool contains a connection with the given connection info, <c>false</c> otherwise.</returns>
    bool ContainsConnection(ConnectionInfo connectionInfo);

    /// <summary>
    /// Checks if the pool contains the given connection object.
    /// </summary>
    /// <param name="connection">The connection object to check.</param>
    /// <returns><c>true</c> if the pool contains the given connection object, <c>false</c> otherwise.</returns>
    bool ContainsConnection(ISerialConnection connection);

    /// <summary>
    /// Removes a connection from the pool using the port name. Does nothing if the connection does not exist.
    /// </summary>
    /// <param name="portName">The port name of the connection to remove.</param>
    void RemoveConnection(string portName);

    /// <summary>
    /// Removes a connection from the pool using the connection info. Does nothing if the connection does not exist.
    /// </summary>
    /// <param name="connectionInfo">The connection info of the connection to remove.</param>
    void RemoveConnection(ConnectionInfo connectionInfo);

    /// <summary>
    /// Removes a connection from the pool using the connection object. Does nothing if the connection does not exist.
    /// </summary>
    /// <param name="connection">The connection object to remove.</param>
    void RemoveConnection(ISerialConnection connection);

    /// <summary>
    /// Gets the connections in the pool.
    /// </summary>
    public IDictionary<string, ISerialConnection> Connections { get; }
}