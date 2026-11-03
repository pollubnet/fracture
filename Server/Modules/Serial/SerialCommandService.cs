using System.IO.Ports;

namespace Fracture.Server.Modules.Serial;

/// <summary>
///     Sends commands through the serial port provided in the constructor.
///     The port is opened lazily on the first send and kept open until the service is disposed.
/// </summary>
public class SerialCommandService : ISerialCommandService, IDisposable
{
    private const int MaxSendAttempts = 2;
    private const int ReconnectDelayMs = 500;

    private readonly string _portName;
    private readonly int _baudRate;
    private readonly int _writeTimeoutMs;
    private readonly ILogger<SerialCommandService> _logger;
    private readonly object _syncRoot = new();

    private SerialPort? _port;
    private bool _disposed;

    public SerialCommandService(
        string portName,
        int baudRate,
        int writeTimeoutMs,
        ILogger<SerialCommandService> logger
    )
    {
        if (string.IsNullOrWhiteSpace(portName))
        {
            throw new ArgumentException("Serial port name must be provided.", nameof(portName));
        }

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(baudRate);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(writeTimeoutMs);

        _portName = portName;
        _baudRate = baudRate;
        _writeTimeoutMs = writeTimeoutMs;
        _logger = logger;
    }

    public void Send(string message)
    {
        ArgumentNullException.ThrowIfNull(message);

        lock (_syncRoot)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            Exception? lastException = null;

            for (var attempt = 1; attempt <= MaxSendAttempts; attempt++)
            {
                try
                {
                    GetOrOpenPort().Write(message);
                    _logger.LogDebug("Sent command to serial port {PortName}", _portName);
                    return;
                }
                catch (Exception exception)
                    when (exception is IOException or UnauthorizedAccessException)
                {
                    lastException = exception;
                    _logger.LogWarning(
                        exception,
                        "Failed to send command through serial port {PortName} (attempt {Attempt}/{MaxAttempts})",
                        _portName,
                        attempt,
                        MaxSendAttempts
                    );

                    ClosePort();

                    if (attempt < MaxSendAttempts)
                    {
                        Thread.Sleep(ReconnectDelayMs);
                    }
                }
            }

            throw new IOException(
                $"Failed to send command through serial port '{_portName}' after {MaxSendAttempts} attempts.",
                lastException
            );
        }
    }

    private SerialPort GetOrOpenPort()
    {
        if (_port is { IsOpen: true })
        {
            return _port;
        }

        ClosePort();

        _port = new SerialPort(_portName, _baudRate)
        {
            WriteTimeout = _writeTimeoutMs,
            Handshake = Handshake.None,
            DtrEnable = true,
            RtsEnable = false,
        };

        try
        {
            _port.Open();
        }
        catch
        {
            ClosePort();
            throw;
        }

        _logger.LogInformation(
            "Opened serial port {PortName} with baud rate {BaudRate}",
            _portName,
            _baudRate
        );

        return _port;
    }

    private void ClosePort()
    {
        if (_port is null)
        {
            return;
        }

        try
        {
            if (_port.IsOpen)
            {
                _port.Close();
            }
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            _logger.LogWarning(
                exception,
                "Failed to close serial port {PortName} cleanly",
                _portName
            );
        }
        finally
        {
            _port.Dispose();
            _port = null;
        }
    }

    public void Dispose()
    {
        lock (_syncRoot)
        {
            if (_disposed)
            {
                return;
            }

            ClosePort();
            _disposed = true;
        }
    }
}
