#include "stdafx.h"
#include <iostream>
#include <string>

struct datetime {
	long long int seconds = 0;
};


struct fooldatetime {
	int year = 0;
	int month = 0;
	int day = 0;
	int hour = 0;
	int minute = 0;
	int second = 0;
};

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

datetime ConvertDateToSeconds(fooldatetime dateToConvert) {
	datetime date;
	int tmp = 0;

	for (int i = 1970; i < dateToConvert.year; i++) {
		if (IsLeapYear(i)) {
			tmp += 366;
		}
		else {
			tmp += 365;
		}
	}
	date.seconds += tmp * 24 * 3600;

	tmp = 0;
	for (int i = 1; i < dateToConvert.month; i++) {
		tmp += GetDaysCountInMonth(i, dateToConvert.year);
	}
	date.seconds += tmp * 24 * 3600;

	date.seconds += (dateToConvert.day - 1) * 24 * 3600;

	date.seconds += dateToConvert.second + dateToConvert.minute * 60 + dateToConvert.hour * 3600;

	return date;
}

fooldatetime ConvertSecondsToDate(datetime dateToConvert) {
	fooldatetime date;
	int tmp = dateToConvert.seconds;
	date.year = 1970;
	int amount = (365 + IsLeapYear(date.year)) * 24 * 3600;
	while (tmp >= amount) {
		tmp -= amount;
		date.year++;
		amount = (365 + IsLeapYear(date.year)) * 24 * 3600;
	}

	date.month = 1;
	amount = GetDaysCountInMonth(date.month, date.year) * 24 * 3600;
	while (tmp >= amount) {
		tmp -= amount;
		date.month++;
		amount = GetDaysCountInMonth(date.month, date.year) * 24 * 3600;
	}

	date.day = 1 + tmp / (24 * 3600);
	tmp -= (date.day - 1)*(24 * 3600);

	date.hour = tmp / (3600);
	tmp -= date.hour * 3600;

	date.minute = tmp / 60;
	tmp -= date.minute * 60;

	date.second = tmp;

	return date;
}

bool operator > (datetime firstdate, datetime seconddate) {
	return firstdate.seconds > seconddate.seconds;
}

bool operator > (fooldatetime firstdate, fooldatetime seconddate) {
	return ConvertDateToSeconds(firstdate) > ConvertDateToSeconds(seconddate);
}

bool operator >= (datetime firstdate, datetime seconddate) {
	return firstdate.seconds >= seconddate.seconds;
}

bool operator >= (fooldatetime firstdate, fooldatetime seconddate) {
	return ConvertDateToSeconds(firstdate) >= ConvertDateToSeconds(seconddate);
}

bool operator < (datetime firstdate, datetime seconddate) {
	return firstdate.seconds < seconddate.seconds;
}

bool operator < (fooldatetime firstdate, fooldatetime seconddate) {
	return ConvertDateToSeconds(firstdate) < ConvertDateToSeconds(seconddate);
}


bool operator <= (datetime firstdate, datetime seconddate) {
	return firstdate.seconds <= seconddate.seconds;
}

bool operator <= (fooldatetime firstdate, fooldatetime seconddate) {
	return ConvertDateToSeconds(firstdate) <= ConvertDateToSeconds(seconddate);
}


bool operator == (datetime firstdate, datetime seconddate) {
	return firstdate.seconds == seconddate.seconds;
}

bool operator == (fooldatetime firstdate, fooldatetime seconddate) {
	return ConvertDateToSeconds(firstdate) == ConvertDateToSeconds(seconddate);
}

datetime ConvertTimeSpanToSecondsForPlus(fooldatetime date, fooldatetime timespan) {
	datetime res;
	res.seconds = timespan.day * 24 * 3600 + timespan.hour * 3600 + timespan.minute * 60 + timespan.second;

	fooldatetime point = date;
	point.month = 2;
	point.day = 28;
	int year = date.year;
	if (date > point) {
		year++;
	}

	for (int i = 0; i < timespan.year; i++)
	{
		res.seconds += (365 + IsLeapYear(year + i)) * 24 * 3600;
		date.year++;
	}

	for (int i = 0; i < timespan.month; i++) {

		res.seconds += GetDaysCountInMonth(date.month, date.year) * 24 * 3600;
		date.month++;
		if (date.month > 12) {
			date.month = 1;
			date.year++;
		}
	}

	
	return res;
}

