using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace App.Data
{
	public class DataContext : DbContext
	{
		public DbSet<Entities.User> Users { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var config = JsonSerializer.Deserialize<JsonNode>(
				File.ReadAllText("appconfig.json"));
			int x = 1;
			//config.AsObject().Get
			//optionsBuilder.UseMySql()
		}
	}
}
