#include "stdafx.h"
#include <iostream>
#include <string>
#include "header.h"

bool IsLeapYear(int year) {
	if (year % 4 == 0) {
		if (year % 100 == 0) {
			if (year % 400 != 0) {
				return false;
			}
		}
		return true;
	}
	else {
		return false;
	}
}

int GetDaysCountInMonth(int month, int year) {
	if (month == 2) {
		if (IsLeapYear(year)) {
			return 29;
		}
		else {
			return 28;
		}
	}


	if (((month / 8) + month % 8) % 2 == 0) {
		return 30;
	}
	else {
		return 31;
	}
}

int FindNearestLeapYear(int year) {
	if (IsLeapYear(year)) { return year; }
	int res = year + (4 - year % 4);
	if (res % 100 == 0) {
		if (res % 400 != 0) {
			return res + 4;
		}
	}
	return res;
}



class fooldatetime {

	int year, month, day;
	int hour, minute, second;
	int timezone;

	long long int dateInSeconds;

public:
	fooldatetime() : year(0), month(0), day(0), hour(0), minute(0), second(0), timezone(0), dateInSeconds(0){ }
	fooldatetime(int yearval) : year(yearval), month(0), day(0), hour(0), minute(0), second(0), timezone(0), dateInSeconds(0) {}

	friend fooldatetime operator + (fooldatetime first, fooldatetime second);
	friend fooldatetime operator - (fooldatetime first, fooldatetime second);
	friend bool operator > (fooldatetime first, fooldatetime second);
	friend bool operator >= (fooldatetime first, fooldatetime second);
	friend bool operator < (fooldatetime first, fooldatetime second);
	friend bool operator <= (fooldatetime first, fooldatetime second);
	friend bool operator == (fooldatetime first, fooldatetime second);

	friend void StatisticsOfDayNumber(int day, fooldatetime startdate, fooldatetime enddate);


	void ConvertDateToSeconds() {
		int tmp = 0;
		dateInSeconds = 0;
		for (int i = 1970; i < year; i++) {
			if (IsLeapYear(i)) {
				tmp += 366;
			}
			else {
				tmp += 365;
			}
		}
		dateInSeconds += tmp * 24 * 3600;

		tmp = 0;
		for (int i = 1; i < month; i++) {
			tmp += GetDaysCountInMonth(i, year);
		}
		dateInSeconds += tmp * 24 * 3600;

		dateInSeconds += (day - 1) * 24 * 3600;

		dateInSeconds += second + minute * 60 + hour * 3600;
	}

	void ConvertSecondsToDate() {
		int tmp = dateInSeconds;
		year = 1970;
		int amount = (365 + IsLeapYear(year)) * 24 * 3600;
		while (tmp >= amount) {
			tmp -= amount;
			year++;
			amount = (365 + IsLeapYear(year)) * 24 * 3600;
		}

		month = 1;
		amount = GetDaysCountInMonth(month, year) * 24 * 3600;
		while (tmp >= amount) {
			tmp -= amount;
			month++;
			amount = GetDaysCountInMonth(month, year) * 24 * 3600;
		}

		day = 1 + tmp / (24 * 3600);
		tmp -= (day - 1)*(24 * 3600);

		hour = tmp / (3600);
		tmp -= hour * 3600;

		minute = tmp / 60;
		tmp -= minute * 60;

		second = tmp;
	}

	void ConvertTimeSpanToSecondsForPlus(fooldatetime date) {
		dateInSeconds = day * 24 * 3600 + hour * 3600 + minute * 60 + second;

		fooldatetime point = date;
		point.month = 2;
		point.day = 28;
		int yeartmp = date.year;
		if (date > point) {
			yeartmp++;
		}

		for (int i = 0; i < yeartmp; i++)
		{
			dateInSeconds += (365 + IsLeapYear(yeartmp + i)) * 24 * 3600;
			date.year++;
		}

		for (int i = 0; i < month; i++) {

			dateInSeconds += GetDaysCountInMonth(date.month, date.year) * 24 * 3600;
			date.month++;
			if (date.month > 12) {
				date.month = 1;
				date.year++;
			}
		}

	}

