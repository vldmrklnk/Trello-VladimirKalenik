using System;
using System.Collections.Generic;
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
								User user = FindOrCreateUser(Console.ReadLine(), userManager);
								Console.WriteLine("Введите информацию: ");
								string data = Console.ReadLine();
								try
								{
									Console.WriteLine("Для какой доски добавить карточку");
									cardManager.CreateNewCard(title, data, user,
																ChooseDesk(Console.ReadLine(), deskManager));
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
									Card card = ChooseCard(Console.ReadLine(), cardManager);
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
									Card card = ChooseCard(Console.ReadLine(), cardManager);
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
									Card card = ChooseCard(Console.ReadLine(), cardManager);
									Console.WriteLine("Введите имя нового исполнителя: ");
									User user = FindOrCreateUser(Console.ReadLine(), userManager);
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
								User user = FindOrCreateUser(Console.ReadLine(), userManager);
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
			public class DeskManager
			{
				public List<Desk> desks = new List<Desk>();
				public void AddNewDesk(Desk desk)
				{
					desks.Add(desk);
				}
				public void DeleteDesk(Desk desk)
				{
					desks.Remove(desk);
				}
				public void ShowAllCardsOfTheDesk(Desk desk, CardManager cardManager)
				{
					Console.WriteLine("|     Title     |     Executer     |     Data     |     Status     |     Desk     |");
					foreach (var c in cardManager.cards)
					{
						if (c.ContainerDesk == desk)
							Console.WriteLine($"|     {c.Title}     |     {c.Executer.Name}     |     {c.Data}     |     {c.status}     |" +
								$"     {c.ContainerDesk.Name}     |");

					}


				}
			}
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
			public class CardManager
			{
				public List<Card> cards = new List<Card>();

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
					Console.WriteLine($"Карточка {card.Title} создана и прикреплена к доске");
					return card;
				}
				public void ChangeStatus(Card card, StatusOfCard status)
				{
					card.status = status;
					Console.WriteLine("Статус изменён");
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
			}
			public class User
			{
				public string Name { get; set; }
				public User(string name)
				{
					this.Name = name;
				}
			}
			public class UserManager
			{
				public List<User> users = new List<User>();
				public User CreateNewUser(string name)
				{
					var user = new User(name);
					users.Add(user);
					return user;
				}
				public void DeleteUser(User user)
				{
					users.Remove(user);
					Console.WriteLine($"Пользователь {user.Name} удалён");
				}
			}

			public enum StatusOfCard
			{
				ToDo,
				OnTeacher,
				OnStudent,
				Done
			}
			static void Main(string[] args)
			{
				Menu menu = new Menu();
			}
			public static Desk ChooseDesk(string name, DeskManager deskManager)
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
			public static Card ChooseCard(string name, CardManager cardManager)
			{
				int i;
				for (i = 0; i < cardManager.cards.Count; i++)
					if (cardManager.cards[i].Title == name)
					{
						break;
					}
				return cardManager.cards[i];
			}
			public static User FindOrCreateUser(string name, UserManager userManager)
			{
				int i;
				if (userManager.users.Count == 0)
				{
					userManager.CreateNewUser(name);
					i = 0;
				}
				else
				{
					for (i = 0; i < userManager.users.Count; i++)
						if (userManager.users[i].Name == name)
						{
							break;
						}
						else
						{
							userManager.CreateNewUser(name);
						}
				}
				return userManager.users[i];
			}
		}
	}
}