datetime ConvertTimeSpanToSecondsForMinus(fooldatetime date, fooldatetime timespan) {
	datetime res;
	res.seconds = timespan.day * 24 * 3600 + timespan.hour * 3600 + timespan.minute * 60 + timespan.second;

	fooldatetime point = date;
	point.month = 2;
	point.day = 28;
	int year = date.year;
	if (date <= point) {
		year--;
	}

	for (int i = 0; i < timespan.year; i++)
	{
		res.seconds += (365 + IsLeapYear(year - i)) * 24 * 3600;
		date.year--;
	}

	for (int i = 0; i < timespan.month; i++) {
		date.month--;
		if (date.month < 1) {
			date.month = 12;
			date.year--;
		}
		res.seconds += GetDaysCountInMonth(date.month, date.year) * 24 * 3600;
		
	}

	
	return res;
}




datetime operator - (datetime firstdate, datetime seconddate) {
	datetime res;
	res.seconds = firstdate.seconds - seconddate.seconds;
	return res;
}

fooldatetime operator - (fooldatetime firstdate, fooldatetime seconddate) {
	return ConvertSecondsToDate(ConvertDateToSeconds(firstdate) - ConvertTimeSpanToSecondsForMinus(firstdate, seconddate));
}


datetime operator + (datetime firstdate, datetime seconddate) {
	datetime res;
	res.seconds = firstdate.seconds + seconddate.seconds;
	return res;
}

fooldatetime operator + (fooldatetime firstdate, fooldatetime seconddate) {
	return ConvertSecondsToDate(ConvertDateToSeconds(firstdate) + ConvertTimeSpanToSecondsForPlus(firstdate, seconddate));
}







