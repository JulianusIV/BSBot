using System.Linq;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;

using Emzi0767;

using BSBot.Objects;

namespace BSBot
{
	public class DateArgumentConverter : IArgumentConverter<Date>
	{
		public Task<DSharpPlus.Entities.Optional<Date>> ConvertAsync(string value, CommandContext ctx)
		{
			if (value.Substring(0, 4).All(x => x.IsBasicDigit()))
			{
				Date date = new Date
				{
					Year = int.Parse(value.Substring(0, 4)),
					Month = int.Parse(value.Substring(5, 2)),
					Day = int.Parse(value.Substring(8, 2))
				};

				return Task.FromResult(DSharpPlus.Entities.Optional.FromValue(date));
			}
			else
			{
				Date date = new Date
				{
					Day = int.Parse(value.Substring(0, 2)),
					Month = int.Parse(value.Substring(3, 2)),
					Year = int.Parse(value.Substring(6, 4))
				};

				return Task.FromResult(DSharpPlus.Entities.Optional.FromValue(date));
			}
		}
	}
}
