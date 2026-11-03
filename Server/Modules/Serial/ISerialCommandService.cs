namespace Fracture.Server.Modules.Serial;

/// <summary>
///     Sends simple commands through a serial port.
/// </summary>
public interface ISerialCommandService
{
    /// <summary>
    ///     Sends the given message through the serial port, opening the port on first use.
    ///     The message is written as-is, so callers are responsible for any protocol-specific
    ///     line terminators.
    /// </summary>
    /// <param name="message">The message to send.</param>
    void Send(string message);
}