	void ConvertTimeSpanToSecondsForMinus(fooldatetime date) {
		dateInSeconds = day * 24 * 3600 + hour * 3600 + minute * 60 + second;

		fooldatetime point = date;
		point.month = 2;
		point.day = 28;
		int yeartmp = date.year;
		if (date <= point) {
			yeartmp--;
		}

		for (int i = 0; i < yeartmp; i++)
		{
			dateInSeconds += (365 + IsLeapYear(yeartmp - i)) * 24 * 3600;
			date.year--;
		}

		for (int i = 0; i < month; i++) {
			date.month--;
			if (date.month < 1) {
				date.month = 12;
				date.year--;
			}
			dateInSeconds += GetDaysCountInMonth(date.month, date.year) * 24 * 3600;

		}
	}

	void GetDateTimeDifference(fooldatetime startdate) {
		fooldatetime point = startdate;
		point.month = 2;
		point.day = 28;
		long long difference = dateInSeconds;
		year = 0;
		month = 0;
		int yeartmp = startdate.year;
		if (startdate > point) {
			yeartmp++;
		}
		while (difference >= (365 + IsLeapYear(yeartmp)) * 24 * 3600) {
			difference -= (365 + IsLeapYear(yeartmp)) * 24 * 3600;
			yeartmp++;
			year += 1;
			startdate.year++;
		}

		while (difference >= GetDaysCountInMonth(startdate.month, startdate.year) * 24 * 3600) {
			difference -= GetDaysCountInMonth(startdate.month, startdate.year) * 24 * 3600;
			month += 1;
			startdate.month += 1;
			if (startdate.month > 12) {
				startdate.month = 1;
				startdate.year += 1;
			}
		}

		day = difference / 3600 / 24;
		difference -= day * 24 * 3600;

	    hour = difference / 3600;
		difference -= hour * 3600;

		minute = difference / 60;
		difference -= minute * 60;

		second = difference;
	}

	int GetDataInSeconds() {
		return dateInSeconds;
	}

	void SetDataInSeconds(int data) {
		dateInSeconds = data;
	}

	bool isCorrectGrig() {
		if (second < 0 || second>59) {
			return false;
		}
		if (minute < 0 || minute>59) {
			return false;
		}
		if (hour < 0 || hour>23) {
			return false;
		}
		if (day < 1 || day > GetDaysCountInMonth(month, year)) {
			return false;
		}
		if (month < 1 || month>12) {
			return false;
		}
		return true;
	}

	void PrintFoolDatetime() {
		std::cout << day << " / " << month << " / " << year << "    " << hour << " : " << minute << " : " << second << "\n";
	}

	void PrintDatetimeWithoutNull() {
		if (year > 0) {
			std::cout << year;
			if (year == 1) {
				std::cout << " year, ";
			}
			else {
				std::cout << " years, ";
			}
		}
		if (month > 0) {
			std::cout << month;
			if (month == 1) {
				std::cout << " month, ";
			}
			else {
				std::cout << " months, ";
			}
		}
		if (day > 0) {
			std::cout << day;
			if (day == 1) {
				std::cout << " day, ";
			}
			else {
				std::cout << " days, ";
			}
		}
		if (hour > 0) {
			std::cout << hour;
			if (hour == 1) {
				std::cout << " hour, ";
			}
			else {
				std::cout << " hours, ";
			}
		}
		if (minute > 0) {
			std::cout << minute;
			if (minute == 1) {
				std::cout << " minute, ";
			}
			else {
				std::cout << " minutes, ";
			}
		}
		if (second > 0) {
			std::cout << second;
			if (second == 1) {
				std::cout << " second ";
			}
			else {
				std::cout << " seconds ";
			}
		}
		std::cout << "\n";
	}

