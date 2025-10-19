using System.Net;
using zio.net;
using Xunit.Abstractions;
using Xunit.Extensions.Logging;

namespace UnitTest;

public class TcpTerminalPortTest
{
	private readonly ITestOutputHelper _output;

	public TcpTerminalPortTest(ITestOutputHelper output)
	{
		this._output = output;
	}

	[Fact]
	public void TestBoot()
	{
		//using ILoggerFactory factory = LoggerFactory.Create(builder => {
		//	builder.Services.AddSingleton<ILoggerProvider>(new XunitLoggerProvider(output, (msg, level) => true));
		//});
		
		
		MyLogger.Config();
		MyLogger.AddProvider(new XunitLoggerProvider(_output, (msg, level) => true));

		var dport = new DebugPort();
		var tcpPort = new TcpTerminalPort(IPEndPoint.Parse("127.0.0.1:2323"));
		Link.Of(dport, tcpPort);
		
		tcpPort.BootAsync();
		
		Thread.Sleep(3000);
		_output.WriteLine("Test end");
		
		
		//
		//
		// TcpClient client = new TcpClient();
		// client.Connect(IPEndPoint.Parse("127.0.0.1:2323"));
		//
		// byte[] buffer = new byte[1024];
		// var nRead = client.GetStream().Read(buffer, 0, buffer.Length);
		//
		// output.WriteLine(Encoding.ASCII.GetString(buffer, 0, nRead));
		// client.Close();
	}

}