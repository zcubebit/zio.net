using System.Net;
using zio.net;
using Xunit.Abstractions;
using Xunit.Extensions.Logging;

namespace UnitTest;

public class UdpTerminalPortTest(ITestOutputHelper output)
{
	[Fact]
	public void TestBoot()
	{
		MyLogger.Config();
		MyLogger.AddProvider(new XunitLoggerProvider(output, (msg, level) => true));

		var dport = new DebugPort();
		var port = new UdpTerminalPort(new IPEndPoint(IPAddress.Any, 2411));
		Link.Of(dport, port);
		
		port.BootAsync();
		
		Thread.Sleep(3000);
		output.WriteLine("Test end");
	}

}