	void PrintAllEqualForms() {
		if (year > 0) {
			std::cout << year << " years\n";
		}

		int tmp = year * 12 + month;
		if (tmp > 0) {
			std::cout << tmp << " month\n";
		}

		tmp = dateInSeconds / 3600 / 24;
		if (tmp > 0) {
			std::cout << tmp << " days\n";
		}

		tmp = dateInSeconds / 3600;
		if (tmp > 0) {
			std::cout << tmp << " hours\n";
		}

		tmp = dateInSeconds / 60;
		if (tmp > 0) {
			std::cout << tmp << " minutes\n";
		}

		if (dateInSeconds > 0) {
			std::cout << dateInSeconds << " seconds\n";
		}
	}

	void PrintData() {
		std::string days[] = { "first", "second", "third", "fourth", "fifth", "sixth", "seventh", "eighth", "ninth", "tenth", "eleventh", "twelfth", "thirteenth", "fourteenth", "fifteenth", "sixteenth", "seventeenth", "eighteenth", "nineteenth", "twentieth", "twenty first", "twenty second", "twenty third", "twenty fourth", "twenty fifth" , "twenty sixth" , "twenty seventh" , "twenty eighth" , "twenty ninth", "thirtieth", "thirty first" };
		std::string months[] = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

		std::cout << days[day - 1] << " of " << months[month - 1] << " " << year << "\n";
	}

	void PrintTime24Format() {
		std::cout << hour << " : " << minute << " : " << second << "\n";
	}

	void PrintTime12Format() {
		if (hour > 12) {
			hour -= 12;
		}
		std::cout << hour << " : " << minute << " : " << second << "\n";
	}

	void ChangeTimeZone(int newtimezone) {
		dateInSeconds -= timezone*3600;
		dateInSeconds += newtimezone * 3600;
		this->ConvertSecondsToDate();
		timezone = newtimezone;
	}

	int GetDayOfWeek() {
		return (dateInSeconds / 3600 / 24 + 3) % 7;
	}

	int GetWeekOfYear() {
		fooldatetime point;
		point.year = 1970;
		point.month = 1;
		point.day = 1;
		point.ConvertDateToSeconds();
		int res1 = ((dateInSeconds - point.GetDataInSeconds()) / 3600 / 24 + 3) / 7;
		
		fooldatetime point2 = point;
		point2.year = year;
		point2.ConvertDateToSeconds();

		int res2 = ((point2.GetDataInSeconds() - point.GetDataInSeconds()) / 3600 / 24 + 3) / 7;
		
		return res1 - res2 + 1;
	}

	int GetWeekOfMonth() {
		fooldatetime point;
		point.year = 1970;
		
		point.month = 1;
		point.day = 1;
		point.ConvertDateToSeconds();

		fooldatetime point2;
		point2.year = year;
		point2.month = month;
		point2.day = 1;
		point2.ConvertDateToSeconds();

		int res1 = ((dateInSeconds - point.GetDataInSeconds()) / 3600 / 24 + 3) / 7;
		int res2 = ((point2.GetDataInSeconds() - point.GetDataInSeconds()) / 3600 / 24 + 3) / 7;
	    
		return res1 - res2 + 1;
	}

	void EnterDate(bool isTimeSpan = false) {
		if (isTimeSpan) {
			std::cout << "Year: ";
		}
		else {
			std::cout << "Year (since 1970): ";
		}

		std::cin >> year;

		std::cout << "Month: ";
		std::cin >> month;

		std::cout << "Day: ";
		std::cin >> day;

		std::cout << "Hour: ";
		std::cin >> hour;

		std::cout << "Minute: ";
		std::cin >> minute;

		std::cout << "Second: ";
		std::cin >> second;

		this->ConvertDateToSeconds();
	}

	int GetDaysCountInThisMonth() {
		return GetDaysCountInMonth(month, year);
	}

	int GetTimezone() {
		return timezone;
	}
};

bool operator > (fooldatetime firstdate, fooldatetime seconddate) {
	return firstdate.GetDataInSeconds() > seconddate.GetDataInSeconds();
}

