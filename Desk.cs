using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello
{
	
	public class Desk
	{
		public string Name { get; set; }
		public DateTime DeadLine { get; set; }

		public Desk(string name, DateTime date)
		{
			this.Name = name;
			this.DeadLine = date;
		}
	}
}
