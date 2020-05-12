using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello
{
	public class UserManager
	{
		public List<User> users = new List<User>();

		public User CreateNewUser(string name)
		{
			var user = new User(name);
			users.Add(user);
			File.WriteAllText("UserManager.json", JsonConvert.SerializeObject(users));
			Logger.WriteActionAsync($"Создан пользователь {user.Name}. Время: \n");
			return user;
		}
		public void DeleteUser(User user)
		{
			users.Remove(user);
			Console.WriteLine($"Пользователь {user.Name} удалён");
		}
		public User FindOrCreateUser(string name, UserManager userManager)
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
