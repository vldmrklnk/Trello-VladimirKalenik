using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello
{
	public class CardManager
	{
		public List<Card> cards = new List<Card>();
		public delegate void Changes();
		public event Changes NewChanges;

		public void DeleteCard(Card card)
		{
			cards.Remove(card);
			Console.WriteLine($"Карточка {card.Title} удалена");
		}
		public Card CreateNewCard(string title, string data, User executer, Desk desk)
		{
			Card card = new Card(title, data, executer, desk);
			card.status = StatusOfCard.ToDo;
			cards.Add(card);
			File.WriteAllText("CardManager.json", JsonConvert.SerializeObject(cards));
			Console.WriteLine($"Карточка {card.Title} создана и прикреплена к доске");
			Logger.WriteActionAsync($"Создана карточка {card.Title}. Время: \n");
			return card;
		}
		public void ChangeStatus(Card card, StatusOfCard status)
		{
			card.status = status;
			Console.WriteLine("Статус изменён");
			NewChanges();
		}
		public void OverdueDeadLine()
		{
			var deadCards = cards.Where(t => t.ContainerDesk.DeadLine < t.dateTime);
			Console.WriteLine("|     Title     |     Executer     |     Data     |     Status     |     Desk     |");
			foreach (var c in deadCards)
			{
				Console.WriteLine($"|     {c.Title}     |     {c.Executer.Name}     |     {c.Data}     |     {c.status}     |" +
						$"     {c.ContainerDesk.Name}     |");
			}
		}
		public Card ChooseCard(string name, CardManager cardManager)
		{
			int i;
			for (i = 0; i < cardManager.cards.Count; i++)
				if (cardManager.cards[i].Title == name)
				{
					break;
				}
			return cardManager.cards[i];
		}
	}
	public enum StatusOfCard
	{
		ToDo,
		OnTeacher,
		OnStudent,
		Done
	}
}
