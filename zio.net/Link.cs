namespace zio.net;

public class Link
{
	public static void Of(IPort p1, IPort p2)
	{
		p1.SinkTo(p2.AsSink());
		p2.SinkTo(p1.AsSink());
	}
}