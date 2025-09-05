using System.Buffers;

namespace zio.net;

public abstract class Endpoint : IPort
{
    private Action<ReadOnlySequence<byte>>? _sinkTo;

    public abstract Action<ReadOnlySequence<byte>> AsSink();

    /// <summary>
    /// 자신에게 들어오는 데이터를 다른 곳을 전달할 때, 연결지점
    /// </summary>
    /// <param name="action"></param>
    public void SinkTo(Action<ReadOnlySequence<byte>> sinkTo)
    {
        _sinkTo = sinkTo;
    }

    protected void Sink(ReadOnlySequence<byte> buf)
    {
        _sinkTo?.Invoke(buf);
    }
}
