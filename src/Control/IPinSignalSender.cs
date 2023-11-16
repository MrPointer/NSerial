using System;
using System.Threading.Tasks;

namespace SerialNET.Control
{
    /// <summary>
    /// Interface representing a signal sender on a specific pin.
    /// </summary>
    public interface IPinSignalSender
    {
        /// <summary>
        /// Sends a short signal on the managed pin.
        /// </summary>
        /// <returns></returns>
        Task SendSignal();

        /// <summary>
        /// Sends a signal for the given duration on the managed pin.
        /// </summary>
        /// <param name="duration">Duration to send signal for.</param>
        /// <returns></returns>
        Task SendSignalFor(TimeSpan duration);
    }
}