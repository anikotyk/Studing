#include "stdafx.h"
#include <iostream>

int **get_count_edges(bool **data, int n) {
	int **count_edges = new int*[n];

	for (int i = 0; i < n; i++) {
		count_edges[i] = new int[2];
		count_edges[i][0] = 0;
		count_edges[i][1] = i;
	}
	for (int i = 0; i < n; i++) {
		for (int j = 0; j < n; j++) {
			count_edges[i][0] += data[i][j];
		}
	}
	return count_edges;
}

int **bubble_sort(int **count_edges, int n) {
	int *temp=new int[2];

	for (int i = 0; i < n - 1; i++) {
		for (int j = 0; j < n - i - 1; j++) {
			if (count_edges[j][0] > count_edges[j + 1][0]) {
				temp = count_edges[j];
				count_edges[j] = count_edges[j + 1];
				count_edges[j + 1] = temp;
			}
		}
	}

	return count_edges;
}


int **merge(int **left, int **right, int start, int end, int center) {
	int n = end - start;
	int lc = center - start;
	int rc = end - center;

	int **arr = new int*[n];
	int l = 0;
	int r = 0;
	int c = 0;
	while (l < lc && r < rc) {
		if (left[l][0] <= right[r][0]) {
			arr[c] = left[l];
			l++;
		}
		else {
			arr[c] = right[r];
			r++;
		}
		c++;
	}
	while (l < lc) {
		arr[c] = left[l];
		l++;
		c++;
	}
	while (r < rc) {
		arr[c] = right[r];
		r++;
		c++;
	}

	return arr;
}


int **merge_sort(int **count_edges, int start, int end) {
	if (end - start == 1) {
		int **arr = new int*[1];
		arr[0] = count_edges[start];
		return arr;
	}
	int center = start + (end-start)/2;
	return merge(merge_sort(count_edges, start, center), merge_sort(count_edges, center, end), start, end, center);
}


int main()
{
	FILE *fp;
	fopen_s(&fp, "../data.txt", "r");

	int n;
	fscanf_s(fp, "%d", &n);
	bool **data = new bool*[n];
	for (int i = 0; i < n; i++) {
		data[i] = new bool[n];
	}

	for (int i = 0; i<n; i++) {
		for (int j = 0; j < n; j++) {
			fscanf_s(fp, "%d", &data[i][j]);
		}
	}
	fclose(fp);

	int **graph = get_count_edges(data, n);

	int **bubble_res = bubble_sort(graph, n);
	int **merge_res = merge_sort(graph, 0, n);

	std::cout << "Bubble sort: ";
	for (int i = 0; i < n; i++) {
		std::cout << bubble_res[i][1] << " ";
	}
	std::cout << "\n\nMerge sort: ";

	for (int i = 0; i < n; i++) {
		std::cout << merge_res[i][1] << " ";
	}
	std::cout << "\n\n";
    return 0;
}
