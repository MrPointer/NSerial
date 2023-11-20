namespace NSerial.Control;

/// <summary>
/// Represents the state of a pin.
/// </summary>
public enum PinState
{
    /// <summary>
    /// The pin is disabled.
    /// </summary>
    Disabled = 0,

    /// <summary>
    /// The pin is enabled.
    /// </summary>
    Enabled
}

/// <summary>
/// Extension methods for <see cref="PinState"/>.
/// </summary>
public static class PinStateExtensions
{
    /// <summary>
    /// Returns whether the pin is enabled.
    /// </summary>
    /// <param name="state">Pin state</param>
    /// <returns><c>true</c> if the pin is enabled, <c>false</c> otherwise.</returns>
    public static bool IsEnabled(this PinState state)
    {
        return state == PinState.Enabled;
    }

    /// <summary>
    /// Returns whether the pin is disabled.
    /// </summary>
    /// <param name="state">Pin state</param>
    /// <returns><c>true</c> if the pin is disabled, <c>false</c> otherwise.</returns>
    public static bool IsDisabled(this PinState state)
    {
        return state == PinState.Disabled;
    }

    /// <summary>
    /// Converts a <see cref="bool"/> to a <see cref="PinState"/>.
    /// </summary>
    /// <param name="value">Value to convert</param>
    /// <returns><see cref="PinState.Enabled"/> if <paramref name="value"/> is <c>true</c>,
    /// <see cref="PinState.Disabled"/> otherwise.</returns>
    public static PinState FromBool(bool value)
    {
        return value ? PinState.Enabled : PinState.Disabled;
    }

    /// <summary>
    /// Converts a <see cref="PinState"/> to a <see cref="bool"/>.
    /// </summary>
    /// <param name="state">State to convert</param>
    /// <returns><c>true</c> if <paramref name="state"/> is <see cref="PinState.Enabled"/>,
    /// <c>false</c> otherwise.</returns>
    public static bool ToBool(this PinState state)
    {
        return state == PinState.Enabled;
    }

    /// <summary>
    /// Toggles the given <see cref="PinState"/>.
    /// </summary>
    /// <param name="state">State to toggle</param>
    /// <returns><see cref="PinState.Enabled"/> if <paramref name="state"/> is <see cref="PinState.Disabled"/>,
    /// <see cref="PinState.Disabled"/> otherwise.</returns>
    public static PinState Toggle(this PinState state)
    {
        return state == PinState.Enabled ? PinState.Disabled : PinState.Enabled;
    }
}