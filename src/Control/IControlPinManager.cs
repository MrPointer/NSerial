using System;
using System.Threading.Tasks;

namespace NSerial.Control;

/// <summary>
/// Interface representing a control-pin manager.
/// </summary>
public interface IControlPinManager
{
    /// <summary>
    /// Enable pin indefinitely.
    /// </summary>
    /// <returns></returns>
    Task EnablePin();

    /// <summary>
    /// Enable pin for the given amount of time, then disable it.
    /// </summary>
    /// <param name="duration">Duration to enable the pin for</param>
    /// <returns></returns>
    Task EnablePinFor(TimeSpan duration);

    /// <summary>
    /// Disable pin indefinitely
    /// </summary>
    /// <returns></returns>
    Task DisablePin();

    /// <summary>
    /// Disable the pin for the given amount of time, then enable it.
    /// </summary>
    /// <param name="duration">Duration to disable the pin for</param>
    /// <returns></returns>
    Task DisablePinFor(TimeSpan duration);

    /// <summary>
    /// Toggles the current pin state (enabled -> disabled and vice-versa).
    /// </summary>
    /// <returns></returns>
    Task TogglePin();

    /// <summary>
    /// Toggles the current pin state (enabled -> disabled and vice-versa) for the given amount of time.
    /// </summary>
    /// <param name="duration">Duration to toggle for</param>
    /// <returns></returns>
    Task TogglePinFor(TimeSpan duration);
}