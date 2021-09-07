#include "stdafx.h"
#include <iostream>
#include <time.h>

using namespace std;

// return the longest common substring of two strings
char *lcsubstr(const char *s_1, const char *s_2)
{
	const char *lcs = 0, *s1 = s_1, *s2 = s_2;

	int max = 0, l1 = strlen(s_1), l2 = strlen(s_2);

	// s1 = s2, s2 = s1 if l1 < l2
	if (l1 < l2) {
		s1 = s_2; 
		s2 = s_1;
		l1 += l2; 
		l2 = l1 - l2; 
		l1 -= l2;
	}

	for (int i = 0; i < (l1 + 1); i++) {
		int cnt = 0;
		for (int j = 0; j < l2; j++) {
			if (s1[(i + j) % (l1 + 1)] == s2[j]) {
				if (++cnt > max) {
					lcs = &s2[j - cnt + 1];
					max = cnt;
				}
			}
			else cnt = 0;
		}
	}

	if (max == 0) {
		return 0;
	}
	// copy lcs into new string
	char *str = (char*)memcpy(new char[max + 1], lcs, max);
	str[max] = 0;    // terminate string with 0
	
	return str;
}


int main()
{
	char *s1 = new char[100];
	char *s2 = new char[100];
	//cin >> s1;
	//cin >> s2;
	srand(time(NULL));
	for (int i = 0; i < 20; i++) {

		s1[i] = rand() % 2 + '0';
		s2[i] = rand() % 2 + '0';
	}
	s1[20] = '\0';
	s2[20] = '\0';
	cout << s1 << "\n" << s2 << "\n";
	

	char *lcs = lcsubstr(s1, s2);

	if (lcs == 0) {
		cout << "There are no common subsrtings\n\n";
	}
	else {
	cout << "\nlongest common substring of " << strlen(lcs) << " symbols is: '" << lcs << "'\n\n";
	}
	delete[] lcs;

    return 0;
}
