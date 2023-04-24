using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	internal class Program
	{
		static HubConnection HubConnection;
		static async Task Main(string[] args)
		{
			 HubConnection = new HubConnectionBuilder().WithUrl("http://localhost:49590/notification").Build();


			HubConnection.On<string>("RefreshProducts", message => Console.WriteLine($"Message from server : {message}"));

			await HubConnection.StartAsync();


			while (true) 
			{
				var message = Console.ReadLine();
				if (message != "exit")
					await HubConnection.SendAsync("RefreshProducts", message);
			}
		}
	}
}