bool isCorrectGrig(fooldatetime date) {
	if (date.second < 0 || date.second>59) {
		return false;
	}
	if (date.minute < 0 || date.minute>59) {
		return false;
	}
	if (date.hour < 0 || date.hour>23) {
		return false;
	}
	if (date.day < 1 || date.day > GetDaysCountInMonth(date.month, date.year)) {
		return false;
	}
	if (date.month < 1 || date.month>12) {
		return false;
	}
	return true;
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

void PrintFoolDatetime(fooldatetime date) {
	std::cout << date.day << " / " << date.month << " / " << date.year << "    " << date.hour << " : " << date.minute << " : " << date.second << "\n";
}

void PrintDatetimeWithoutNull(fooldatetime date) {
	if (date.year > 0) {
		std::cout << date.year;
		if (date.year == 1) {
			std::cout << " year, ";
		}
		else {
			std::cout << " years, ";
		}
	}
	if (date.month > 0) {
		std::cout << date.month;
		if (date.month == 1) {
			std::cout << " month, ";
		}
		else {
			std::cout << " months, ";
		}
	}
	if (date.day > 0) {
		std::cout << date.day;
		if (date.day == 1) {
			std::cout << " day, ";
		}
		else {
			std::cout << " days, ";
		}
	}
	if (date.hour > 0) {
		std::cout << date.hour;
		if (date.hour == 1) {
			std::cout << " hour, ";
		}
		else {
			std::cout << " hours, ";
		}
	}
	if (date.minute > 0) {
		std::cout << date.minute;
		if (date.minute == 1) {
			std::cout << " minute, ";
		}
		else {
			std::cout << " minutes, ";
		}
	}
	if (date.second > 0) {
		std::cout << date.second;
		if (date.second == 1) {
			std::cout << " second ";
		}
		else {
			std::cout << " seconds ";
		}
	}
	std::cout << "\n";
}

void PrintData(fooldatetime date) {
	std::string days[] = { "first", "second", "third", "fourth", "fifth", "sixth", "seventh", "eighth", "ninth", "tenth", "eleventh", "twelfth", "thirteenth", "fourteenth", "fifteenth", "sixteenth", "seventeenth", "eighteenth", "nineteenth", "twentieth", "twenty first", "twenty second", "twenty third", "twenty fourth", "twenty fifth" , "twenty sixth" , "twenty seventh" , "twenty eighth" , "twenty ninth", "thirtieth", "thirty first" };
	std::string months[] = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

	std::cout << days[date.day - 1] << " of " << months[date.month - 1] << " " << date.year << "\n";
}

void PrintTime24Format(fooldatetime date) {
	std::cout << date.hour << " : " << date.minute << " : " << date.second << "\n";
}

void PrintTime12Format(fooldatetime date) {
	if (date.hour > 12) {
		date.hour -= 12;
	}
	std::cout << date.hour << " : " << date.minute << " : " << date.second << "\n";
}





fooldatetime EnterDate(bool isTimeSpan=false) {
	fooldatetime date;
	if (isTimeSpan) {
		std::cout << "Year: ";
	}
	else {
		std::cout << "Year (since 1970): ";
	}
	
	std::cin >> date.year;

	std::cout << "Month: ";
	std::cin >> date.month;

	std::cout << "Day: ";
	std::cin >> date.day;

	std::cout << "Hour: ";
	std::cin >> date.hour;

	std::cout << "Minute: ";
	std::cin >> date.minute;

	std::cout << "Second: ";
	std::cin >> date.second;

	return date;
}

datetime TimeZone(datetime date, int timezone) {
	date.seconds += timezone * 3600;
	return date;
}

int GetDayOfWeek(datetime date) {
	return (date.seconds/3600/24 + 3) % 7;
}

int GetWeekOfYear(datetime date, fooldatetime fooldate) {
	fooldatetime point;
	point.year = 1970;
	point.month = 1;
	point.day = 1;

	int res1 = ((date - ConvertDateToSeconds(point)).seconds /3600/24 + 3)/7;

	fooldatetime point2 = point;
	point2.year = fooldate.year;

	int toadd = 0;
	int res2 =((ConvertDateToSeconds(point2) - ConvertDateToSeconds(point)).seconds / 3600 / 24 + 3)/7;
		
	return res1 - res2 + 1;
}

int GetWeekOfMonth(fooldatetime date, datetime dateSeconds) {
	fooldatetime point;
	point.year = 1970;
	point.month = 1;
	point.day = 1;

	fooldatetime point2;
	point2.year = date.year;
	point2.month = date.month;
	point2.day = 1;

	int toadd = 0;
	int res1 = ((dateSeconds - ConvertDateToSeconds(point)).seconds / 3600 / 24 + 3)/7;
	int res2 = ((ConvertDateToSeconds(point2) - ConvertDateToSeconds(point)).seconds / 3600 / 24 + 3) / 7;

	return res1-res2 + 1;
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

	datetime pointSeconds = ConvertDateToSeconds(point);
	datetime enddateseconds = ConvertDateToSeconds(enddate);

	int times[] = { 0, 0, 0, 0, 0, 0, 0 };
	while (enddateseconds >= pointSeconds) {
		times[GetDayOfWeek(pointSeconds)]++;

		pointSeconds.seconds += GetDaysCountInMonth(point.month, point.year) * 24 * 3600;
		point.month++;
		if (point.month > 12) {
			point.month = 1;
			point.year++;
		}
		
		
	}


	std::cout << "Monday: " << times[0] << "\nTuesday: " << times[1] << "\nWednesday: " << times[2] << "\nThursday: " << times[3] << "\nFriday: " << times[4] << "\nSaturday: " << times[5] << "\nSunday: " << times[6] << "\n";
}

fooldatetime GetDateTimeDifference(fooldatetime startdate, datetime difference) {
	fooldatetime res;

	fooldatetime point = startdate;
	point.month = 2;
	point.day = 28;

	int year = startdate.year;
	if (startdate > point) {
		year++;
	}
	while (difference.seconds >= (365 + IsLeapYear(year)) * 24 * 3600) {
		difference.seconds -= (365 + IsLeapYear(year)) * 24 * 3600;
		year++;
		res.year += 1;
		startdate.year++;
	}

	while (difference.seconds >= GetDaysCountInMonth(startdate.month, startdate.year)*24*3600){
		difference.seconds -= GetDaysCountInMonth(startdate.month, startdate.year) * 24 * 3600;
		res.month += 1;
		startdate.month += 1;
		if (startdate.month > 12) {
			startdate.month = 1;
			startdate.year += 1;
		}
	}

	res.day = difference.seconds / 3600 / 24;
	difference.seconds -= res.day * 24 * 3600;

	std::cout << difference.seconds << "\n";
	res.hour = difference.seconds / 3600 ;
	difference.seconds -= res.hour * 3600;

	res.minute = difference.seconds / 60;
	difference.seconds -= res.minute * 60;

	res.second = difference.seconds ;

	return res;
}



int main()
{
	fooldatetime date;
	datetime dateInSeconds;
	std::cout << "Enter date you want to work with\n";
	date = EnterDate();
	dateInSeconds = ConvertDateToSeconds(date);

	std::string help = "Enter '0' to exit \nEnter '1' to see a list of available functions\nEnter '2' to enter a new date\nEnter '3' to check is your date correct\nEnter '4' to check is some year leap\nEnter '5' to find nearest leap year\nEnter '6' to get number of days in this month\nEnter '7' to get difference modulo with some date dates\nEnter '8' to subtract some datetime\nEnter '9' to add some datetime\nEnter '10' to print date and time\nEnter '11' to print date\nEnter '12' to print time\nEnter '13' to print time in 12-hour format\nEnter '14' to get time in other time zone\nEnter '15' to get which week of year is your date\nEnter '16' to get which week of month is your date\nEnter '17' to know whick day of week is your date\nEnter '18' to get statistics about some day number\n";
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
			date = EnterDate();
			dateInSeconds = ConvertDateToSeconds(date);
		}
		else if (userinput == 3) {
			if (isCorrectGrig(date)) {
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
			std::cout << "Nearest leap year is " << FindNearestLeapYear(date.year) << "\n";
		}
		else if (userinput == 6) {
			std::cout << "In this month " << GetDaysCountInMonth(date.month, date.year) << " days\n";
		}
		else if (userinput == 7) {
			std::cout << "Enter second date\n";
			fooldatetime date2 = EnterDate();
			datetime date2InSeconds = ConvertDateToSeconds(date2);
			datetime datetimedelta;
			fooldatetime datetimespan;

			

			if (dateInSeconds >= date2InSeconds) {
				datetimedelta = dateInSeconds - date2InSeconds;
				datetimespan = GetDateTimeDifference(date2, datetimedelta);
			}
			else {
				datetimedelta = date2InSeconds - dateInSeconds;
				datetimespan = GetDateTimeDifference(date, datetimedelta);
			}

			std::cout << "\n\n";
			PrintDatetimeWithoutNull(datetimespan);
			std::cout << "\nIt is equal to: \n";

			std::cout << datetimedelta.seconds << " seconds\n";
			std::cout << datetimedelta.seconds/60 << " minutes\n";
			std::cout << datetimedelta.seconds/3600 << " hours\n";
			std::cout << datetimedelta.seconds/3600/24 << " days\n";
			std::cout << datetimespan.year * 12 + datetimespan.month << " month\n";
			std::cout << datetimespan.year << " years\n";
			

		}
		else if (userinput == 8) {
			std::cout << "Enter datetime to subtract\n";
			fooldatetime datetimespan = EnterDate(true);

			date = date - datetimespan;
			dateInSeconds = ConvertDateToSeconds(date);
			PrintFoolDatetime(date);
		}
		else if (userinput == 9) {
			std::cout << "Enter datetime to add\n";
			fooldatetime datetimespan = EnterDate(true);
			date = date + datetimespan;
			dateInSeconds = ConvertDateToSeconds(date);
			PrintFoolDatetime(date);
		}
		else if (userinput == 10) {
			PrintFoolDatetime(date);
		}
		else if (userinput == 11) {
			PrintData(date);
		}
		else if (userinput == 12) {
			PrintTime24Format(date);
		}
		else if (userinput == 13) {
			PrintTime12Format(date);
		}
		else if (userinput == 14) {
			int timezone;
			std::cout << "Enter timezone: ";
			std::cin >> timezone;
			std::cout << "GMT(" << timezone << "): ";
			PrintFoolDatetime(ConvertSecondsToDate(TimeZone(dateInSeconds, timezone)));

		}
		else if (userinput == 15) {
			std::cout << "Your date is " << GetWeekOfYear(dateInSeconds, date) << " weak of year\n";
		}
		else if (userinput == 16) {
			std::cout << "Your date is " << GetWeekOfMonth(date, dateInSeconds) << " weak of month\n";
		}
		else if (userinput == 17) {
			std::string daysofweek[] = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
			std::cout << "Your date is " + daysofweek[GetDayOfWeek(dateInSeconds)] << "\n";
		}
		else if (userinput == 18) {
			int day;
			bool flag = false;
			fooldatetime datetime1;
			fooldatetime datetime2;
			std::cout << "Enter day number: ";
			std::cin >> day;
			std::cout << "Do you want to enter time limits? (Enter 1 if yes and 0 is not) \n";
			std::cin >> flag;
			if (flag) {
				std::cout << "Enter start\n";
				datetime1 = EnterDate();
				std::cout << "Enter end\n";
				datetime2 = EnterDate();
			}
			else {
				datetime2.year = 2021;
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



