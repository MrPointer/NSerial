using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using NSerial.Control;
using NSerial.Model;

namespace NSerial.Connection
{
    public class SerialConnection : ISerialConnection
    {
        public SerialConnection(SerialPort underlyingConnection)
        {
            UnderlyingConnection = underlyingConnection;
            DtrPinManager = new RS232ControlPinManager(ControlPin.DTR, underlyingConnection);
            RtsPinManager = new RS232ControlPinManager(ControlPin.RTS, underlyingConnection);
            DtrPinSignalSender = DtrPinManager as RS232ControlPinManager;
            RtsPinSignalSender = RtsPinManager as RS232ControlPinManager;
            
            UnderlyingConnection.DataReceived += (sender, args) =>
            {
                byte[] receiveBuffer =
                    new byte[Math.Max(UnderlyingConnection.BytesToRead, UnderlyingConnection.ReadBufferSize)];
                UnderlyingConnection.Read(receiveBuffer, 0, UnderlyingConnection.BytesToRead);
                UnderlyingConnection.DiscardInBuffer();

                foreach (var dataReceivedHandler in m_DataReceivedHandlers)
                {
                    Task.Run(() =>
                        dataReceivedHandler.Value?.Invoke(this, new DataReceivedEventArgs(receiveBuffer)));
                }
            };
        }

        public Task Open()
        {
            UnderlyingConnection?.Open();
            return Task.CompletedTask;
        }

        public Task Close()
        {
            UnderlyingConnection?.DiscardInBuffer();
            UnderlyingConnection?.DiscardOutBuffer();
            UnderlyingConnection?.Close();
            return Task.CompletedTask;
        }

        public Task SendData(Memory<byte> data)
        {
            return Task.Run(() =>
            {
                UnderlyingConnection.DiscardOutBuffer();
                UnderlyingConnection.Write(data.ToArray(), 0, data.Length);
            });
        }

        public void RegisterDataReceivedHandler(EventHandler<DataReceivedEventArgs> handler)
        {
            if (handler == null)
            {
                return;
            }

            m_DataReceivedHandlers.Add(sm_dataReceivedHandlerId++, handler);
        }

        public void UnregisterDataReceivedHandler(EventHandler<DataReceivedEventArgs> handler)
        {
            if (handler == null)
            {
                return;
            }

            foreach (var dataReceivedHandler in m_DataReceivedHandlers.Where(dataReceivedHandler =>
                         dataReceivedHandler.Value == handler))
            {
                m_DataReceivedHandlers.Remove(dataReceivedHandler.Key);
                break;
            }
        }

        public IControlPinManager DtrPinManager { get; }
        public IControlPinManager RtsPinManager { get; }
        public IPinSignalSender DtrPinSignalSender { get; }
        public IPinSignalSender RtsPinSignalSender { get; }

        public SerialPort UnderlyingConnection { get; private set; }

        private Dictionary<int, EventHandler<DataReceivedEventArgs>> m_DataReceivedHandlers =
            new Dictionary<int, EventHandler<DataReceivedEventArgs>>();

        private static int sm_dataReceivedHandlerId;
    }
}