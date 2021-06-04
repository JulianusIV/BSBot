namespace BSBot
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Bot.Instance.RunAsync().GetAwaiter().GetResult();
		}
	}
}
