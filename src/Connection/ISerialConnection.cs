using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSerial.Control;
using NSerial.Model;

namespace NSerial.Connection
{
    /// <summary>
    /// Represents a serial connection that allows for sending and receiving data, and controlling the DTR and RTS pins.
    /// </summary>
    public interface ISerialConnection
    {
        /// <summary>
        /// Opens the connection, preferably asynchronously.
        /// </summary>
        /// <returns>The task associated with opening the connection.</returns>
        Task Open();

        /// <summary>
        /// Closes the connection, preferably asynchronously.
        /// </summary>
        /// <returns>The task associated with closing the connection.</returns>
        Task Close();

        /// <summary>
        /// Sends data over the connection, preferably asynchronously.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <returns>The task associated with sending the data.</returns>
        Task SendData(IEnumerable<byte> data);

        /// <summary>
        /// Registers a data received handler. Each handler will be called in a separate <see cref="Task"/> when data
        /// is received.
        /// </summary>
        /// <param name="handler">The handler to register.</param>
        void RegisterDataReceivedHandler(EventHandler<DataReceivedEventArgs> handler);

        /// <summary>
        /// Unregisters a data received handler.
        /// </summary>
        /// <param name="handler">The handler to unregister.</param>
        void UnregisterDataReceivedHandler(EventHandler<DataReceivedEventArgs> handler);

        /// <summary>
        /// Gets the connection info.
        /// </summary>
        public ConnectionInfo ConnectionInfo { get; }

        /// <summary>
        /// Gets the DTR pin manager.
        /// </summary>
        IControlPinManager DtrPinManager { get; }

        /// <summary>
        /// Gets the RTS pin manager.
        /// </summary>
        IControlPinManager RtsPinManager { get; }

        /// <summary>
        /// Gets the DTR pin signal sender.
        /// </summary>
        IPinSignalSender? DtrPinSignalSender { get; }

        /// <summary>
        /// Gets the RTS pin signal sender.
        /// </summary>
        IPinSignalSender? RtsPinSignalSender { get; }
    }
}