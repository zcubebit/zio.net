using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace zio.net
{
	public class DebugPort : IPort
	{
		private Stream? _stream;
		private Action<ReadOnlySequence<byte>>? _sinkTo;

		public Action<ReadOnlySequence<byte>> AsSink()
		{
			return buf =>
			{
				ILogger logger = MyLogger.GetLogger("Program");

				String msg = EncodingExtensions.GetString(Encoding.UTF8, buf);
				logger.LogInformation("LogPort {Description}.", msg);
			};
		}

		public void Link(IPort to)
		{
			_sinkTo = to.AsSink();
		}

		protected void Sink(ReadOnlySequence<byte> buf)
		{
			_sinkTo?.Invoke(buf);
		}

		public void TestPeer(String msg)
		{
			Sink(new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes(msg)));
		}
	}
}
