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
	public class DebugPort : TerminalPort
	{
		private Stream? _stream;

		public override Task BootAsync()
		{
			throw new NotImplementedException();
		}

		public override void Shutdown()
		{
			throw new NotImplementedException();
		}

		public override Action<ReadOnlySequence<byte>> AsSink()
		{
			return buf =>
			{
				ILogger logger = MyLogger.GetLogger("Program");

				String msg = EncodingExtensions.GetString(Encoding.UTF8, buf);
				logger.LogInformation("LogPort {Description}.", msg);
			};
		}
	}
}
