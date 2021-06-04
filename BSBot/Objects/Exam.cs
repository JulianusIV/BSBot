using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSBot.Objects
{
	public class Exam : BaseObject
	{
		public DateTime DueDate { get; set; }
		public long MessageId { get; set; }
		public string Text { get; set; }
		public string Subject { get; set; }
	}
}
