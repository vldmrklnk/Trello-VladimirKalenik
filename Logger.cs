using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Trello
{
	public class Logger
	{
		public async void WriteActionAsync(string action)
		{
			action += DateTime.Now;
			await Task.Run(() => File.AppendAllText("logs.txt", action));
		}
	}
}
