using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;

namespace zio.net;

public interface IInternetPort
{
	void CommAsync(IPEndPoint peer);
}

public class TcpTerminalPort(IPEndPoint endPoint) : TerminalPort
{
	private readonly ILogger _logger = MyLogger.GetLogger("Program");
	private TcpClient? _client;
	private bool _closed = false;

	public override async Task BootAsync()
	{
		_client = new TcpClient();
		var cts = new CancellationTokenSource();
		cts.CancelAfter(TimeSpan.FromSeconds(20));
		await _client.ConnectAsync(endPoint, cts.Token);
		
		CommAsync(_client!);
	}

	public override void Shutdown()
	{
		_closed = true;
	}

	private async void CommAsync(TcpClient client)
	{
		try
		{
			//_client!.GetStream().Write(Encoding.ASCII.GetBytes("Hello comRelay:"));
			while (!_closed)
			{
				byte[] buffer = new byte[1024];
				var nRead = await client.GetStream().ReadAsync(buffer, 0, buffer.Length, default);
				if (nRead == 0)
				{
					_logger.Log(LogLevel.Debug, "Disconnected");
					break;
				}
				
				_logger.Log(LogLevel.Debug, "ReadAsync nRead={}", nRead);
			
				var seq = new ReadOnlySequence<byte>(buffer, 0, nRead);
				Sink(seq);
			}
		}
		catch (Exception e)
		{
			_logger.LogError(e, e.Message);
		}
	}

	public override Action<ReadOnlySequence<byte>> AsSink()
	{
		return buf =>
		{
			if (_client == null) throw new SocketException((int)SocketError.NotConnected);
			_client!.GetStream().Write(buf.FirstSpan);
		};
	}
}

public class UdpTerminalPort(IPEndPoint endPoint) : TerminalPort
{
    private readonly ILogger _logger = MyLogger.GetLogger("Program");
    private UdpClient? _client;

    public override void Shutdown()
    {
        throw new NotImplementedException();
    }

    public override Action<ReadOnlySequence<byte>> AsSink()
    {
        return buf =>
        {
            while (buf.IsEmpty == false)
            {
                _client!.Send(buf.FirstSpan);
            }
        };
    }

    
    public override async Task BootAsync()
    {
        _client = new UdpClient(endPoint);
        // var dgram = Encoding.UTF8.GetBytes("Hello nc");
        // _client!.Send(dgram, dgram.Length, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2424));
		CommAsync();
    }

    private async void CommAsync()
    {
        try
        {
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, endPoint.Port);
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(20));

            //_client!.GetStream().Write(Encoding.ASCII.GetBytes("Hello comRelay:"));
            while (true)
            {
                var bytes = await _client!.ReceiveAsync(cts.Token);
                _logger.Log(LogLevel.Debug, "ReadAsync nRead={}", bytes.Buffer.Length);
			
                var seq = new ReadOnlySequence<byte>(bytes.Buffer, 0, bytes.Buffer.Length);
                Sink(seq);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }

    public void boot(int listenPort)
    {
        UdpClient listener = new UdpClient(listenPort);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

        try
        {
            while (true)
            {
                Console.WriteLine("Waiting for broadcast");
                byte[] bytes = listener.Receive(ref groupEP);

                Console.WriteLine($"Received broadcast from {groupEP} :");
                Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            listener.Close();
        }
    }

    public void shutdown()
    {
        UdpClient listener = new UdpClient();
    }
}