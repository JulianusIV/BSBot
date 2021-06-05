namespace BSBot.Objects
{
	public class Exam : BaseObject
	{
		public Date DueDate { get; set; }
		public ulong MessageId { get; set; }
		public string Text { get; set; }
		public string Subject { get; set; }
	}
}
