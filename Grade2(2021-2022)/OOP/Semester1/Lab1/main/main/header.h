#ifndef HEADER_H
#define HEADER_H


class fooldatetime {
private:
	int year, month, day, hour, minute, second, timezone;
	long long int dateInSeconds;

public:
	fooldatetime();
	fooldatetime(int yearval);
	fooldatetime(int yeardata, int monthdata, int daydata, int hourdata, int minutedata, int seconddata);
	fooldatetime(int yeardata, int monthdata, int daydata);


	friend fooldatetime operator + (fooldatetime first, fooldatetime second);
	friend fooldatetime operator - (fooldatetime first, fooldatetime second);
	friend bool operator > (fooldatetime first, fooldatetime second);
	friend bool operator >= (fooldatetime first, fooldatetime second);
	friend bool operator < (fooldatetime first, fooldatetime second);
	friend bool operator <= (fooldatetime first, fooldatetime second);
	friend bool operator == (fooldatetime first, fooldatetime second);
	friend bool operator != (fooldatetime first, fooldatetime second);

	friend void StatisticsOfDayNumber(int day, fooldatetime startdate, fooldatetime enddate);
	
	void ConvertDateToSeconds();
	void ConvertSecondsToDate();

	void ConvertTimeSpanToSecondsForPlus(fooldatetime date);

	void ConvertTimeSpanToSecondsForMinus(fooldatetime date);
	void GetDateTimeDifference(fooldatetime startdate);

	int GetDataInSeconds();

	void SetDataInSeconds(int data);

	bool isCorrectGrig();

	void PrintFoolDatetime(); 

	void PrintDatetimeWithoutNull();
	void PrintAllEqualForms();

	void PrintData();

	void PrintTime24Format();

	void PrintTime12Format();

	void ChangeTimeZone(int newtimezone);

	int GetDayOfWeek();

	int GetWeekOfYear();

	int GetWeekOfMonth();

	void EnterDate(bool isTimeSpan = false);

	int GetDaysCountInThisMonth();

	int GetTimezone();
};


#endif