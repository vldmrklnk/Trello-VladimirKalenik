using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Trello
{
	public class DeskManager
	{

		public List<Desk> desks = new List<Desk>();
public void AddNewDesk(Desk desk)
		{
			desks.Add(desk);
			File.WriteAllText("DeskManager.json", JsonConvert.SerializeObject(desks));
		}
		public void DeleteDesk(Desk desk)
		{
			desks.Remove(desk);
		}
		public void ShowAllCardsOfTheDesk(Desk desk, CardManager cardManager)
		{
			Console.WriteLine("|     Title     |     Executer     |     Data     |     Status     |     Desk     |");
			foreach (var c in cardManager.cards.Where(t => t.ContainerDesk == desk))
			{
				Console.WriteLine($"|     {c.Title}     |     {c.Executer.Name}     |     {c.Data}     |     {c.status}     |" +
					$"     {c.ContainerDesk.Name}     |");

			}
		}
		public Desk ChooseDesk(string name, DeskManager deskManager)
		{
			int i;
			for (i = 0; i < deskManager.desks.Count; i++)
			{
				if (deskManager.desks[i].Name == name)
				{
					break;
				}
			}
			return deskManager.desks[i];
		}
	}
}
