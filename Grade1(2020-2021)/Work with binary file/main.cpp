#include "stdafx.h"
#include <iostream>
#include <fstream>
#include <time.h>

using namespace std;

void write_file(const char *fname, int size = 1000)
{
	float data;

	ofstream file(fname, ios::out | ios::binary);

	if (file.is_open())
	{
		srand(time(NULL));
		for (int i = 1; i <= size; i++)
		{
			data = rand()%2001 + (-1000);
			cout << data << " ";
			file.write((char*)&data, sizeof(data));
			
		}
		file.close();
	}
	else cout << "Can't open file for writing\n";
}

double read_file(const char *fname)
{
	double cnt = 0, sum = 0;

	ifstream file(fname, ios::in | ios::binary);
	if (file.is_open())
	{
		float data[1];
		while (++cnt) {
			file.read((char*)data, sizeof(data));
			if (!file) {
				cnt--;
				break;
			}
			else if (data[0] < 0) {
				cnt--;
			}
			else {
				sum += data[0];
			}
		}
		file.close();
	}
	else cout << "Can't open file for reading\n";
	if (cnt == 0) {
		return -1;
	}
	return sum / cnt;
}


int main()
{
	write_file("data.bin", 5);
	cout << "\n";
	double avg = read_file("data.bin");
	if (avg < 0) {
		cout << "There are only negative numbers";
	}
	else {
		cout << "the average of even elements in the file is: " << avg << "\n";
	}
	cout << "\n\n";
	return 0;
}
