using System;

namespace BSBot.Objects
{
	public class Date
	{
		public int Year { get; set; }
		public int Month { get; set; }
		public int Day { get; set; }

		public static Date Parse(string value)
		{
			return new Date
			{
				Year = int.Parse(value.Substring(0, 4)),
				Month = int.Parse(value.Substring(5, 2)),
				Day = int.Parse(value.Substring(8, 2))
			};
		}
		
		public static Date FromDateTime(DateTime dateTime)
		{
			return new Date
			{
				Year = dateTime.Year,
				Month = dateTime.Month,
				Day = dateTime.Day
			};
		}

		public static bool operator >(Date lhs, Date rhs)
		{
			if (lhs.Year > rhs.Year)
			{
				return true;
			}
			else if (lhs.Year < rhs.Year)
			{
				return false;
			}
			else
			{
				if (lhs.Month > rhs.Month)
				{
					return true;
				}
				else if (lhs.Month < rhs.Month)
				{
					return false;
				}
				else
				{
					if (lhs.Day > rhs.Day)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			}
		}

		public static bool operator >=(Date lhs, Date rhs)
		{
			if (lhs.Year > rhs.Year)
			{
				return true;
			}
			else if (lhs.Year < rhs.Year)
			{
				return false;
			}
			else
			{
				if (lhs.Month > rhs.Month)
				{
					return true;
				}
				else if (lhs.Month < rhs.Month)
				{
					return false;
				}
				else
				{
					if (lhs.Day > rhs.Day)
					{
						return true;
					}
					else if (lhs.Day < rhs.Day)
					{
						return false;
					}
					else
					{
						return true;
					}
				}
			}
		}

		public static bool operator <(Date lhs, Date rhs)
		{
			if (lhs.Year < rhs.Year)
			{
				return true;
			}
			else if (lhs.Year > rhs.Year)
			{
				return false;
			}
			else
			{
				if (lhs.Month < rhs.Month)
				{
					return true;
				}
				else if (lhs.Month > rhs.Month)
				{
					return false;
				}
				else
				{
					if (lhs.Day < rhs.Day)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			}
		}

		public static bool operator <=(Date lhs, Date rhs)
		{
			if (lhs.Year < rhs.Year)
			{
				return true;
			}
			else if (lhs.Year > rhs.Year)
			{
				return false;
			}
			else
			{
				if (lhs.Month < rhs.Month)
				{
					return true;
				}
				else if (lhs.Month > rhs.Month)
				{
					return false;
				}
				else
				{
					if (lhs.Day < rhs.Day)
					{
						return true;
					}
					else if (lhs.Day > rhs.Day)
					{
						return false;
					}
					else
					{
						return true;
					}
				}
			}
		}

		public override string ToString()
		{
			return Year.ToString("0000") + "." + Month.ToString("00") + "." + Day.ToString("00");
		}
	}
}
