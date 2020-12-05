#include "stdafx.h"
#include <complex>
#include <iostream>

using namespace std;


complex<double> ulam_spiral_next()
{
	static int layer = 0, sideitr = 0;
	static complex<double> dir = 1i, cur = 1.0 - 1.0i;

	if (layer == 0) { 
		return layer++; 
	}

	if (++sideitr > 2 * layer) {
		// at the end of the side we turn direction
		sideitr = 1;
		// turn 90 deg
		dir *= 1i;
		// after 4 turns go to the next layer
		if (dir == 1i) { 
			cur = double(++layer) * (1.0 - 1.0i);
		}

	}
	return (cur += dir);
}


int main()
{
	int x, y;

	const int spiral_size = 9;

	char array[spiral_size][spiral_size] = {' '};

	char *input = new char[spiral_size*spiral_size];
	cin >> input;

	complex<double> c;
	// fill the array by spiral coordinates
	for (int i = 0; i < strlen(input); i++) {
		c = ulam_spiral_next();
		x = spiral_size / 2 + c.real();
		y = spiral_size / 2 - c.imag();
		array[y][x] = input[i];
	}

	// print the result
	for (int j = 0; j < spiral_size; j++) {
		for (int i = 0; i < spiral_size; i++)
			cout << array[j][i] << "\t";
		cout << "\n";
	}
	cout << "\n";
}
