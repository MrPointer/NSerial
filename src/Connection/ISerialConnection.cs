using System;
using System.Threading.Tasks;
using NSerial.Control;

namespace NSerial.Connection
{
    public interface ISerialConnection
    {
        Task Open();

        Task Close();

        Task SendData(Memory<byte> data);

        void RegisterDataReceivedHandler(EventHandler<DataReceivedEventArgs> handler);

        void UnregisterDataReceivedHandler(EventHandler<DataReceivedEventArgs> handler);

        IControlPinManager DtrPinManager { get; }

        IControlPinManager RtsPinManager { get; }

        IPinSignalSender DtrPinSignalSender { get; }

        IPinSignalSender RtsPinSignalSender { get; }
    }
}