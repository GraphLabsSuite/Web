using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Utils
{
	public class ParseDate
	{
		static public DateTime? Parse(string date = "")
		{
			if (date == "")
			{
				return null;
			}
			else
			{
				return DateTime.Parse(date);
			}
		}
	}
}