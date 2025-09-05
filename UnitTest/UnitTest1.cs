using zio.net;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using Xunit.Extensions.Logging;

namespace UnitTest;

public class UnitTest1
{
    private readonly ITestOutputHelper output;

    public UnitTest1(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Test1()
    {
        //using ILoggerFactory factory = LoggerFactory.Create(builder => {
        //	builder.Services.AddSingleton<ILoggerProvider>(new XunitLoggerProvider(output, (msg, level) => true));
        //});
        MyLogger.Config(null);
        MyLogger.AddProvider(new XunitLoggerProvider(output, (msg, level) => true));

        ILogger logger = MyLogger.GetLogger("Program");
        //
        // DebugPort2 p = new DebugPort2();
        // p.Link(new System.IO.Pipelines.Pipe());
        // output.WriteLine("Linked");
        //
        // for (int i = 0; i < 5; i++)
        // {
        // 	Thread.Sleep(1000 + 500);
        // 	p.Write(Encoding.ASCII.GetBytes("Hello comRelay:" + i));
        // 	output.WriteLine("Written");
        // }
        //
        // //p.reading();
        //
        //
        // Thread.Sleep(1000 * 20);
        // p.DeLink();
        //
        // Thread.Sleep(100);
        // logger.LogInformation("Test end");
    }
}