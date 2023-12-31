﻿using System;
using System.Threading.Tasks;
using NSerial.Model;

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

    /// <summary>
    /// Gets the pin that is being managed.
    /// </summary>
    public ControlPin Pin { get; }

    /// <summary>
    /// Gets the current state of the managed pin.
    /// </summary>
    public PinState State { get; }
}