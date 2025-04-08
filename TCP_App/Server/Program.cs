using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

TcpListener listener = new TcpListener(IPAddress.Any, 5000);
List<StreamWriter> clients = new List<StreamWriter>();

listener.Start();
Console.WriteLine($"Server started. Waiting for clients ...");

TcpState state = new TcpState();
Console.WriteLine(state.ToString());


while (true)
{
	TcpClient client = await listener.AcceptTcpClientAsync();
	Console.WriteLine("Client connected!");

	_=HandleClientAsync(client);
}

async Task HandleClientAsync(TcpClient client)
{
	NetworkStream stream = client.GetStream();
	StreamReader reader = new StreamReader(stream);
	StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

	lock (clients) clients.Add(writer);

	try
	{
		string message;
		while ((message=await reader.ReadLineAsync()??string.Empty) !=null)
		{

			if (string.IsNullOrEmpty(message))
				break;

			Console.WriteLine($"Client: {message}");
			await BroadcastAsync(message);
		}

	}
	catch (Exception ex)
	{
		Console.WriteLine($"Error: {ex.Message}");
	}
	finally
	{
		lock (clients) clients.Remove(writer);
		client.Close();
		Console.WriteLine("Client disconected");
	}

}

async Task BroadcastAsync(string message)
{
	lock (clients)
	{
		foreach (var client in clients)
		{
			try { client.WriteLine(message); } catch { }
		}
	}
	await Task.CompletedTask;
}