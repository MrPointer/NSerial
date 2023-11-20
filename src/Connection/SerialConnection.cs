using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using NSerial.Control;
using NSerial.Core;
using NSerial.Model;

namespace NSerial.Connection;

/// <summary>
/// Represents a serial connection wrapping a <see cref="SerialPort" />.
/// </summary>
public class SerialConnection : ISerialConnection
{
    /// <summary>
    ///    Creates a new instance of <see cref="SerialConnection" />.
    /// </summary>
    /// <param name="underlyingConnection">The underlying <see cref="SerialPort" /> to use.</param>
    /// <param name="connectionInfo">The connection info associated with this connection.</param>
    /// <exception cref="InvalidOperationException">PinSignalSender cannot be casted from their respective
    /// <see cref="IControlPinManager" />.</exception>
    public SerialConnection(ISerialPortWrapper underlyingConnection, ConnectionInfo connectionInfo
        , IControlPinManager dtrPinManager, IControlPinManager rtsPinManager,
        IPinSignalSender? dtrPinSignalSender = null,
        IPinSignalSender? rtsPinSignalSender = null)
    {
        UnderlyingConnection = underlyingConnection;
        ConnectionInfo = connectionInfo;

        DtrPinManager = dtrPinManager;
        RtsPinManager = rtsPinManager;
        DtrPinSignalSender = dtrPinSignalSender;
        RtsPinSignalSender = rtsPinSignalSender;

        UnderlyingConnection.DataReceived += (sender, args) =>
        {
            byte[] receiveBuffer =
                new byte[Math.Max(UnderlyingConnection.BytesToRead, UnderlyingConnection.ReadBufferSize)];
            UnderlyingConnection.Read(receiveBuffer, 0, UnderlyingConnection.BytesToRead);
            UnderlyingConnection.DiscardInBuffer();

            foreach (var dataReceivedHandler in m_dataReceivedHandlers)
            {
                Task.Run(() =>
                    dataReceivedHandler.Value?.Invoke(this, new DataReceivedEventArgs(receiveBuffer)));
            }
        };
    }

    /// <inheritdoc />
    public Task Open()
    {
        UnderlyingConnection.Open();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task Close()
    {
        UnderlyingConnection.DiscardInBuffer();
        UnderlyingConnection.DiscardOutBuffer();
        UnderlyingConnection.Close();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task SendData(IEnumerable<byte> data)
    {
        return Task.Run(() =>
        {
            var dataAsList = data.ToList();
            UnderlyingConnection.DiscardOutBuffer();
            UnderlyingConnection.Write(dataAsList.ToArray(), 0, dataAsList.Count);
        });
    }

    /// <inheritdoc />
    public void RegisterDataReceivedHandler(EventHandler<DataReceivedEventArgs> handler)
    {
        m_dataReceivedHandlers.Add(sm_dataReceivedHandlerId++, handler);
    }

    /// <inheritdoc />
    public void UnregisterDataReceivedHandler(EventHandler<DataReceivedEventArgs> handler)
    {
        foreach (var dataReceivedHandler in m_dataReceivedHandlers.Where(dataReceivedHandler =>
                     dataReceivedHandler.Value == handler))
        {
            m_dataReceivedHandlers.Remove(dataReceivedHandler.Key);
            break;
        }
    }

    /// <inheritdoc />
    public ConnectionInfo ConnectionInfo { get; }

    /// <inheritdoc />
    public IControlPinManager DtrPinManager { get; }

    /// <inheritdoc />
    public IControlPinManager RtsPinManager { get; }

    /// <inheritdoc />
    public IPinSignalSender? DtrPinSignalSender { get; }

    /// <inheritdoc />
    public IPinSignalSender? RtsPinSignalSender { get; }

    /// <summary>
    ///   The underlying <see cref="SerialPort" />.
    /// </summary>
    public ISerialPortWrapper UnderlyingConnection { get; }

    private readonly Dictionary<int, EventHandler<DataReceivedEventArgs>> m_dataReceivedHandlers = new();

    private static int sm_dataReceivedHandlerId;
}