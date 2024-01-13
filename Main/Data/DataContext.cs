using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Main.Data
{
	public class DataContext : DbContext
	{
		public DbSet<Entities.User> Users { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var config = JsonSerializer.Deserialize<JsonNode>(
				File.ReadAllText("appconfig.json"));

			optionsBuilder.UseMySql(
				config["databases"]["planetScale"]["connectionString"].ToString(),
				new MySqlServerVersion(new Version(8, 0, 34))
			);
		}
	}
}
/*  Add-Migration Name   -- створити міграцію
 *   у першій міграції видалити чи коментувати AlterDatabase (14-15 рядок)
 *  Update-Database -- внести дані міграції до БД
 *  
 *  Якщо треба розширювати БД, то
 *  - описуєте сутності (змінюєте наявні)
 *  - додаєте їх до DataContext як DbSet
 *  - створюєте міграцію (Add-Migration NewName)
 *  - вносите її до БД Update-Database
 *  
 *  створити користувача
 *  _dataContext.Users.Add( new User { Name=... } )
 *  _dataContext.SaveChanges()
 *  
 *  знайти користувача по id
 *  var user = _dataContext.Users.Find(100500)  // 100500 - id
 *  
 *  знайти користувача по імені
 *  var user = _dataContext.Users.FirstOrDefault( u => u.Name="Petrovich")
 *  внести зміни
 *  user.Email = "new@email"
 *  _dataContext.SaveChanges()
 */
