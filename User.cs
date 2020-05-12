using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello
{
	public class User
	{
		public string Name { get; set; }
		public User(string name)
		{
			this.Name = name;
		}
		public void MessageAboutChanges()
		{
			Console.WriteLine($"{this.Name} получил уведомление об измененнии статуса");
		}
	}
}
