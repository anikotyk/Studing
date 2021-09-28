#ifndef HEADER_H
#define HEADER_H

class datetimespan;


class datetime {
private:
	int year, month, day, hour, minute, second, timezone;
	long long int dateInSeconds;

public:
	datetime();
	datetime(int yearval);
	datetime(int yeardata, int monthdata, int daydata, int hourdata, int minutedata, int seconddata);
	datetime(int yeardata, int monthdata, int daydata);


	friend datetime operator + (datetime first, datetimespan second);

	friend datetime operator - (datetime first, datetimespan second);
	friend datetimespan operator - (datetime first, datetime second);
	friend bool operator > (datetime &first, datetime &second);
	friend bool operator >= (datetime &first, datetime &second);
	friend bool operator < (datetime first, datetime second);
	friend bool operator <= (datetime &first, datetime &second);
	friend bool operator == (datetime first, datetime second);
	friend bool operator != (datetime &first, datetime &second);
	datetime& operator = (const datetime &date);

	friend void StatisticsOfDayNumber(int day, datetime startdate, datetime enddate);
	friend class datetimespan;

	void ConvertDateToSeconds();
	void ConvertSecondsToDate();


	long long int GetDataInSeconds();

	void SetDataInSeconds(int data);

	bool isCorrectGrig();

	void PrintFoolDatetime(); 

	void PrintData();

	void PrintTime24Format();

	void PrintTime12Format();

	void ChangeTimeZone(int newtimezone);

	int GetDayOfWeek();

	int GetWeekOfYear();

	int GetWeekOfMonth();

	void EnterDate(bool isGrig = false);

	int GetDaysCountInThisMonth();

	int GetTimezone();
	bool CheckDateCorrect();

};

void UserInterfaceDateTime();

#endif