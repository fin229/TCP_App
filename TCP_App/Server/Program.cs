using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

TcpListener listener = new TcpListener(IPAddress.Any, 5000);
listener.Start();
Console.WriteLine($"Server started. Waiting for clients ...");

TcpState state = new TcpState();
Console.WriteLine(state.ToString());

TcpClient client = listener.AcceptTcpClient();
Console.WriteLine("Client connected!");

NetworkStream stream = client.GetStream();
StreamReader reader = new StreamReader(stream);
StreamWriter writer = new StreamWriter(stream) {AutoFlush=true };

try
{
	while (true)
	{
		string message=reader.ReadLine() ?? string.Empty;
		if(string.IsNullOrEmpty(message))
			break;

		Console.WriteLine($"Client: {message}");
		writer.WriteLine("Server received: "+message);
	}
}
catch (Exception ex)
{
	Console.WriteLine($"Error: {ex}");
}
finally
{
	client.Close();
	listener.Stop();
	Console.WriteLine("Server shutdown");
}
