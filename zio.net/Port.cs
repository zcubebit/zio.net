using Microsoft.Extensions.Logging;
using System.Buffers;
using System.Collections;
using System.Data;
using System.IO.Pipelines;
using System.Linq.Expressions;
using System.Threading.Channels;
using System.Transactions;
using System.Xml;

namespace zio.net
{
	public interface IPort
	{
		public Action<ReadOnlySequence<byte>> AsSink();

		public void SinkTo(Action<ReadOnlySequence<byte>> to);
	}
}