bool operator >= (fooldatetime firstdate, fooldatetime seconddate) {
	return firstdate.GetDataInSeconds() >= seconddate.GetDataInSeconds();
}

bool operator < (fooldatetime firstdate, fooldatetime seconddate) {
	return firstdate.GetDataInSeconds() < seconddate.GetDataInSeconds();
}

bool operator <= (fooldatetime firstdate, fooldatetime seconddate) {
	return firstdate.GetDataInSeconds() <= seconddate.GetDataInSeconds();
}

bool operator == (fooldatetime firstdate, fooldatetime seconddate) {
	return firstdate.GetDataInSeconds() == seconddate.GetDataInSeconds();
}

fooldatetime operator - (fooldatetime firstdate, fooldatetime seconddate) {
	seconddate.ConvertTimeSpanToSecondsForMinus(firstdate);
	firstdate.SetDataInSeconds(firstdate.GetDataInSeconds() - seconddate.GetDataInSeconds());
	firstdate.ConvertSecondsToDate();
	return firstdate;
}

fooldatetime operator + (fooldatetime firstdate, fooldatetime seconddate) {
	seconddate.ConvertTimeSpanToSecondsForPlus(firstdate);
	firstdate.SetDataInSeconds(firstdate.GetDataInSeconds() + seconddate.GetDataInSeconds());
	firstdate.ConvertSecondsToDate();

	return firstdate;
}


fooldatetime GetDifference(fooldatetime firstdate, fooldatetime seconddate) {
	fooldatetime date;
	date.SetDataInSeconds(abs(firstdate.GetDataInSeconds() - seconddate.GetDataInSeconds()));
	if (seconddate < firstdate) {
		date.GetDateTimeDifference(seconddate);
	}
	else {
		date.GetDateTimeDifference(firstdate);
	}

	return date;
}

void StatisticsOfDayNumber(int day, fooldatetime startdate, fooldatetime enddate) {

	fooldatetime point;
	point.year = startdate.year;
	point.month = startdate.month;
	point.day = day;
	if (day < startdate.day) {
		point.month += 1;
		if (point.month > 12) {
			point.month = 1;
			point.year++;
		}
	}
	point.ConvertDateToSeconds();

	int times[] = { 0, 0, 0, 0, 0, 0, 0 };
	while (enddate.GetDataInSeconds() >= point.GetDataInSeconds()) {
		times[point.GetDayOfWeek()]++;

		point.SetDataInSeconds(point.GetDataInSeconds() + GetDaysCountInMonth(point.month, point.year) * 24 * 3600);
		point.month++;
		if (point.month > 12) {
			point.month = 1;
			point.year++;
		}
		
		
	}

	std::cout << "Monday: " << times[0] << "\nTuesday: " << times[1] << "\nWednesday: " << times[2] << "\nThursday: " << times[3] << "\nFriday: " << times[4] << "\nSaturday: " << times[5] << "\nSunday: " << times[6] << "\n";
}

