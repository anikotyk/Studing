#include "stdafx.h"
#include <iostream>
#include <stdio.h>

using namespace std;

int gcd(int a, int b) {
	a = abs(a);
	b = abs(b);
	while (a != b) {
		if (a > b) {
			int c = a;
			a = b;
			b = c;
		}
		b = b - a;
	}
	return a;
}

struct Rnumber {
	int znam;
	int chis;
};

int main()
{
	int n;
	cout << "Size of matrix: ";
	cin >> n;
	Rnumber **m1 = new Rnumber*[n];
	Rnumber **m2 = new Rnumber*[n];
	Rnumber **res = new Rnumber*[n];
	for (int i = 0; i < n; i++) {
		m1[i] = new Rnumber[n];
		m2[i] = new Rnumber[n];
		res[i] = new Rnumber[n];
	}

	FILE *fp;
	if ((fp = fopen("data.txt", "r")) == NULL)
	{
		return 0;
	}
	int y;
	char sign;
	int x;
	for (int i = 0; i < n; i++) {
		for (int j = 0; j < n; j++) {
			fscanf(fp, "%d", &x);
			fscanf(fp, "%c", &sign);
			fscanf(fp, "%d", &y);
			fscanf(fp, "%c", &sign);
			m1[i][j].chis = x;
			m1[i][j].znam = y;
		}
	}

	for (int i = 0; i < n; i++) {
		for (int j = 0; j < n; j++) {
			fscanf(fp, "%d", &x);
			fscanf(fp, "%c", &sign);
			fscanf(fp, "%d", &y);
			fscanf(fp, "%c", &sign);
			m2[i][j].chis = x;
			m2[i][j].znam = y;
		}
	}

	int res_element_c;
	int res_element_z;
	for (int i = 0; i < n; i++) {
		for (int j = 0; j < n; j++) {
			res_element_c = 0;
			res_element_z = 1;
			for (int i1 = 0; i1 < n; i1++) {
				res_element_z *= m1[i][i1].znam * m2[i1][j].znam;
			}
			for (int i1 = 0; i1 < n; i1++) {
				res_element_c += (m1[i][i1].chis * m2[i1][j].chis)*res_element_z/(m1[i][i1].znam * m2[i1][j].znam);
			}
			int gcd1 = gcd(res_element_c, res_element_z);
			res[i][j].chis = res_element_c/gcd1;
			res[i][j].znam = res_element_z/gcd1;
			cout << res[i][j].chis << "/" << res[i][j].znam << " ";
		}
		cout << "\n";
	}
	
	for (int i = 0; i < n; i++) {
		delete[]m1[i];
		delete[]m2[i];
		delete[]res[i];
	}

	fclose(fp);

	return 0;
}
