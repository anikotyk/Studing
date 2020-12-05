#include "stdafx.h"
#include <iostream>
#include <stack>

using namespace std;

const double pi = 2 * acos(0);

char* clear(char *array) {
	for (int i = 0; i < strlen(array); i++) {
		array[i] = ' ';
	}
	return array;
}

char* scobk(char *data, int i) {
	int cnt = 1;
	char *substring = new char[strlen(data)];
	for (int k = 0; k < strlen(data); k++) {
		if (data[i] == '(') {
			cnt++;
		}
		else if (data[i] == ')') {
			cnt--;
		}
		if (cnt != 0) {
			substring[k] = data[i];
		}
		else {
			substring[k] = '\0';
			break;
		}
		i++;
	}
	return substring;
}


double parse(char *data) {
	stack <char> oper;
	int len = strlen(data);
	bool flag = false;
	int j1 = 0;
	int j3 = 0;
	double *numbers = new double[len];
	char *element = new char[len] { ' ' };
	char *substring = new char[len];
	char *diya = new char[len];
	int cnt_diya = 0;

	for (int i = 0; i <= len; i++) {
		if (i == len) {
			flag = true;
		}
		if (flag && element[0] != ' ') {
			numbers[j3] = atof(element);
			diya[cnt_diya++] = 'x';
			clear(element);
			j1 = 0;
			j3++;
			flag = false;
		}
		if (i == len) {
			while (!(oper.empty())) {
				diya[cnt_diya++] = oper.top();
				oper.pop();
			}
		}
		if ((data[i] >= 48 && data[i] <= 57) || data[i] == '.') {
			element[j1++] = data[i];
			if ((data[i + 1] >= 48 && data[i+1] <= 57) || data[i+1] == '.'){
				flag = false;
			}else {
				flag = true;
			}
		}
		else if (data[i] == '(') {
			i++;
			substring=scobk(data,i);
			i += strlen(substring);
			numbers[j3++]= parse(substring);
			diya[cnt_diya++] = 'x';
			clear(substring);
		}
		else if(i != len) {
			if(i==0 || !((data[i - 1] >= 48 && data[i - 1] <= 57) or data[i - 1]==')')){
				if (data[i] == '-') {
					if (data[i + 1] == '(') {
						i+=2;
						substring = scobk(data, i);
						i += strlen(substring);
						numbers[j3++] = (parse(substring))*(-1);
						diya[cnt_diya++] = 'x';
						clear(substring);
					}
					else {
						element[j1++] = data[i];
						flag = false;
					}
					continue;
				}
				else if (data[i] == 's') {
					i += 4;
					substring = scobk(data, i);
					i += strlen(substring);
					numbers[j3++] = sin(parse(substring)*pi/180);
					diya[cnt_diya++] = 'x';
					clear(substring);
					continue;
				}
				else if (data[i] == 'c') {
					i += 4;
					substring = scobk(data, i);
					i += strlen(substring);
					numbers[j3++] = cos(parse(substring)*pi/180);
					diya[cnt_diya++] = 'x';
					clear(substring);
					continue;
				}
			}else if (!(oper.empty())) {
				if ((data[i] == '+' || data[i] == '-') && (oper.top() == '*' || oper.top() == '%')) {
					diya[cnt_diya++] = oper.top();
					oper.pop();
				}else if ((data[i] == '*' || data[i] == '%') && (oper.top() == '*' || oper.top() == '%')) {
					diya[cnt_diya++] = oper.top();
					oper.pop();
				}
				else if ((data[i] == '+' || data[i] == '-') && (oper.top() == '+' || oper.top() == '-')) {
					diya[cnt_diya++] = oper.top();
					oper.pop();
				}
			}
			oper.push(data[i]);
			flag = true;
		}
	}
	stack <double> steck;
	double a;
	double b;
	int cnt = 0;
	/*for (int i = 0; i < cnt_diya; i++) {
		cout << diya[i];
	}
	cout << "\n";*/
	
	for (int i = 0; i < cnt_diya; i++) {
		if (diya[i] == 'x') {
			steck.push(numbers[cnt]);
			cnt++;
		}
		else {
			b = steck.top();
			steck.pop();
			a = steck.top();
			steck.pop();

			if (diya[i] == '*') {
				steck.push(a*b);
			}else if(diya[i] == '%') {
				if (b == 0) {
					cout << "cannot be divisible by 0 \n";
					return 0;
				}
				else {
					steck.push(fmod(a, b));
				}

			}
			else if (diya[i] == '+') {
				steck.push(a+b);
			}
			else if (diya[i] == '-') {
				steck.push(a-b);
			}
		}
	}
	return steck.top();
}

int main()
{
	char *data = new char[100];
	cin >> data;
	cout<<parse(data)<< "\n\n";
	return 0;
}
