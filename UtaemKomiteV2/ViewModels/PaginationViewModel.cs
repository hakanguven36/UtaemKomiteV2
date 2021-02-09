using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtaemKomiteV2.ViewModels
{
	public class PaginationViewModel
	{
		public int itemCount { get; set; }
		public int totalPage { get; set; }
		public int currentPage { get; set; }
		public bool isFirstPage { get; set; }
		public bool isLastPage { get; set; }
	}
}
