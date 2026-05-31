using System.Net;
using zio.net;
using Xunit.Abstractions;
using Xunit.Extensions.Logging;

namespace UnitTest;

public class TcpTerminalPortTest(ITestOutputHelper output)
{
	[Fact]
	public void TestBoot()
	{
		//using ILoggerFactory factory = LoggerFactory.Create(builder => {
		//	builder.Services.AddSingleton<ILoggerProvider>(new XunitLoggerProvider(output, (msg, level) => true));
		//});
		
		
		MyLogger.Config();
		MyLogger.AddProvider(new XunitLoggerProvider(output, (msg, level) => true));

		var dport = new DebugPort();
		var tcpPort = new TcpTerminalPort(IPEndPoint.Parse("192.168.8.204:2323"));
		Link.Of(dport, tcpPort);
		
		tcpPort.BootAsync();
		
		Thread.Sleep(1000);
		dport.TestPeer("Hello peer");
		Thread.Sleep(1000);

		output.WriteLine("Test end");
	}

}