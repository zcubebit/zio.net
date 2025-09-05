using System.Buffers;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;

namespace zio.net;

public class TcpEndpoint(IPEndPoint ipEndPoint) : Endpoint
{
	private readonly ILogger logger = MyLogger.GetLogger("Program");
	private TcpClient? _client;

	public void BootAsync()
	{
		_client = new TcpClient();
		//_client.Connect(IPEndPoint.Parse("127.0.0.1:2323"));
		ConnectAsync(_client);
		//_client = client;
			
		CommAsync();
	}

	private void ConnectAsync(TcpClient client)
	{
		try
		{
			// var cts = new CancellationTokenSource();
			// cts.CancelAfter(TimeSpan.FromSeconds(20));
			//await client.ConnectAsync("127.0.0.1", 2323);
			client.Connect(ipEndPoint);
		}
		catch (Exception e)
		{
			logger.LogError(e, e.Message);
		}
	}

	private void CommAsync()
	{
		try
		{
			while (true)
			{
				byte[] buffer = new byte[1024];
				//var nRead = await _client!.GetStream().ReadAsync(buffer, 0, buffer.Length, default);
				var nRead = _client!.GetStream().Read(buffer, 0, buffer.Length);

				var seq = new ReadOnlySequence<byte>(buffer, 0, nRead);
				Sink(seq);
			}
		}
		catch (Exception e)
		{
			logger.LogError(e, e.Message);
		}
	}

	public override Action<ReadOnlySequence<byte>> AsSink()
	{
		return buf =>
		{
			while (buf.IsEmpty == false)
			{
				_client!.GetStream().Write(buf.FirstSpan);
			}
		};
	}
}