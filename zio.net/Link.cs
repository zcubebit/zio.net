namespace zio.net;

public class Link
{
	public static void Of(IPort p1, IPort p2)
	{
		p1.Link(p2);
		p2.Link(p1);
	}
}