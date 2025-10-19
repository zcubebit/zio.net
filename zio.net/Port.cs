using System.Buffers;

namespace zio.net
{
	public interface IPort
	{
		Action<ReadOnlySequence<byte>> AsSink();

		void Link(IPort to);
	}
	
	
	public abstract class TerminalPort : IPort
	{
		private Action<ReadOnlySequence<byte>>? _sinkTo;

		public abstract Task BootAsync();
		
		public abstract void Shutdown();
    
		public abstract Action<ReadOnlySequence<byte>> AsSink();

		/// <summary>
		/// 자신에게 들어오는 데이터를 다른 곳을 전달할 때, 연결지점
		/// </summary>
		/// <param name="sinkTo"></param>
		public void Link(IPort sinkTo)
		{
			_sinkTo = sinkTo.AsSink();
		}

		protected void Sink(ReadOnlySequence<byte> buf)
		{
			_sinkTo?.Invoke(buf);
		}
	}
}
