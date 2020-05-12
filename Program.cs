using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello
{
	class Program
	{

		public class Menu
		{
			public Menu()
			{
				DeskManager deskManager = new DeskManager();
				UserManager userManager = new UserManager();
				CardManager cardManager = new CardManager();
				if (File.Exists("DeskManager.json"))
				{
					deskManager.desks = JsonConvert.DeserializeObject<List<Desk>>(File.ReadAllText("DeskManager.json"));
				}
				if (File.Exists("CardManager.json"))
				{
					cardManager.cards = JsonConvert.DeserializeObject<List<Card>>(File.ReadAllText("CardManager.json"));
				}
				if (File.Exists("UserManager.json"))
				{
					userManager.users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText("UserManager.json"));
				}

				while (true)
				{
					int selector;
					Console.WriteLine("------------------------------------------------------------\n" +
						"1. Создать доску\n" +
						"2. Создать карточку \n" +
						"3. Изменить карточку\n" +
						"4. Сменить статус карточки\n" +
						"5. Сменить исполнителя карточки\n" +
						"6. Показать все карточки исполнителя\n" +
						"7. Показать все карточки по статусу\n" +
						"8. Отчет для руководителя\n" +
						"------------------------------------------------------------\n");
					selector = Convert.ToInt32(Console.ReadLine());
					switch (selector)
					{
						case 1:
							{
								Console.WriteLine("Введите название доски: ");
								string ttl = Console.ReadLine();
								Console.WriteLine("Введите срок дедлайна(год, месяц, день): ");
								DateTime deadLine = Convert.ToDateTime(Console.ReadLine());
								Desk desk = new Desk(ttl, deadLine);
								deskManager.AddNewDesk(desk);
								break;
							}
						case 2:
							{
								Console.WriteLine("Введите название карточки: ");
								string title = Console.ReadLine();
								Console.WriteLine("Введите имя исполнителя: ");
								User user = userManager.FindOrCreateUser(Console.ReadLine(), userManager);
								Console.WriteLine("Введите информацию: ");
								string data = Console.ReadLine();
								try
								{
									Console.WriteLine("Для какой доски добавить карточку");
									cardManager.CreateNewCard(title, data, user,
																deskManager.ChooseDesk(Console.ReadLine(), deskManager));
								}
								catch
								{
									Console.WriteLine("Такой доски не существует, создайте новую или выберите другую");
								}
								break;
							}
						case 3:
							{
								try
								{
									Console.WriteLine("Введите название карточки, которую хотите изменить: ");
									Card card = cardManager.ChooseCard(Console.ReadLine(), cardManager);
									Console.WriteLine("Измените текст");
									card.Data = Console.ReadLine();

								}
								catch
								{
									Console.WriteLine("Такой карточки не существует, создайте новую или выберите другую");
								}
								break;
							}
						case 4:
							{
								try
								{
									Console.WriteLine("Введите название карточки статус которой, хотите изменить: ");
									Card card = cardManager.ChooseCard(Console.ReadLine(), cardManager);
									cardManager.NewChanges += card.Executer.MessageAboutChanges;
									Console.WriteLine("Выберите статус: \n" +
														"0-ToDO\n" +
														"1-OnTeacher\n" +
														"2-OnStudent\n" +
														"3-Done\n");
									cardManager.ChangeStatus(card, (StatusOfCard)Convert.ToInt32(Console.ReadLine()));
								}
								catch
								{
									Console.WriteLine("Такой карточки не существует, создайте новую или выберите другую");
								}
								break;
							}
						case 5:
							{
								try
								{
									Console.WriteLine("Введите название карточки исполнителя которой, хотите изменить: ");
									Card card = cardManager.ChooseCard(Console.ReadLine(), cardManager);
									Console.WriteLine("Введите имя нового исполнителя: ");
									User user = userManager.FindOrCreateUser(Console.ReadLine(), userManager);
									card.ChangeExecuter(user);
								}
								catch
								{
									Console.WriteLine("Такой карточки не существует, создайте новую или выберите другую");
								}
								break;
							}
						case 6:
							{
								Console.WriteLine("Введите имя исполнителя, чтобы вывести его карточки");
								User user = userManager.FindOrCreateUser(Console.ReadLine(), userManager);
								Console.WriteLine("|     Title     |     Executer     |     Data     |     Status     |     Desk     |");
								foreach (var c in cardManager.cards)
								{
									if (c.Executer == user)
										Console.WriteLine($"|     {c.Title}     |     {c.Executer.Name}     |     {c.Data}     |     {c.status}     |" +
											$"     {c.ContainerDesk.Name}     |");

								}
								break;
							}
						case 7:
							{
								Console.WriteLine("Выберите статус: \n" +
								"0-ToDO\n" +
								"1-OnTeacher\n" +
								"2-OnStudent\n" +
								"3-Done\n");
								StatusOfCard st = (StatusOfCard)Convert.ToInt32(Console.ReadLine());
								foreach (var c in cardManager.cards)
								{
									if (c.status == st)
										Console.WriteLine($"|     {c.Title}     |     {c.Executer.Name}     |     {c.Data}     |     {c.status}     |" +
											$"     {c.ContainerDesk.Name}     |");

								}
								break;
							}
						case 8:
							{
								Console.WriteLine("Просроченные карточки: ");
								cardManager.OverdueDeadLine();
								Console.WriteLine("Все карточки существующие");
								foreach (var d in deskManager.desks)
								{
									deskManager.ShowAllCardsOfTheDesk(d, cardManager);
								}

								break;

							}
					}
				}
			}
			static void Main(string[] args)
			{
				Menu menu = new Menu();
			}
		}
	}
}