int _main()
{
	fooldatetime date;
	std::cout << "Enter date you want to work with\n";
	date.EnterDate();

	std::string help = "Enter '0' to exit \nEnter '1' to see a list of available functions\nEnter '2' to enter a new date\nEnter '3' to check is your date correct\nEnter '4' to check is some year leap\nEnter '5' to find nearest leap year\nEnter '6' to get number of days in this month\nEnter '7' to get difference modulo with some date dates\nEnter '8' to subtract some datetime\nEnter '9' to add some datetime\nEnter '10' to print date and time\nEnter '11' to print date\nEnter '12' to print time\nEnter '13' to print time in 12-hour format\nEnter '14' to get time in other time zone\nEnter '15' to get which week of year is your date\nEnter '16' to get which week of month is your date\nEnter '17' to know whick day of week is your date\nEnter '18' to get statistics about some day number\nEnter '19' to get timezone of your date\n";
	std::cout << help;
	int userinput;

	while (true) {
		std::cout << ">>> ";
		std::cin >> userinput;

		if (userinput == 0) {
			break;
		}
		else if (userinput == 1) {
			std::cout << help;
		}
		else if (userinput == 2) {
			date.EnterDate();
		}
		else if (userinput == 3) {
			if (date.isCorrectGrig()) {
				std::cout << "Yes, your date is correct\n";
			}
			else {
				std::cout << "No, your date isn't correct\n";
			}
		}
		else if (userinput == 4) {
			int year;
			std::cout << "Enter year you want to check: ";
			std::cin >> year;
			if (IsLeapYear(year)) {
				std::cout << year << " is leap year\n";
			}
			else {
				std::cout << year << " isn't leap year\n";
			}
		}
		else if (userinput == 5) {
			int year;
			std::cout << "Enter year: ";
			std::cin >> year;

			std::cout << "Nearest leap year is " << FindNearestLeapYear(year) << "\n";
		}
		else if (userinput == 6) {
			std::cout << "In this month " << date.GetDaysCountInThisMonth() << " days\n";
		}
		else if (userinput == 7) {
			std::cout << "Enter second date\n";
			fooldatetime date2;
			date2.EnterDate();

			fooldatetime datetimespan;
			datetimespan = GetDifference(date, date2);

			std::cout << "\n\n";
			if (datetimespan.GetDataInSeconds() != 0) {
				datetimespan.PrintDatetimeWithoutNull();
				std::cout << "\nIt is equal to: \n";

				datetimespan.PrintAllEqualForms();
			}
			else {
				std::cout << "This dates are the same. Difference is 0 seconds";
			}
		}
		else if (userinput == 8) {
			std::cout << "Enter datetime to subtract\n";
			fooldatetime datetimespan;
			datetimespan.EnterDate(true);
			date = date - datetimespan;
			date.PrintFoolDatetime();
		}
		else if (userinput == 9) {
			std::cout << "Enter datetime to add\n";
			fooldatetime datetimespan;
			datetimespan.EnterDate(true);
			date = date + datetimespan;
			date.PrintFoolDatetime();
		}
		else if (userinput == 10) {
			date.PrintFoolDatetime();
		}
		else if (userinput == 11) {
			date.PrintData();
		}
		else if (userinput == 12) {
			date.PrintTime24Format();
		}
		else if (userinput == 13) {
			date.PrintTime12Format();
		}
		else if (userinput == 14) {
			int timezone;
			std::cout << "Enter timezone: ";
			std::cin >> timezone;
			std::cout << "GMT(" << timezone << "): ";
			date.ChangeTimeZone(timezone);
			date.PrintFoolDatetime();
		}
		else if (userinput == 19) {
			std::cout << "Now date in timezone GMT(" << date.GetTimezone() << ")\n";
		}
		else if (userinput == 15) {
			std::cout << "Your date is " <<date.GetWeekOfYear() << " week of year\n";
		}
		else if (userinput == 16) {
			std::cout << "Your date is " << date.GetWeekOfMonth() << " week of month\n";
		}
		else if (userinput == 17) {
			std::string daysofweek[] = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
			std::cout << "Your date is " + daysofweek[date.GetDayOfWeek()] << "\n";
		}
		else if (userinput == 18) {
			int day;
			bool flag = false;
			fooldatetime datetime1(1970);
			fooldatetime datetime2(2021);
			std::cout << "Enter day number: ";
			std::cin >> day;
			std::cout << "Do you want to enter time limits? (Enter 1 if yes and 0 is not) \n";
			std::cin >> flag;
			if (flag) {
				std::cout << "Enter start\n";
				datetime1.EnterDate();
				std::cout << "Enter end\n";
				datetime2.EnterDate();
			}
			else {
				std::cout << "You will get statistics from 1970 to 2021 years\n";
			}
			StatisticsOfDayNumber(day, datetime1, datetime2);
		}

		else {
			std::cout << "This command isn't exist, enter '1' to see a list of available functions \n";
		}
	}

	return 0;
}



