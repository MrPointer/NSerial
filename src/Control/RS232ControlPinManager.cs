using System;
using System.IO.Ports;
using System.Threading.Tasks;
using NSerial.Constants;
using NSerial.Model;

namespace NSerial.Control
{
    /// <summary>
    /// Manages control pins of RS232 serial port.
    /// </summary>
    public class RS232ControlPinManager : IControlPinManager, IPinSignalSender
    {
        /// <summary>
        /// Creates a new instance of <see cref="RS232ControlPinManager"/>.
        /// </summary>
        /// <param name="pin">Pin to be controlled.</param>
        /// <param name="serialPort">Serial port to which the control pin is attached.</param>
        public RS232ControlPinManager(ControlPin pin, SerialPort serialPort)
        {
            Pin = pin;
            SerialPort = serialPort;
        }

        #region Implementation of IControlPinManager

        /// <inheritdoc cref="IControlPinManager.EnablePin"/>
        public Task EnablePin()
        {
            switch (Pin)
            {
                case ControlPin.DTR:
                    SerialPort.DtrEnable = true;
                    break;
                case ControlPin.RTS:
                    SerialPort.RtsEnable = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc cref="IControlPinManager.EnablePinFor" />
        public async Task EnablePinFor(TimeSpan duration)
        {
            switch (Pin)
            {
                case ControlPin.DTR:
                    SerialPort.DtrEnable = true;
                    await Task.Delay(ComputeHoldDuration(duration));
                    SerialPort.DtrEnable = false;
                    break;
                case ControlPin.RTS:
                    SerialPort.RtsEnable = true;
                    await Task.Delay(ComputeHoldDuration(duration));
                    SerialPort.RtsEnable = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <inheritdoc cref="IControlPinManager.DisablePin" />
        public Task DisablePin()
        {
            switch (Pin)
            {
                case ControlPin.DTR:
                    SerialPort.DtrEnable = false;
                    break;
                case ControlPin.RTS:
                    SerialPort.RtsEnable = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc cref="IControlPinManager.DisablePinFor" />
        public async Task DisablePinFor(TimeSpan duration)
        {
            switch (Pin)
            {
                case ControlPin.DTR:
                    SerialPort.DtrEnable = false;
                    await Task.Delay(ComputeHoldDuration(duration));
                    SerialPort.DtrEnable = true;
                    break;
                case ControlPin.RTS:
                    SerialPort.RtsEnable = false;
                    await Task.Delay(ComputeHoldDuration(duration));
                    SerialPort.RtsEnable = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <inheritdoc cref="IControlPinManager.TogglePin" />
        public Task TogglePin()
        {
            return TogglePinFor(TimeSpan.Zero);
        }

        /// <inheritdoc cref="IControlPinManager.TogglePinFor" />
        public async Task TogglePinFor(TimeSpan duration)
        {
            switch (Pin)
            {
                case ControlPin.DTR:
                    SerialPort.DtrEnable = !SerialPort.DtrEnable;
                    await Task.Delay(ComputeHoldDuration(duration));
                    SerialPort.DtrEnable = !SerialPort.DtrEnable;
                    break;
                case ControlPin.RTS:
                    SerialPort.RtsEnable = !SerialPort.RtsEnable;
                    await Task.Delay(ComputeHoldDuration(duration));
                    SerialPort.RtsEnable = !SerialPort.RtsEnable;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region Implementation of IPinSignalSender

        /// <inheritdoc cref="IPinSignalSender.SendSignal" />
        public Task SendSignal()
        {
            return SendSignalFor(TimeSpan.Zero);
        }

        /// <inheritdoc cref="IPinSignalSender.SendSignalFor" />
        public Task SendSignalFor(TimeSpan duration)
        {
            return TogglePinFor(duration);
        }

        #endregion

        private static TimeSpan ComputeHoldDuration(TimeSpan duration)
        {
            return duration < Timings.MinimumSignalSwitchTime
                ? Timings.MinimumSignalSwitchTime
                : duration;
        }

        /// <summary>
        /// Pin to be controlled.
        /// </summary>
        public ControlPin Pin { get; private set; }

        /// <summary>
        /// Serial port to which the control pin is attached.
        /// </summary>
        public SerialPort SerialPort { get; private set; }
    }
}