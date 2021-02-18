using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UtaemKomiteV2.Models
{
	public class MyContext : DbContext
	{
		public MyContext(DbContextOptions<MyContext> options) : base(options)
		{
		}
		public DbSet<Kullar> Kullar { get; set; }
		public DbSet<Dosya> Dosya { get; set; }
		public DbSet<Tur> Tur { get; set; }
	}
}
