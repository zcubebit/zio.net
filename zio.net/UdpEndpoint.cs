using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace zio.net;

public class UdpEndpoint : Endpoint
{
    public override Action<ReadOnlySequence<byte>> AsSink()
    {
        throw new NotImplementedException();
    }

    public void boot(int listenPort)
    {
        UdpClient listener = new UdpClient(listenPort);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

        try
        {
            while (true)
            {
                Console.WriteLine("Waiting for broadcast");
                byte[] bytes = listener.Receive(ref groupEP);

                Console.WriteLine($"Received broadcast from {groupEP} :");
                Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            listener.Close();
        }
    }

    public void shutdown()
    {
        UdpClient listener = new UdpClient();
    }
}