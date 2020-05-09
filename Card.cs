using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello
{
	public class Card
	{
		public Desk ContainerDesk { get; set; }
		public string Title { get; set; }
		public string Data { get; set; }
		public User Executer { get; set; }
		public DateTime dateTime { get; set; }
		public StatusOfCard status { get; set; }
		public Card(string title, string data, User user, Desk desk)
		{
			this.Title = title;
			this.Data = data;
			this.Executer = user;
			this.dateTime = DateTime.Now;
			this.ContainerDesk = desk;
		}
		public void ChangeExecuter(User user)
		{
			this.Executer = user;
			Console.WriteLine($"Исполнитель изменён на {user.Name}");
		}
	}
}
