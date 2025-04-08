using System.Net.Sockets;


Console.WriteLine("Enter Server IP");
string serverIP = Console.ReadLine() ?? string.Empty;

try
{
	TcpClient client = new TcpClient();
	await client.ConnectAsync(serverIP, 5000);
	Console.WriteLine("Connected to server.");

	NetworkStream stream = client.GetStream();
	StreamReader reader = new StreamReader(stream);
	StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

	_ = Task.Run(async () =>
	{
		Console.WriteLine("Received: ");
		string? incoming;
		while ((incoming = await reader.ReadLineAsync()) != null)
		{
			Console.WriteLine(incoming);
		}

	});

	Console.WriteLine("Sent: ");
	string? message;
	while ((message = Console.ReadLine()) != null)
	{
		await writer.WriteLineAsync(message);
	}

	//while (true)
	//{
	//	Console.Write("You: ");
	//	string message=Console.ReadLine()??string.Empty;

	//	if(string.IsNullOrEmpty(message))
	//		break;

	//	writer.WriteLine(message);
	//	string response=reader.ReadLine()??string.Empty;
	//	Console.WriteLine($"Server: {response}");
	//}
}
catch (Exception ex)
{
	Console.WriteLine($"Error: {ex}");
}