using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Utils
{
	public class ParseDate
	{
	    static private string _date;
        static private string _time;
        static private string _year;
        static private string _month;
        static private string _day;
        static private string _hour;
        static private string _minute;
        static private string _second;
        static private string _millisecond;

	    private static Errors _error;

	    enum Errors
	    {
	        EverythingIsOk = 0,
            IncorrectDateFormat = 1,
            IncorrectTimeFormat = 2,
            IncorrectMonthValue = 3,
            IncorrectDayValue = 4,
            IncorrectHourValue = 5,
            IncorrectMinuteValue = 6,
            IncorrectSecondValue = 7,
            IncorrectMillisecondValue = 8,
            IncorrectYearValue = 9
	    }

	    enum Types
	    {
	        Millisecond = 0,
            Second = 1,
            Minute = 2,
            Hour = 3,
            Day = 4,
            Month = 5,
            Year = 6

	    }

		static public DateTime? Parse(string date, out string message)
		{
		    _error = Errors.EverythingIsOk;
			if ((date == "") || (date == " "))
			{
			    message = LookUpForMessage();
			    return null;
			}
		    char[] separator1 = {' '};
            char[] separator2 = { '.', '/', '-' };
		    char[] separator3 = {':'};
		    char[] separator4 = {'.'};
            string[] dateTime = date.Split(separator1);
		    if (dateTime.Length == 1)
		    {
		        _date = dateTime[0];
		        _time = "00:00:00.000";
		    }
			else
			{
			    _date = dateTime[0];
			    _time = dateTime[1];
			}
		    string[] dateParts = _date.Split(separator2);
            if (dateParts.Length < 3)
            {
                _error = Errors.IncorrectDateFormat;
                message = LookUpForMessage();
                return null;
            }
            string[] timeParts = _time.Split(separator3);
            if (timeParts.Length < 2)
            {
                _error = Errors.IncorrectTimeFormat;
                message = LookUpForMessage();
                return null;
            }
		    if (timeParts.Length < 3)
		    {
		        _second = "00";
		        _millisecond = "000";
		    }
		    else
		    {
		        string[] secondAndMs = timeParts[2].Split(separator4);
		        Validate(secondAndMs[0],Types.Second);
		        if (secondAndMs.Length == 1) _millisecond = "000";
		        else Validate(secondAndMs[1],Types.Millisecond);
		    }
		    Validate(timeParts[0],Types.Hour);
		    Validate(timeParts[1],Types.Minute);

		    Validate(dateParts[0], Types.Day);
		    Validate(dateParts[1], Types.Month);
		    Validate(dateParts[2], Types.Year);

		    if (_error != Errors.EverythingIsOk)
		    {
		        message = LookUpForMessage();
		        return null;
		    }

		    _date = _day + "/" + _month + "/" + _year;
		    _time = _hour + ":" + _minute + ":" + _second + "." + _millisecond;

            var forParser =  _date + " " + _time;
		    message = LookUpForMessage();
            return DateTime.Parse(forParser);
		}

        internal static bool TryParse(string date)
        {
            DateTime lumps;
            return DateTime.TryParse(date, out lumps);
        }

	    static private string LookUpForMessage()
	    {
	        var code = (int) _error;
	        switch (code)
	        {
	            case 0:
	                return "Всё замечательно!";
                case 1:
	                return "Некорректный ввод даты";
                case 2:
                    return "Некорректный ввод времени";
                case 3:
                    return "Некорректный ввод месяцев";
                case 4:
                    return "Некорректный ввод дней";
                case 5:
	                return "Некорректный ввод часов";
                case 6:
	                return "Некорректный ввод минут";
                case 7:
                    return "Некорректный ввод секунд";
                case 8:
                    return "Некорректный ввод миллисекунд";
                case 9:
	                return "Некорректный ввод года";
                default:
	                return "";
	        }
	    }

	    static private void Validate(string input, Types type)
	    {
	        var value = GetValue(input);
	        switch (type)
	        {
	                case Types.Hour:
	                if ((value > 23) || (value < 0))
	                {
                        _error = Errors.IncorrectHourValue;  
	                }
	                else
	                {
                        _hour = value.ToString();
                    }
	                return;
                    case Types.Minute:
                    if ((value > 59) || (value < 0))
                    {
                        _error = Errors.IncorrectMinuteValue;
                    }
                    else
                    {
                        _minute = value.ToString();
                    }
	                return;
                    case Types.Second:
	                if ((value > 59) || (value < 0))
	                {
                        _error = Errors.IncorrectSecondValue;
                    }
	                else
	                {
                        _second = value.ToString();
                    }
	                return;
                    case Types.Millisecond:
	                if ((value > 999) || (value < 0))
	                {
	                    _error = Errors.IncorrectMillisecondValue;
                    }
	                else
	                {
                        _millisecond = input;
                    }
	                return;
                    case Types.Year:
	                if (value > 2000)
	                {
	                    _year = value.ToString();
	                }
	                else if ((value > 100) || (value < 0))
	                {
                        _error = Errors.IncorrectYearValue;  
	                }
	                else
	                {
                        _year = 20 + value.ToString();
                    }
	                return;
                    case Types.Month:
	                if ((value > 12) || (value < 0))
	                {
	                    _error = Errors.IncorrectMonthValue;
	                }
	                else
	                {
	                    _month = value.ToString();
	                }
	                return;
                    case Types.Day:
	                if ((value > 31) || (value < 0))
	                {
	                    _error = Errors.IncorrectDayValue;
	                }
	                else
	                {
	                    _day = value.ToString();
	                }
	                return;

	        }
	    }

	    static private bool IsNumber(char ch)
	    {
	        if ((ch <= '9') && (ch >= '0')) return true;
	        return false;
	    }

	    static private int GetValue(string str)
	    {
            char[] charArray = str.ToCharArray();
            var multiplier = 1;
            var value = 0;
            for (int i = charArray.Length - 1; i >= 0; i--)
            {
                if (IsNumber(charArray[i]))
                {
                    value = multiplier * ((int) charArray[i] - 48) + value;
                    multiplier = multiplier * 10;
                }
            }
	        return value;
	    }


    }
}