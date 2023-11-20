using System;
using System.Threading.Tasks;

namespace NSerial.Core;

/// <summary>
/// Delays for a specified duration.
/// </summary>
public interface IDelay
{
    /// <summary>
    /// Delays for a specified duration.
    /// </summary>
    /// <param name="duration">Duration to delay.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Delay(TimeSpan duration);
}