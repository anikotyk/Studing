#ifndef HEADER_H
#define HEADER_H


class datetime {
private:
	int year, month, day, hour, minute, second, timezone;
	long long int dateInSeconds;

public:
	datetime();
	datetime(int yearval);
	datetime(int yeardata, int monthdata, int daydata, int hourdata, int minutedata, int seconddata);
	datetime(int yeardata, int monthdata, int daydata);


	friend datetime operator + (datetime first, datetime second);
	friend datetime operator - (datetime first, datetime second);
	friend bool operator > (datetime first, datetime second);
	friend bool operator >= (datetime first, datetime second);
	friend bool operator < (datetime first, datetime second);
	friend bool operator <= (datetime first, datetime second);
	friend bool operator == (datetime first, datetime second);
	friend bool operator != (datetime first, datetime second);

	friend void StatisticsOfDayNumber(int day, datetime startdate, datetime enddate);
	
	void ConvertDateToSeconds();
	void ConvertSecondsToDate();

	void ConvertTimeSpanToSecondsForPlus(datetime date);

	void ConvertTimeSpanToSecondsForMinus(datetime date);
	void GetDateTimeDifference(datetime startdate);

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